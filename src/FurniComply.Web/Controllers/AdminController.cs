using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "System.Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
            var userRoles = await _userManager.GetRolesAsync(user);
            activeSummaries.Add(new UserSummary(user.Id, user.Email ?? user.UserName ?? "unknown", userRoles.ToList()));
        }

        var archivedSummaries = new List<UserSummary>();
        foreach (var user in archivedUsers)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            archivedSummaries.Add(new UserSummary(user.Id, user.Email ?? user.UserName ?? "unknown", userRoles.ToList()));
        }

        var roles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .ToListAsync();

        var roleSummaries = new List<RoleSummary>();
        foreach (var role in roles)
        {
            var roleName = role.Name ?? string.Empty;
            var userCount = (await _userManager.GetUsersInRoleAsync(roleName)).Count;
            var isProtected = string.Equals(roleName, RoleNames.SuperAdmin, StringComparison.OrdinalIgnoreCase);
            roleSummaries.Add(new RoleSummary(role.Id, roleName, userCount, isProtected));
        }
        ViewBag.RoleSummaries = roleSummaries;
        ViewBag.ArchivedUsers = archivedSummaries;

        return View(activeSummaries);
    }

    public async Task<IActionResult> CreateUser()
    {
        var roles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync();

        var vm = new CreateUserViewModel
        {
            Roles = roles.Select(r => new RoleSelection { Name = r, Selected = false }).ToList()
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
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync();
            model.Roles ??= roles.Select(r => new RoleSelection { Name = r, Selected = false }).ToList();
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email.Trim());
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
            model.Roles ??= (await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name!).ToListAsync())
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
                .Select(r => new RoleSelection { Name = r, Selected = false }).ToList();
            return View(model);
        }

        var selectedRoles = model.SelectedRoles ?? model.Roles?.Where(r => r.Selected).Select(r => r.Name).ToList() ?? new List<string>();
        if (selectedRoles.Count > 0)
        {
            await _userManager.AddToRolesAsync(user, selectedRoles);
        }

        TempData["SuccessMessage"] = model.AssignmentOnly
            ? $"Contact '{user.Email}' added. They will appear in the Assign To dropdown and receive email when assigned. (No login—assignment/notification only.)"
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

        var roles = _roleManager.Roles.Select(r => r.Name!).ToList();
        var userRoles = await _userManager.GetRolesAsync(user);

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

        var selectedRoles = model.Roles.Where(r => r.Selected).Select(r => r.Name).ToList();
        var currentRoles = await _userManager.GetRolesAsync(user);

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
}
