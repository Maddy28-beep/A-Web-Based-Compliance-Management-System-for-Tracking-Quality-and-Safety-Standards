using FurniComply.Web.Services;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Web.ViewModels.Capa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize]
[Authorize(Policy = "Compliance.Read")]
public class CapaController : Controller
{
    private const string FilterOpen = "open";
    private const string FilterOverdue = "overdue";
    private const string FilterEvidenceSubmitted = "evidence";
    private const string FilterClosed = "closed";
    private const string UnassignedFilterValue = "__unassigned__";

    private readonly AppDbContext _db;
    private readonly SupplierPerformanceService _performance;
    private readonly IAssignmentNotificationService _assignmentNotify;

    public CapaController(AppDbContext db, SupplierPerformanceService performance, IAssignmentNotificationService assignmentNotify)
    {
        _db = db;
        _performance = performance;
        _assignmentNotify = assignmentNotify;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Start(Guid id, int? dueDays, string? startNotes)
    {
        var capa = await _db.CorrectiveActions.FindAsync(id);
        if (capa == null) return NotFound();

        if (capa.Status == CorrectiveActionStatus.Closed)
        {
            TempData["ErrorMessage"] = "CAPA is already closed.";
            return RedirectToCapaSource(capa);
        }

        capa.Status = CorrectiveActionStatus.InProgress;
        if (dueDays.HasValue && (dueDays.Value < 1 || dueDays.Value > 30))
        {
            TempData["ErrorMessage"] = "Due days must be between 1 and 30.";
            return RedirectToAction(nameof(Details), new { id });
        }
        if (dueDays.HasValue && dueDays.Value > 0 && dueDays.Value <= 30)
        {
            capa.DueAtUtc = DateTime.UtcNow.AddDays(dueDays.Value);
        }
        capa.UpdatedAtUtc = DateTime.UtcNow;
        
        var sourceDesc = capa.ComplianceCheckId.HasValue ? $"check {capa.ComplianceCheckId.Value}" : "quality evaluation";
        var auditDetail = $"Started CAPA for {sourceDesc}.";
        if (!string.IsNullOrWhiteSpace(startNotes))
        {
            auditDetail += $" Note: {startNotes.Trim()}";
        }
        AddAudit(nameof(CorrectiveAction), capa.Id, "Start", auditDetail);
        
        await _db.SaveChangesAsync();
        return RedirectToCapaSource(capa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> SubmitEvidence(Guid id, string? evidenceNotes)
    {
        var capa = await _db.CorrectiveActions.FindAsync(id);
        if (capa == null) return NotFound();

        if (capa.Status == CorrectiveActionStatus.Closed)
        {
            TempData["ErrorMessage"] = "CAPA is already closed.";
            return RedirectToCapaSource(capa);
        }

        capa.Status = CorrectiveActionStatus.EvidenceSubmitted;
        capa.EvidenceSubmittedAtUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(evidenceNotes))
        {
            capa.EvidenceNotes = evidenceNotes.Trim();
        }
        capa.UpdatedAtUtc = DateTime.UtcNow;

        var sourceDesc = capa.ComplianceCheckId.HasValue ? $"check {capa.ComplianceCheckId.Value}" : "quality evaluation";
        AddAudit(nameof(CorrectiveAction), capa.Id, "SubmitEvidence", $"Submitted CAPA evidence for {sourceDesc}.");
        
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Evidence submitted.";
        return RedirectToCapaSource(capa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Verify")]
    public async Task<IActionResult> Close(Guid id, string? closureNotes)
    {
        var capa = await _db.CorrectiveActions.FindAsync(id);
        if (capa == null) return NotFound();

        if (capa.Status != CorrectiveActionStatus.EvidenceSubmitted)
        {
            TempData["ErrorMessage"] = "Cannot close: evidence required.";
            return RedirectToCapaSource(capa);
        }

        if (string.IsNullOrWhiteSpace(capa.EvidenceNotes))
        {
            TempData["ErrorMessage"] = "Cannot close: evidence notes required.";
            return RedirectToCapaSource(capa);
        }

        if (!string.IsNullOrWhiteSpace(closureNotes))
        {
            var mergedNotes = string.IsNullOrWhiteSpace(capa.EvidenceNotes)
                ? closureNotes.Trim()
                : $"{capa.EvidenceNotes}{Environment.NewLine}{closureNotes.Trim()}";
            capa.EvidenceNotes = mergedNotes;
        }

        capa.Status = CorrectiveActionStatus.Closed;
        capa.ClosedAtUtc = DateTime.UtcNow;
        capa.ClosedBy = User.Identity?.Name ?? "system";
        capa.UpdatedAtUtc = DateTime.UtcNow;

        // 1. If linked to a check, update the check status
        if (capa.ComplianceCheckId.HasValue)
        {
            var check = await _db.ComplianceChecks.FindAsync(capa.ComplianceCheckId.Value);
            if (check != null)
            {
                check.ComplianceStatusId = new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"); // Compliant
                check.RiskLevelId = new Guid("cdcdcdcd-1111-1111-1111-111111111111"); // Low
                check.UpdatedAtUtc = DateTime.UtcNow;
                AddAudit(nameof(ComplianceCheck), check.Id, "Remediated", "Compliance check remediated and marked as Compliant after CAPA closure.");
            }
            await TryUpdateSupplierComplianceStatusAsync(capa.ComplianceCheckId.Value);
        }

        // 2. If linked to a supplier (Quality CAPA), trigger risk re-evaluation
        if (capa.SupplierId.HasValue)
        {
            await _performance.UpdateSupplierRiskStatusAsync(capa.SupplierId.Value);
        }

        var sourceDesc = capa.ComplianceCheckId.HasValue ? $"check {capa.ComplianceCheckId.Value}" : "quality evaluation";
        AddAudit(nameof(CorrectiveAction), capa.Id, "Close", $"Closed CAPA for {sourceDesc}.");
        
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "CAPA closed.";
        return RedirectToCapaSource(capa);
    }

    private IActionResult RedirectToCapaSource(CorrectiveAction capa)
    {
        if (capa.ComplianceCheckId.HasValue)
        {
            return RedirectToAction("Details", "ComplianceChecks", new { id = capa.ComplianceCheckId.Value });
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task TryUpdateSupplierComplianceStatusAsync(Guid checkId)
    {
        var check = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .FirstOrDefaultAsync(c => c.Id == checkId);

        if (check?.Notes == null) return;

        var supplier = await _db.Suppliers
            .Include(s => s.ComplianceDocuments)
            .FirstOrDefaultAsync(s => check.Notes.Contains(s.Name));

        if (supplier == null) return;

        var allDocsVerified = supplier.ComplianceDocuments.All(d => d.DocumentStatus == SupplierDocumentStatus.Verified);
        var allReportsApproved = await _db.RegulatoryReports
            .Where(r => r.SupplierId == supplier.Id)
            .Include(r => r.ReportStatus)
            .AllAsync(r => r.ReportStatus != null && r.ReportStatus.Name == "Accepted");

        var otherOpenCapas = await _db.CorrectiveActions
            .Where(ca => ca.Status != CorrectiveActionStatus.Closed && 
                         (ca.SupplierId == supplier.Id || 
                          (_db.ComplianceChecks.Any(cc => cc.Id == ca.ComplianceCheckId && cc.Notes.Contains(supplier.Name)))))
            .AnyAsync();

        if (allDocsVerified && allReportsApproved && !otherOpenCapas)
        {
            await _performance.UpdateSupplierRiskStatusAsync(supplier.Id);
            AddAudit(nameof(Supplier), supplier.Id, "Sync", "Supplier risk status re-evaluated after CAPA remediation.");
        }
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Write")]
    public async Task<IActionResult> Assign(Guid id, string? assignedTo, int? dueDays)
    {
        var capa = await _db.CorrectiveActions.FindAsync(id);
        if (capa == null) return NotFound();

        if (capa.Status == CorrectiveActionStatus.Closed)
        {
            TempData["ErrorMessage"] = "CAPA is already closed.";
            return RedirectToCapaSource(capa);
        }

        if (string.IsNullOrWhiteSpace(assignedTo))
        {
            TempData["ErrorMessage"] = "Assignee is required.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var assigneeEmail = assignedTo.Trim();
        var userExists = await _db.Users.AnyAsync(u => u.Email == assigneeEmail);
        if (!userExists)
        {
            TempData["ErrorMessage"] = "Assignee must be a registered user. Select from the dropdown.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (dueDays.HasValue && (dueDays.Value < 1 || dueDays.Value > 30))
        {
            TempData["ErrorMessage"] = "Due days must be between 1 and 30.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (!string.IsNullOrWhiteSpace(assignedTo))
        {
            capa.AssignedTo = assigneeEmail;
            capa.AssignedAtUtc = DateTime.UtcNow;
            capa.Status = CorrectiveActionStatus.Assigned;
        }
        if (dueDays.HasValue && dueDays.Value > 0 && dueDays.Value <= 30)
        {
            capa.DueAtUtc = DateTime.UtcNow.AddDays(dueDays.Value);
        }
        capa.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(CorrectiveAction), capa.Id, "Assign", $"Assigned CAPA to {capa.AssignedTo ?? "-"}.");
        await _db.SaveChangesAsync();

        var assigneeUser = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == assigneeEmail);
        var detailsUrl = Url.Action("Details", "Capa", new { id }, Request.Scheme, Request.Host.Value);
        var capaTitle = capa.Title ?? "Corrective Action";
        if (string.IsNullOrWhiteSpace(capa.Title) && capa.ComplianceCheckId.HasValue)
        {
            var check = await _db.ComplianceChecks.Include(c => c.Policy).FirstOrDefaultAsync(c => c.Id == capa.ComplianceCheckId.Value);
            if (check?.Policy != null) capaTitle = check.Policy.Title;
        }
        await _assignmentNotify.NotifyAssigneeAsync(
            assigneeEmail,
            assigneeUser?.FullName ?? assigneeEmail,
            capaTitle,
            capa.DueAtUtc.ToString("yyyy-MM-dd"),
            detailsUrl ?? "");

        TempData["SuccessMessage"] = "CAPA assigned.";
        return RedirectToCapaSource(capa);
    }

    [Authorize(Policy = "Compliance.Read")]
    public async Task<IActionResult> Details(Guid id)
    {
        var capa = await _db.CorrectiveActions
            .Include(c => c.Supplier)
            .Include(c => c.ComplianceCheck)
                .ThenInclude(ch => ch!.Policy)
            .Include(c => c.ComplianceCheck)
                .ThenInclude(ch => ch!.ComplianceStatus)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (capa == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(capa.AssignedTo))
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == capa.AssignedTo);
            ViewBag.AssignedToFullName = user?.FullName ?? capa.AssignedTo;
        }

        var now = DateTimeOffset.UtcNow;
        var users = await _db.Users
            .AsNoTracking()
            .Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= now)
            .OrderBy(u => u.FullName ?? u.Email ?? "")
            .Select(u => new { u.Email, Display = $"{(string.IsNullOrEmpty(u.FullName) ? u.Email : u.FullName)} ({u.Email})" })
            .ToListAsync();
        ViewBag.UserOptions = new SelectList(users, "Email", "Display");

        return View(capa);
    }

    public async Task<IActionResult> Index(string? status, string? assignee, DateTime? from, DateTime? to)
    {
        var selectedFilter = NormalizeFilter(status);
        var now = DateTime.UtcNow;
        var selectedAssignee = NormalizeAssignee(assignee);
        var fromDate = from?.Date;
        var toDate = to?.Date;

        var scopedBaseQuery = _db.CorrectiveActions.AsQueryable();
        if (User.IsInRole(RoleNames.DepartmentHead))
        {
            var user = User.Identity?.Name ?? string.Empty;
            scopedBaseQuery = scopedBaseQuery.Where(c => 
                (c.ComplianceCheck != null && c.ComplianceCheck.Policy != null && c.ComplianceCheck.Policy.Owner == user) ||
                (c.IssueType == CorrectiveActionType.Quality && (c.AssignedTo == user || c.AssignedTo == "Procurement Team")));
        }

        var nowOffset = DateTimeOffset.UtcNow;
        var registeredEmails = await _db.Users
            .AsNoTracking()
            .Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= nowOffset)
            .Select(u => u.Email!)
            .ToListAsync();
        var assigneeOptions = await scopedBaseQuery
            .Where(c => c.AssignedTo != null && c.AssignedTo.Trim() != string.Empty && registeredEmails.Contains(c.AssignedTo))
            .Select(c => c.AssignedTo!)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        var hasUnassigned = await scopedBaseQuery
            .AnyAsync(c => c.AssignedTo == null || c.AssignedTo.Trim() == string.Empty);

        var filteredBaseQuery = ApplyAssigneeAndDateFilters(scopedBaseQuery, selectedAssignee, fromDate, toDate);

        var query = filteredBaseQuery
            .Include(c => c.Supplier)
            .Include(c => c.ComplianceCheck)
                .ThenInclude(ch => ch!.Policy)
            .Include(c => c.ComplianceCheck)
                .ThenInclude(ch => ch!.ComplianceStatus)
            .AsQueryable();

        query = ApplyFilter(query, selectedFilter, now);

        query = selectedFilter == FilterClosed
            ? query.OrderByDescending(c => c.ClosedAtUtc ?? c.UpdatedAtUtc ?? c.CreatedAtUtc)
            : query.OrderBy(c => c.DueAtUtc).ThenByDescending(c => c.CreatedAtUtc);

        var actions = await query.ToListAsync();
        var allUsers = await _db.Users.AsNoTracking().ToListAsync();
        var rows = actions
            .Select(c => new CapaDashboardRowViewModel
            {
                Id = c.Id,
                ComplianceCheckId = c.ComplianceCheckId,
                SupplierId = c.SupplierId,
                IssueType = c.IssueType,
                Title = c.Title,
                PolicyTitle = c.ComplianceCheck?.Policy?.Title ?? (c.IssueType == CorrectiveActionType.Quality ? "Quality Performance" : "-"),
                SupplierName = c.Supplier?.Name ?? ExtractSupplierName(c.ComplianceCheck?.Notes),
                CheckStatus = c.ComplianceCheck?.ComplianceStatus?.Name ?? "-",
                CapaStatus = c.Status,
                AssignedTo = string.IsNullOrWhiteSpace(c.AssignedTo) ? "-" : c.AssignedTo,
                AssignedToFullName = string.IsNullOrWhiteSpace(c.AssignedTo) 
                    ? "-" 
                    : allUsers.FirstOrDefault(u => u.Email == c.AssignedTo)?.FullName ?? c.AssignedTo,
                DueAtUtc = c.DueAtUtc,
                EvidenceSubmittedAtUtc = c.EvidenceSubmittedAtUtc,
                ClosedAtUtc = c.ClosedAtUtc
            })
            .ToList();

        var model = new CapaDashboardViewModel
        {
            SelectedFilter = selectedFilter,
            SelectedAssignee = selectedAssignee,
            FromDate = fromDate,
            ToDate = toDate,
            IncludeUnassignedOption = hasUnassigned,
            AssigneeOptions = assigneeOptions,
            OpenCount = await ApplyFilter(filteredBaseQuery, FilterOpen, now).CountAsync(),
            OverdueCount = await ApplyFilter(filteredBaseQuery, FilterOverdue, now).CountAsync(),
            EvidenceSubmittedCount = await ApplyFilter(filteredBaseQuery, FilterEvidenceSubmitted, now).CountAsync(),
            ClosedCount = await ApplyFilter(filteredBaseQuery, FilterClosed, now).CountAsync(),
            Items = rows
        };

        return View(model);
    }

    private static string NormalizeFilter(string? filter)
    {
        var value = (filter ?? FilterOpen).Trim().ToLowerInvariant();
        return value switch
        {
            FilterOpen => FilterOpen,
            FilterOverdue => FilterOverdue,
            FilterEvidenceSubmitted => FilterEvidenceSubmitted,
            FilterClosed => FilterClosed,
            _ => FilterOpen
        };
    }

    private static IQueryable<CorrectiveAction> ApplyFilter(IQueryable<CorrectiveAction> query, string filter, DateTime nowUtc)
    {
        return filter switch
        {
            FilterOverdue => query.Where(c =>
                c.Status == CorrectiveActionStatus.Overdue ||
                (c.Status != CorrectiveActionStatus.Closed &&
                 c.Status != CorrectiveActionStatus.EvidenceSubmitted &&
                 c.DueAtUtc < nowUtc)),
            FilterEvidenceSubmitted => query.Where(c => c.Status == CorrectiveActionStatus.EvidenceSubmitted),
            FilterClosed => query.Where(c => c.Status == CorrectiveActionStatus.Closed),
            _ => query.Where(c =>
                c.Status == CorrectiveActionStatus.Open ||
                c.Status == CorrectiveActionStatus.Assigned ||
                c.Status == CorrectiveActionStatus.InProgress ||
                c.Status == CorrectiveActionStatus.EvidenceSubmitted)
        };
    }

    private static IQueryable<CorrectiveAction> ApplyAssigneeAndDateFilters(
        IQueryable<CorrectiveAction> query,
        string? selectedAssignee,
        DateTime? fromDate,
        DateTime? toDate)
    {
        if (!string.IsNullOrWhiteSpace(selectedAssignee))
        {
            if (selectedAssignee == UnassignedFilterValue)
            {
                query = query.Where(c => c.AssignedTo == null || c.AssignedTo.Trim() == string.Empty);
            }
            else
            {
                query = query.Where(c => c.AssignedTo == selectedAssignee);
            }
        }

        if (fromDate.HasValue)
        {
            query = query.Where(c => c.DueAtUtc >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            var toExclusive = toDate.Value.AddDays(1);
            query = query.Where(c => c.DueAtUtc < toExclusive);
        }

        return query;
    }

    private static string? NormalizeAssignee(string? assignee)
    {
        if (string.IsNullOrWhiteSpace(assignee))
        {
            return null;
        }

        var value = assignee.Trim();
        return string.Equals(value, UnassignedFilterValue, StringComparison.OrdinalIgnoreCase)
            ? UnassignedFilterValue
            : value;
    }

    private static string ExtractSupplierName(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            return "-";
        }

        const string marker = "Supplier:";
        var start = notes.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (start < 0)
        {
            return "-";
        }

        start += marker.Length;
        var end = notes.IndexOf('|', start);
        var candidate = end >= 0
            ? notes.Substring(start, end - start)
            : notes.Substring(start);

        var supplierName = candidate.Trim();
        return string.IsNullOrWhiteSpace(supplierName) ? "-" : supplierName;
    }
}
