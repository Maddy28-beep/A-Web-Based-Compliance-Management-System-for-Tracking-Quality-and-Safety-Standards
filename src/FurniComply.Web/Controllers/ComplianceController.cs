using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Compliance.Read")]
public class ComplianceController : Controller
{
    private readonly AppDbContext _db;

    public ComplianceController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        // One-time Sync: Ensure any ComplianceCheck with a CLOSED CAPA is marked as Compliant
        var legacyChecks = await _db.ComplianceChecks
            .Include(c => c.ComplianceStatus)
            .Where(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant")
            .Where(c => _db.CorrectiveActions.Any(ca => ca.ComplianceCheckId == c.Id && ca.Status == CorrectiveActionStatus.Closed))
            .ToListAsync();

        if (legacyChecks.Any())
        {
            var compliantId = await _db.ComplianceStatuses
                .Where(s => s.Name == "Compliant")
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
            
            var lowRiskId = await _db.RiskLevels
                .Where(r => r.Name == "Low")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (compliantId != Guid.Empty)
            {
                foreach (var check in legacyChecks)
                {
                    check.ComplianceStatusId = compliantId;
                    if (lowRiskId != Guid.Empty) check.RiskLevelId = lowRiskId;
                    check.UpdatedAtUtc = DateTime.UtcNow;
                }
                await _db.SaveChangesAsync();
            }
        }

        var recentChecksQuery = _db.ComplianceChecks
            .Include(c => c.ComplianceStatus)
            .Include(c => c.RiskLevel)
            .Include(c => c.Policy)
            .AsQueryable();

        var openIssuesQuery = _db.ComplianceChecks
            .Include(c => c.ComplianceStatus)
            .Include(c => c.RiskLevel)
            .Include(c => c.Policy)
            .Where(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant")
            .AsQueryable();

        if (User.IsInRole(RoleNames.DepartmentHead))
        {
            var owner = User.Identity?.Name ?? string.Empty;
            recentChecksQuery = recentChecksQuery.Where(c => c.Policy != null && c.Policy.Owner == owner);
            openIssuesQuery = openIssuesQuery.Where(c => c.Policy != null && c.Policy.Owner == owner);
        }

        var recentChecks = await recentChecksQuery
            .OrderByDescending(c => c.CheckedAtUtc)
            .Take(8)
            .ToListAsync();

        var openIssues = await openIssuesQuery
            .OrderByDescending(c => c.CheckedAtUtc)
            .Take(5)
            .ToListAsync();

        var drafts = User.IsInRole(RoleNames.DepartmentHead)
            ? new List<FurniComply.Domain.Entities.RegulatoryReport>()
            : await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Where(r => r.ReportStatus != null && r.ReportStatus.Name == "Draft")
                .OrderByDescending(r => r.PeriodEndUtc)
                .Take(5)
                .ToListAsync();

        ViewBag.RecentChecks = recentChecks;
        ViewBag.OpenIssues = openIssues;
        ViewBag.DraftReports = drafts;

        return View();
    }

    public async Task<IActionResult> Queue()
    {
        var approvedStatusId = await _db.ProcurementStatuses
            .Where(s => s.Name == "Approved")
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync();

        if (approvedStatusId == null)
        {
            return View(new List<FurniComply.Domain.Entities.ProcurementOrder>());
        }

        var queue = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .Include(o => o.Items)
            .Where(o => o.ProcurementStatusId == approvedStatusId.Value)
            .OrderByDescending(o => o.UpdatedAtUtc)
            .ToListAsync();

        return View(queue);
    }
}
