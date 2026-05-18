using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Services;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Web;
using FurniComply.Web.Models;
using FurniComply.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "System.Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _db;
    private readonly SmtpMailHealthService _mailHealth;
    private readonly IPasswordResetEmailSender _passwordResetMail;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<AdminController> _logger;
    private readonly SupplierContactUniquenessService _contactUniqueness;

    public AdminController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext db,
        SmtpMailHealthService mailHealth,
        IPasswordResetEmailSender passwordResetMail,
        IWebHostEnvironment environment,
        ILogger<AdminController> logger,
        SupplierContactUniquenessService contactUniqueness)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _mailHealth = mailHealth;
        _passwordResetMail = passwordResetMail;
        _environment = environment;
        _logger = logger;
        _contactUniqueness = contactUniqueness;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DownloadBackup()
    {
        try
        {
            var backup = await CreateBackupAsync();
            return PhysicalFile(backup.FullPath, "application/octet-stream", backup.FileName);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Database backup download failed.");
            TempData["ErrorMessage"] = SafeErrorMessages.ForUser(ex, "Backup could not be created.");
            return RedirectToAction(nameof(Users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during database backup download.");
            TempData["ErrorMessage"] = SafeErrorMessages.Generic;
            return RedirectToAction(nameof(Users));
        }
    }

    [HttpGet]
    public IActionResult Backups()
    {
        var isSqlite = _db.Database.IsSqlite();
        var canCreateBackup = isSqlite;
        var databasePath = ResolveDatabaseLocation();
        var backupsDirectory = GetBackupsDirectory();
        const string searchPattern = "*.db";

        var backups = Directory.Exists(backupsDirectory)
            ? Directory.EnumerateFiles(backupsDirectory, searchPattern, SearchOption.TopDirectoryOnly)
                .Select(path => new FileInfo(path))
                .OrderByDescending(file => file.CreationTimeUtc)
                .ThenByDescending(file => file.LastWriteTimeUtc)
                .Select(file => new BackupFileSummary(
                    file.Name,
                    file.Length,
                    file.CreationTimeUtc,
                    FormatFileSize(file.Length)))
                .ToList()
            : new List<BackupFileSummary>();

        var model = new BackupHistoryViewModel
        {
            IsSqlite = isSqlite,
            CanCreateBackup = canCreateBackup,
            BackupModeLabel = isSqlite ? "SQLite backup mode" : "Backup unavailable",
            DatabaseLocation = databasePath ?? "Not configured",
            Backups = backups
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult DownloadBackupFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return NotFound();
        }

        var sanitizedFileName = Path.GetFileName(fileName);
        if (!string.Equals(fileName, sanitizedFileName, StringComparison.Ordinal))
        {
            return NotFound();
        }

        var backupPath = Path.Combine(GetBackupsDirectory(), sanitizedFileName);
        if (!System.IO.File.Exists(backupPath))
        {
            return NotFound();
        }

        return PhysicalFile(backupPath, "application/octet-stream", sanitizedFileName);
    }

    [HttpGet]
    public async Task<IActionResult> MailDiagnostics(CancellationToken cancellationToken)
    {
        var model = await _mailHealth.GetDiagnosticsAsync(cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendTestEmail(string testEmail, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(testEmail))
        {
            TempData["MailTestError"] = "Enter an email address to receive the test message.";
            return RedirectToAction(nameof(MailDiagnostics));
        }

        var sent = await _passwordResetMail.TrySendTestMessageAsync(testEmail.Trim());
        if (sent)
            TempData["MailTestSuccess"] = $"Test email sent to {testEmail.Trim()}. Check the inbox and spam folder.";
        else
            TempData["MailTestError"] = "SMTP send failed. Update Mail:* in .env and user secrets (Gmail App Password, no spaces), restart the app, and try again.";

        return RedirectToAction(nameof(MailDiagnostics));
    }

    public async Task<IActionResult> AccessRequests()
    {
        var pending = await _db.AccessRequests
            .Where(r => r.Status == AccessRequestStatus.Pending)
            .OrderBy(r => r.CreatedAtUtc)
            .ToListAsync();
        return View(pending);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveAccessRequest(Guid id)
    {
        var request = await _db.AccessRequests.FindAsync(id);
        if (request == null || request.Status != AccessRequestStatus.Pending)
            return RedirectToAction(nameof(AccessRequests));

        request.Status = AccessRequestStatus.Approved;
        request.ReviewedByUserId = _userManager.GetUserId(User);
        request.ReviewedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        TempData["AccessRequestApproved"] = request.Id;
        return RedirectToAction(nameof(CreateUser), new { email = request.Email, fullName = request.FullName, preferredRole = request.PreferredRole });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectAccessRequest(Guid id, string? rejectionReason)
    {
        var request = await _db.AccessRequests.FindAsync(id);
        if (request == null || request.Status != AccessRequestStatus.Pending)
            return RedirectToAction(nameof(AccessRequests));

        request.Status = AccessRequestStatus.Rejected;
        request.ReviewedByUserId = _userManager.GetUserId(User);
        request.ReviewedAtUtc = DateTime.UtcNow;
        request.RejectionReason = rejectionReason;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Access request from {request.Email} has been rejected.";
        return RedirectToAction(nameof(AccessRequests));
    }

    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users
            .OrderBy(u => u.Email)
            .ThenBy(u => u.UserName)
            .ToListAsync();

        var activeUsers = users
            .Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd.Value <= DateTimeOffset.UtcNow)
            .ToList();
        var archivedUsers = users
            .Where(u => u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.UtcNow)
            .ToList();

        var activeSummaries = new List<UserSummary>();
        foreach (var user in activeUsers)
        {
            // Filter out system accounts
            if (IsSystemAccount(user))
                continue;

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(IsHiddenAdministrativeRole))
                continue;

            activeSummaries.Add(new UserSummary(user.Id, user.Email ?? user.UserName ?? "unknown", userRoles.ToList(), user.TwoFactorEnabled));
        }

        var archivedSummaries = new List<UserSummary>();
        foreach (var user in archivedUsers)
        {
            // Filter out system accounts
            if (IsSystemAccount(user))
                continue;

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(IsHiddenAdministrativeRole))
                continue;

            archivedSummaries.Add(new UserSummary(user.Id, user.Email ?? user.UserName ?? "unknown", userRoles.ToList(), user.TwoFactorEnabled));
        }

        var roles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .ToListAsync();

        var roleSummaries = new List<RoleSummary>();
        foreach (var role in roles)
        {
            var roleName = role.Name ?? string.Empty;
            if (IsHiddenAdministrativeRole(roleName))
                continue;

            var userCount = (await _userManager.GetUsersInRoleAsync(roleName)).Count;
            var isProtected = string.Equals(roleName, RoleNames.SuperAdmin, StringComparison.OrdinalIgnoreCase);
            roleSummaries.Add(new RoleSummary(role.Id, roleName, userCount, isProtected));
        }
        ViewBag.RoleSummaries = roleSummaries;
        ViewBag.ArchivedUsers = archivedSummaries;
        try
        {
            ViewBag.PendingAccessRequestCount = await _db.AccessRequests.CountAsync(r => r.Status == AccessRequestStatus.Pending);
        }
        catch
        {
            // AccessRequests table may not exist if AddAccessRequests migration hasn't been applied
            ViewBag.PendingAccessRequestCount = 0;
        }

        return View(activeSummaries);
    }

    public async Task<IActionResult> CreateUser(string? email = null, string? fullName = null, string? preferredRole = null)
    {
        var allRoles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync();
        var roles = allRoles.Where(r => !IsHiddenAdministrativeRole(r)).ToList();

        var vm = new CreateUserViewModel
        {
            Email = email ?? string.Empty,
            FullName = fullName ?? string.Empty,
            Roles = roles.Select(r => new RoleSelection { Name = r, Selected = !string.IsNullOrEmpty(preferredRole) && string.Equals(r, preferredRole, StringComparison.OrdinalIgnoreCase) }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email is required.");
        }

        string passwordToUse;
        if (model.AssignmentOnly)
        {
            passwordToUse = GenerateSecureRandomPassword();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Password is required.");
            }
            else if (model.Password.Length < 12)
            {
                ModelState.AddModelError(nameof(model.Password), "Password must be at least 12 characters.");
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(model.ConfirmPassword), "Passwords do not match.");
            }
            passwordToUse = model.Password;
        }

        if (model.AssignmentOnly)
        {
            ModelState.Remove(nameof(CreateUserViewModel.Password));
            ModelState.Remove(nameof(CreateUserViewModel.ConfirmPassword));
        }

        if (!ModelState.IsValid)
        {
            var roles = (await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync())
                .Where(r => !IsHiddenAdministrativeRole(r))
                .ToList();
            model.Roles ??= roles.Select(r => new RoleSelection { Name = r, Selected = false }).ToList();
            return View(model);
        }

        if (await _contactUniqueness.IsEmailInUseAsync(model.Email.Trim()))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already in use.");
            model.Roles ??= (await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync())
                .Where(r => !IsHiddenAdministrativeRole(r))
                .Select(r => new RoleSelection { Name = r, Selected = false }).ToList();
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email.Trim(),
            Email = model.Email.Trim(),
            FullName = string.IsNullOrWhiteSpace(model.FullName) ? null : model.FullName.Trim(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, passwordToUse);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            model.Roles ??= (await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync())
                .Where(r => !IsHiddenAdministrativeRole(r))
                .Select(r => new RoleSelection { Name = r, Selected = false }).ToList();
            return View(model);
        }

        var selectedRoles = model.SelectedRoles ?? model.Roles?.Where(r => r.Selected).Select(r => r.Name).ToList() ?? new List<string>();
        selectedRoles = selectedRoles.Where(r => !IsHiddenAdministrativeRole(r)).ToList();
        if (selectedRoles.Count > 0)
        {
            await _userManager.AddToRolesAsync(user, selectedRoles);
        }

        TempData["SuccessMessage"] = model.AssignmentOnly
            ? $"Contact '{user.Email}' added. They will appear in the Assign To dropdown and receive email when assigned. (No login - assignment/notification only.)"
            : $"User '{user.Email}' created successfully. They will appear in the CAPA Assign Responsibility dropdown.";
        return RedirectToAction(nameof(Users));
    }

    private static string GenerateSecureRandomPassword()
    {
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghjkmnpqrstuvwxyz";
        const string digits = "23456789";
        const string special = "!@#$%^&*";
        var all = upper + lower + digits + special;
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        var chars = new char[16];
        chars[0] = upper[bytes[0] % upper.Length];
        chars[1] = lower[bytes[1] % lower.Length];
        chars[2] = digits[bytes[2] % digits.Length];
        chars[3] = special[bytes[3] % special.Length];
        for (var i = 4; i < 16; i++)
            chars[i] = all[bytes[i] % all.Length];
        return new string(chars);
    }

    public async Task<IActionResult> EditRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Protect system accounts
        if (IsSystemAccount(user))
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Any(IsHiddenAdministrativeRole))
        {
            return NotFound();
        }

        var roles = (await _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync())
            .Where(r => !IsHiddenAdministrativeRole(r))
            .ToList();

        var vm = new UserRolesViewModel
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? "unknown",
            Roles = roles.Select(role => new RoleSelection
            {
                Name = role,
                Selected = userRoles.Contains(role)
            }).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoles(UserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        // Protect system accounts
        if (IsSystemAccount(user))
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Any(IsHiddenAdministrativeRole))
        {
            return NotFound();
        }

        var selectedRoles = model.Roles.Where(r => r.Selected).Select(r => r.Name).ToList();
        selectedRoles = selectedRoles.Where(r => !IsHiddenAdministrativeRole(r)).ToList();
        var currentRoles = userRoles.Where(r => !IsHiddenAdministrativeRole(r)).ToList();

        var toAdd = selectedRoles.Except(currentRoles).ToList();
        var toRemove = currentRoles.Except(selectedRoles).ToList();

        if (toAdd.Count > 0)
        {
            await _userManager.AddToRolesAsync(user, toAdd);
        }

        if (toRemove.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(user, toRemove);
        }

        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return RedirectToAction(nameof(Users));
        }

        var currentUserId = _userManager.GetUserId(User);
        if (string.Equals(user.Id, currentUserId, StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Users));
        }

        // Protect system accounts
        if (IsSystemAccount(user))
        {
            TempData["ErrorMessage"] = "Cannot delete a system account.";
            return RedirectToAction(nameof(Users));
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Any(r => string.Equals(r, RoleNames.SuperAdmin, StringComparison.OrdinalIgnoreCase)))
        {
            TempData["ErrorMessage"] = "Cannot delete a SuperAdmin account.";
            return RedirectToAction(nameof(Users));
        }
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join("; ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Users));
        }

        TempData["SuccessMessage"] = $"Archived user '{user.Email ?? user.UserName ?? user.Id}'.";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return RedirectToAction(nameof(Users));
        }

        user.LockoutEnd = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join("; ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Users));
        }

        TempData["SuccessMessage"] = $"Restored user '{user.Email ?? user.UserName ?? user.Id}'.";
        return RedirectToAction(nameof(Users));
    }

    /// <summary>Disables 2FA for a user (e.g. when locked out after losing authenticator access).</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetTwoFactor(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return RedirectToAction(nameof(Users));
        }

        if (!user.TwoFactorEnabled)
        {
            TempData["InfoMessage"] = $"Two-factor authentication is already disabled for '{user.Email ?? user.UserName ?? user.Id}'.";
            return RedirectToAction(nameof(Users));
        }

        // Protect system accounts
        if (IsSystemAccount(user))
        {
            TempData["ErrorMessage"] = "Cannot manage a system account from this screen.";
            return RedirectToAction(nameof(Users));
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Any(IsHiddenAdministrativeRole))
        {
            TempData["ErrorMessage"] = "Cannot manage a hidden administrative account from this screen.";
            return RedirectToAction(nameof(Users));
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        TempData["SuccessMessage"] = $"Two-factor authentication has been reset for '{user.Email ?? user.UserName ?? user.Id}'. They can log in with password only and may re-enable 2FA from Profile.";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return RedirectToAction(nameof(Users));
        }

        var roleName = role.Name ?? string.Empty;
        if (string.Equals(roleName, RoleNames.SuperAdmin, StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Cannot delete SuperAdmin role.";
            return RedirectToAction(nameof(Users));
        }

        var assignedUsers = await _userManager.GetUsersInRoleAsync(roleName);
        if (assignedUsers.Count > 0)
        {
            TempData["ErrorMessage"] = $"Cannot delete role '{roleName}' because it is assigned to {assignedUsers.Count} user(s).";
            return RedirectToAction(nameof(Users));
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            TempData["ErrorMessage"] = string.Join("; ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Users));
        }

        TempData["SuccessMessage"] = $"Role '{roleName}' archived.";
        return RedirectToAction(nameof(Users));
    }

    private static bool IsHiddenAdministrativeRole(string roleName) =>
        string.Equals(roleName, RoleNames.SuperAdmin, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(roleName, RoleNames.Admin, StringComparison.OrdinalIgnoreCase);

    private static bool IsSystemAccount(ApplicationUser user) =>
        user.IsSystemAccount || 
        user.IsHidden ||
        user.Email?.EndsWith("@furnicomply.local", StringComparison.OrdinalIgnoreCase) == true;

    private async Task<(string FileName, string FullPath)> CreateBackupAsync()
    {
        if (_db.Database.IsSqlite())
        {
            var backupsDirectory = GetBackupsDirectory();
            Directory.CreateDirectory(backupsDirectory);

            var backupFileName = $"FurniComply-backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.db";
            var backupPath = Path.Combine(backupsDirectory, backupFileName);

            var sourceConnectionString = _db.Database.GetConnectionString();
            if (string.IsNullOrWhiteSpace(sourceConnectionString))
            {
                throw new InvalidOperationException("Backup failed: database connection string is not configured.");
            }

            var destinationConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = backupPath,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            await using var sourceConnection = new SqliteConnection(sourceConnectionString);
            await using var destinationConnection = new SqliteConnection(destinationConnectionString);
            await sourceConnection.OpenAsync();
            await destinationConnection.OpenAsync();
            sourceConnection.BackupDatabase(destinationConnection);

            await LogBackupCreationAsync(backupFileName);
            return (backupFileName, backupPath);
        }

        throw new InvalidOperationException("Backup download is only available for SQLite databases.");
    }

    private string GetBackupsDirectory() =>
        Path.Combine(_environment.ContentRootPath, "App_Data", "Backups");

    private string? ResolveDatabaseLocation()
    {
        if (_db.Database.IsSqlite())
        {
            var sqliteConnectionString = _db.Database.GetConnectionString() ?? string.Empty;
            var sqliteBuilder = new SqliteConnectionStringBuilder(sqliteConnectionString);
            return sqliteBuilder.DataSource;
        }

        return _db.Database.ProviderName;
    }

    private async Task LogBackupCreationAsync(string backupFileName)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            EntityName = "DatabaseBackup",
            EntityId = Guid.NewGuid(),
            Action = "Create",
            Actor = User.Identity?.Name ?? "system",
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            TimestampUtc = DateTime.UtcNow,
            Details = $"Created backup file '{backupFileName}'."
        });
        await _db.SaveChangesAsync();
    }

    private static string FormatFileSize(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB"];
        double size = bytes;

        for (var unitIndex = 0; unitIndex < units.Length; unitIndex++)
        {
            if (size < 1024 || unitIndex == units.Length - 1)
            {
                return unitIndex == 0 ? $"{size:0} {units[unitIndex]}" : $"{size:0.##} {units[unitIndex]}";
            }

            size /= 1024;
        }

        return $"{bytes} B";
    }
}
