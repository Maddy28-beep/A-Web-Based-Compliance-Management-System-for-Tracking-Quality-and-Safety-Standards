using FurniComply.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize]
public class ComplianceChecksController : Controller
{
    private readonly AppDbContext _db;
    private readonly SupplierPerformanceService _performance;
    private readonly IAssignmentNotificationService _assignmentNotify;

    public ComplianceChecksController(AppDbContext db, SupplierPerformanceService performance, IAssignmentNotificationService assignmentNotify)
    {
        _db = db;
        _performance = performance;
        _assignmentNotify = assignmentNotify;
    }

    

    [Authorize(Policy = "Compliance.Read")]
    public async Task<IActionResult> Archived()
    {
        var checksQuery = _db.ComplianceChecks
            .IgnoreQueryFilters()
            .Where(c => c.IsDeleted)
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Include(c => c.ComplianceCategory)
            .Include(c => c.RiskLevel)
            .AsQueryable();

        if (IsDepartmentHead())
        {
            var owner = CurrentDepartmentOwner();
            checksQuery = checksQuery.Where(c => c.Policy != null && c.Policy.Owner == owner);
        }

        var checks = await checksQuery
            .OrderByDescending(c => c.DeletedAtUtc ?? c.CheckedAtUtc)
            .ToListAsync();

        return View(checks);
    }

    [Authorize(Policy = "Compliance.Read")]
    public async Task<IActionResult> Details(Guid id)
    {
        var check = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Include(c => c.ComplianceCategory)
            .Include(c => c.RiskLevel)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (check == null)
        {
            return NotFound();
        }
        if (!await CanAccessCheckAsync(check.Id))
        {
            return Forbid();
        }

        var correctiveActions = await _db.CorrectiveActions
            .Where(c => c.ComplianceCheckId == check.Id)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync();
        ViewBag.CorrectiveActions = correctiveActions;

        var allUsers = await _db.Users.AsNoTracking().ToListAsync();
        ViewBag.UserNames = allUsers.ToDictionary(u => u.Email!, u => u.FullName ?? u.Email!);
        var now = DateTimeOffset.UtcNow;
        var users = allUsers
            .Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= now)
            .OrderBy(u => u.FullName ?? u.Email ?? "")
            .Select(u => new { u.Email, Display = $"{(string.IsNullOrEmpty(u.FullName) ? u.Email : u.FullName)} ({u.Email})" })
            .ToList();
        ViewBag.UserOptions = new SelectList(users, "Email", "Display");

        var activeCapa = correctiveActions.FirstOrDefault(ca => ca.Status != CorrectiveActionStatus.Closed);
        if (activeCapa != null && !string.IsNullOrWhiteSpace(activeCapa.AssignedTo))
        {
            ViewBag.AssignedToFullName = ViewBag.UserNames.ContainsKey(activeCapa.AssignedTo) 
                ? ViewBag.UserNames[activeCapa.AssignedTo] 
                : activeCapa.AssignedTo;
        }

        var hasHighOrCriticalAlert = await _db.ComplianceAlerts.AnyAsync(a =>
            a.IsActive &&
            a.EntityType == nameof(ComplianceCheck) &&
            a.EntityId == check.Id &&
            (a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High));
        ViewBag.HasHighOrCriticalAlert = hasHighOrCriticalAlert;

        return View(check);
    }

    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Create()
    {
        await PopulateLookupsAsync();
        return View(new ComplianceCheck { CheckedAtUtc = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Create(ComplianceCheck check, Guid? supplierReferenceId, string? siteReference)
    {
        if (!await CanUsePolicyAsync(check.PolicyId))
        {
            return Forbid();
        }

        if (!supplierReferenceId.HasValue || supplierReferenceId.Value == Guid.Empty)
        {
            ModelState.AddModelError("supplierReferenceId", "The Supplier Reference field is required.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateLookupsAsync();
            return View(check);
        }

        var checkedBy = User.Identity?.Name ?? "system";
        string? supplierReference = null;
        if (supplierReferenceId.HasValue)
        {
            supplierReference = await _db.Suppliers
                .Where(s => s.Id == supplierReferenceId.Value)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();
        }

        var detailParts = new[]
        {
            string.IsNullOrWhiteSpace(supplierReference) ? null : $"Supplier: {supplierReference.Trim()}",
            string.IsNullOrWhiteSpace(siteReference) ? null : $"Site: {siteReference.Trim()}",
            $"Checked By: {checkedBy}"
        }.Where(x => !string.IsNullOrWhiteSpace(x));

        var detailsLine = string.Join(" | ", detailParts);
        check.Notes = string.IsNullOrWhiteSpace(check.Notes)
            ? detailsLine
            : $"{check.Notes.Trim()}{Environment.NewLine}{detailsLine}";

        _db.ComplianceChecks.Add(check);
        AddAudit(nameof(ComplianceCheck), check.Id, "Create", $"Created compliance check. {detailsLine}");
        await ApplyCorrectiveActionPolicyAsync(check.Id, check.ComplianceStatusId);
        await ApplyComplianceAlertPolicyAsync(check.Id, check.ComplianceStatusId);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var check = await _db.ComplianceChecks.FindAsync(id);
        if (check == null)
        {
            return NotFound();
        }
        if (!await CanAccessCheckAsync(check.Id))
        {
            return Forbid();
        }

        var allUsers = await _db.Users.AsNoTracking().ToListAsync();
        ViewBag.UserNames = allUsers.ToDictionary(u => u.Email!, u => u.FullName ?? u.Email!);

        var correctiveActions = await _db.CorrectiveActions
            .Where(c => c.ComplianceCheckId == id)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync();

        var activeCapa = correctiveActions.FirstOrDefault(ca => ca.Status != CorrectiveActionStatus.Closed);
        if (activeCapa != null && !string.IsNullOrWhiteSpace(activeCapa.AssignedTo))
        {
            ViewBag.AssignedToFullName = ViewBag.UserNames.ContainsKey(activeCapa.AssignedTo) 
                ? ViewBag.UserNames[activeCapa.AssignedTo] 
                : activeCapa.AssignedTo;
        }

        ViewBag.ActiveCapa = activeCapa;
        ViewBag.CorrectiveActions = correctiveActions;
        await PopulateLookupsAsync();
        return View(check);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Edit(Guid id, ComplianceCheck check)
    {
        if (id != check.Id)
        {
            return BadRequest();
        }
        if (!await CanAccessCheckAsync(id) || !await CanUsePolicyAsync(check.PolicyId))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            await PopulateLookupsAsync();
            return View(check);
        }

        _db.Entry(check).State = EntityState.Modified;
        AddAudit(nameof(ComplianceCheck), check.Id, "Edit", "Updated compliance check.");
        await ApplyCorrectiveActionPolicyAsync(check.Id, check.ComplianceStatusId);
        await ApplyComplianceAlertPolicyAsync(check.Id, check.ComplianceStatusId);
        
        // Trigger risk re-evaluation for linked supplier if name is found in notes
        var supplier = await _db.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => check.Notes != null && check.Notes.Contains(s.Name));
        if (supplier != null)
        {
            await _performance.UpdateSupplierRiskStatusAsync(supplier.Id);
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var check = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (check == null)
        {
            return NotFound();
        }
        if (!await CanAccessCheckAsync(check.Id))
        {
            return Forbid();
        }

        return View(check);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!await CanAccessCheckAsync(id))
        {
            return Forbid();
        }

        var check = await _db.ComplianceChecks.FindAsync(id);
        if (check != null)
        {
            check.IsDeleted = true;
            check.DeletedAtUtc = DateTime.UtcNow;
            check.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(ComplianceCheck), check.Id, "Archive", "Archived compliance check.");
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Compliance check archived.";
        }
        else
        {
            TempData["ErrorMessage"] = "Archive failed: compliance check not found.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Restore(Guid id)
    {
        if (!await CanAccessCheckAsync(id))
        {
            return Forbid();
        }

        var check = await _db.ComplianceChecks
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (check == null)
        {
            return NotFound();
        }

        check.IsDeleted = false;
        check.DeletedAtUtc = null;
        check.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(ComplianceCheck), check.Id, "Restore", "Restored compliance check.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Compliance check restored.";

        return RedirectToAction(nameof(Details), new { id = check.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> AssignCorrectiveAction(Guid checkId, string? assignedTo, int dueDays = 7)
    {
        if (!await CanAccessCheckAsync(checkId))
        {
            return Forbid();
        }

        var check = await _db.ComplianceChecks
            .Include(c => c.ComplianceStatus)
            .Include(c => c.Policy)
            .FirstOrDefaultAsync(c => c.Id == checkId);
        if (check == null)
        {
            return NotFound();
        }

        if (!string.Equals(check.ComplianceStatus?.Name, "Non-Compliant", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "CAPA can only be assigned for non-compliant checks.";
            return RedirectToAction(nameof(Details), new { id = checkId });
        }

        var assigneeEmail = !string.IsNullOrWhiteSpace(assignedTo) ? assignedTo.Trim() : User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(assigneeEmail))
        {
            TempData["ErrorMessage"] = "Assignee is required.";
            return RedirectToAction(nameof(Details), new { id = checkId });
        }

        var userExists = await _db.Users.AnyAsync(u => u.Email == assigneeEmail);
        if (!userExists)
        {
            TempData["ErrorMessage"] = "Assignee must be a registered user. Select from the dropdown.";
            return RedirectToAction(nameof(Details), new { id = checkId });
        }

        var now = DateTime.UtcNow;
        var dueAt = now.AddDays(Math.Clamp(dueDays, 1, 90));
        var capa = await _db.CorrectiveActions
            .Where(c => c.ComplianceCheckId == checkId && c.Status != CorrectiveActionStatus.Closed)
            .OrderByDescending(c => c.CreatedAtUtc)
            .FirstOrDefaultAsync();

        if (capa == null)
        {
            capa = new CorrectiveAction
            {
                Id = Guid.NewGuid(),
                ComplianceCheckId = checkId,
                Title = check.Policy?.Title ?? "CAPA Task",
                Description = "Corrective action required due to non-compliance.",
                Status = CorrectiveActionStatus.Open,
                DueAtUtc = dueAt
            };
            _db.CorrectiveActions.Add(capa);
        }

        capa.AssignedTo = assigneeEmail;
        capa.AssignedAtUtc = now;
        capa.DueAtUtc = dueAt;
        capa.Status = CorrectiveActionStatus.Assigned;
        capa.UpdatedAtUtc = now;

        AddAudit(nameof(CorrectiveAction), capa.Id, "Assign", $"Assigned CAPA for check {checkId} to {capa.AssignedTo}.");
        await _db.SaveChangesAsync();

        var assigneeUser = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == assigneeEmail);
        var detailsUrl = Url.Action("Details", "Capa", new { id = capa.Id }, Request.Scheme, Request.Host.Value);
        var capaTitle = check.Policy?.Title ?? "Corrective Action";
        await _assignmentNotify.NotifyAssigneeAsync(
            assigneeEmail,
            assigneeUser?.FullName ?? assigneeEmail,
            capaTitle,
            dueAt.ToString("yyyy-MM-dd"),
            detailsUrl ?? "");

        TempData["SuccessMessage"] = "CAPA assigned.";
        return RedirectToAction(nameof(Details), new { id = checkId });
    }

    public async Task<IActionResult> Index(Guid? policyId, Guid? statusId, Guid? categoryId, Guid? riskId, string? site)
    {
        var checksQuery = _db.ComplianceChecks
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Include(c => c.ComplianceCategory)
            .Include(c => c.RiskLevel)
            .AsQueryable();

        if (policyId.HasValue)
        {
            checksQuery = checksQuery.Where(c => c.PolicyId == policyId.Value);
        }
        if (statusId.HasValue)
        {
            checksQuery = checksQuery.Where(c => c.ComplianceStatusId == statusId.Value);
        }
        if (categoryId.HasValue)
        {
            checksQuery = checksQuery.Where(c => c.ComplianceCategoryId == categoryId.Value);
        }
        if (riskId.HasValue)
        {
            checksQuery = checksQuery.Where(c => c.RiskLevelId == riskId.Value);
        }
        if (!string.IsNullOrWhiteSpace(site))
        {
            checksQuery = checksQuery.Where(c => c.Notes.Contains(site));
        }

        if (IsDepartmentHead())
        {
            var owner = CurrentDepartmentOwner();
            checksQuery = checksQuery.Where(c => c.Policy != null && c.Policy.Owner == owner);
        }

        var checks = await checksQuery
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync();

        await PopulateLookupsAsync();
        return View(checks);
    }

    private async Task PopulateLookupsAsync()
    {
        var policiesQuery = _db.Policies.AsQueryable();
        if (IsDepartmentHead())
        {
            var owner = CurrentDepartmentOwner();
            policiesQuery = policiesQuery.Where(p => p.Owner == owner);
        }

        var policies = await policiesQuery
            .OrderBy(p => p.Title)
            .Select(p => new { p.Id, Display = $"{p.Code} - {p.Title}" })
            .ToListAsync();

        ViewBag.PolicyId = new SelectList(policies, "Id", "Display");

        var statuses = await _db.ComplianceStatuses
            .OrderBy(s => s.SortOrder)
            .Select(s => new { s.Id, s.Name })
            .ToListAsync();
        ViewBag.ComplianceStatusId = new SelectList(statuses, "Id", "Name");

        var categories = await _db.ComplianceCategories
            .OrderBy(c => c.SortOrder)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();
        ViewBag.ComplianceCategoryId = new SelectList(categories, "Id", "Name");

        var risks = await _db.RiskLevels
            .OrderBy(r => r.SortOrder)
            .Select(r => new { r.Id, r.Name })
            .ToListAsync();
        ViewBag.RiskLevelId = new SelectList(risks, "Id", "Name");

        var supplierOptions = await _db.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name })
            .ToListAsync();
        ViewBag.SupplierReferenceId = new SelectList(supplierOptions, "Id", "Name");

        var now = DateTimeOffset.UtcNow;
        var users = await _db.Users
            .AsNoTracking()
            .Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= now)
            .OrderBy(u => u.FullName ?? u.Email ?? "")
            .Select(u => new { u.Email, Display = $"{(string.IsNullOrEmpty(u.FullName) ? u.Email : u.FullName)} ({u.Email})" })
            .ToListAsync();
        ViewBag.UserOptions = new SelectList(users, "Email", "Display");
    }

    private void AddAudit(string entityName, Guid entityId, string action, string details)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            Actor = User.Identity?.Name ?? "system",
            TimestampUtc = DateTime.UtcNow,
            Details = details
        });
    }

    private bool IsDepartmentHead() =>
        User.IsInRole(RoleNames.DepartmentHead);

    private string CurrentDepartmentOwner() =>
        User.Identity?.Name ?? string.Empty;

    private async Task<bool> CanUsePolicyAsync(Guid policyId)
    {
        if (!IsDepartmentHead())
        {
            return true;
        }

        var owner = CurrentDepartmentOwner();
        return await _db.Policies.AnyAsync(p => p.Id == policyId && p.Owner == owner);
    }

    private async Task<bool> CanAccessCheckAsync(Guid checkId)
    {
        if (!IsDepartmentHead())
        {
            return true;
        }

        var owner = CurrentDepartmentOwner();
        return await _db.ComplianceChecks.AnyAsync(c => c.Id == checkId && c.Policy != null && c.Policy.Owner == owner);
    }

    private async Task ApplyComplianceAlertPolicyAsync(Guid checkId, Guid complianceStatusId)
    {
        var statusName = await _db.ComplianceStatuses
            .Where(s => s.Id == complianceStatusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(statusName))
        {
            return;
        }

        var activeAlerts = await _db.ComplianceAlerts
            .Where(a => a.IsActive && a.EntityType == nameof(ComplianceCheck) && a.EntityId == checkId)
            .ToListAsync();

        var isNonCompliant = string.Equals(statusName, "Non-Compliant", StringComparison.OrdinalIgnoreCase);
        if (isNonCompliant)
        {
            if (activeAlerts.Count > 0)
            {
                return;
            }

            var ruleId = await _db.ComplianceAlertRules
                .Where(r => r.RuleKey == ComplianceAlertRuleKeys.NonCompliantChecks)
                .Select(r => (Guid?)r.Id)
                .FirstOrDefaultAsync();
            if (!ruleId.HasValue)
            {
                return;
            }

            _db.ComplianceAlerts.Add(new ComplianceAlert
            {
                Id = Guid.NewGuid(),
                ComplianceAlertRuleId = ruleId.Value,
                Severity = ComplianceAlertSeverity.High,
                Title = "Non-compliant check requires CAPA",
                Message = $"Compliance check {checkId} is non-compliant and requires corrective action.",
                EntityType = nameof(ComplianceCheck),
                EntityId = checkId,
                IsActive = true,
                IsAcknowledged = false,
                TriggeredAtUtc = DateTime.UtcNow
            });
            return;
        }

        foreach (var alert in activeAlerts)
        {
            alert.IsActive = false;
            alert.ResolvedAtUtc = DateTime.UtcNow;
            alert.UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    private async Task ApplyCorrectiveActionPolicyAsync(Guid checkId, Guid complianceStatusId)
    {
        var statusName = await _db.ComplianceStatuses
            .Where(s => s.Id == complianceStatusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(statusName))
        {
            return;
        }

        var now = DateTime.UtcNow;
        var openActions = await _db.CorrectiveActions
            .Where(c => c.ComplianceCheckId == checkId && c.Status != CorrectiveActionStatus.Closed)
            .ToListAsync();

        foreach (var action in openActions.Where(a =>
                     a.DueAtUtc < now &&
                     a.Status != CorrectiveActionStatus.EvidenceSubmitted &&
                     a.Status != CorrectiveActionStatus.Overdue))
        {
            action.Status = CorrectiveActionStatus.Overdue;
            action.UpdatedAtUtc = now;
        }

        var isNonCompliant = string.Equals(statusName, "Non-Compliant", StringComparison.OrdinalIgnoreCase);
        if (!isNonCompliant || openActions.Count > 0)
        {
            return;
        }

        var policyTitle = await _db.ComplianceChecks
            .Where(c => c.Id == checkId)
            .Select(c => c.Policy != null ? c.Policy.Title : null)
            .FirstOrDefaultAsync();

        _db.CorrectiveActions.Add(new CorrectiveAction
        {
            Id = Guid.NewGuid(),
            ComplianceCheckId = checkId,
            Title = policyTitle ?? "CAPA Task",
            Description = "Auto-created corrective action due to non-compliant check.",
            Status = CorrectiveActionStatus.Open,
            DueAtUtc = now.AddDays(7)
        });
    }
}
