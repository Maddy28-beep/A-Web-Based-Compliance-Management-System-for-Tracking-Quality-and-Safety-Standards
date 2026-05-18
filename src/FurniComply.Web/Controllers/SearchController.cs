using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly AppDbContext _db;
    private readonly AuthorizationPolicyHelper _policyHelper;

    public SearchController(AppDbContext db, AuthorizationPolicyHelper policyHelper)
    {
        _db = db;
        _policyHelper = policyHelper;
    }

    public async Task<IActionResult> Index(string? q, string? type, string? status, string? category)
    {
        var canPolicies = await _policyHelper.HasPolicyAsync(User, "Policies.Read");
        var canProcurement = await _policyHelper.HasPolicyAsync(User, "Procurement.Read");
        var canReports = await _policyHelper.HasPolicyAsync(User, "Reports.Read");
        var canCompliance = await _policyHelper.HasPolicyAsync(User, "Compliance.Read");
        var canAudit = await _policyHelper.HasPolicyAsync(User, "Audit.Read");

        var allowedTypes = new List<string> { "All" };
        if (canPolicies) allowedTypes.Add("Policy");
        if (canProcurement)
        {
            allowedTypes.Add("Supplier");
            allowedTypes.Add("Order");
        }
        if (canReports) allowedTypes.Add("Report");
        if (canCompliance) allowedTypes.Add("Compliance Check");
        if (canAudit) allowedTypes.Add("Audit Log");

        ViewBag.Query = q?.Trim() ?? string.Empty;
        ViewBag.Type = type ?? "All";
        ViewBag.Status = status ?? "All";
        ViewBag.Category = category ?? "All";
        ViewBag.Types = allowedTypes.ToArray();
        ViewBag.Statuses = new[] { "All", "Active", "Draft", "Submitted", "Accepted", "Rejected", "Pending", "Compliant", "Non-Compliant", "OnHold", "Ordered", "Approved" };

        var categories = new List<string> { "All" };
        if (canPolicies)
        {
            categories.AddRange(await _db.PolicyCategories.Select(c => c.Name).ToListAsync());
        }
        if (canProcurement)
        {
            categories.AddRange(await _db.SupplierCategories.Select(c => c.Name).ToListAsync());
        }
        if (canReports)
        {
            categories.AddRange(await _db.RegulatoryReports.Select(r => r.ReportType).Distinct().ToListAsync());
        }
        ViewBag.Categories = categories.Distinct().OrderBy(x => x).ToList();

        if (string.IsNullOrWhiteSpace(q))
        {
            return View(Enumerable.Empty<SearchResult>());
        }

        var query = q.Trim();
        var selectedType = type ?? "All";
        var selectedStatus = status ?? "All";
        var selectedCategory = category ?? "All";

        if (selectedType != "All" && !allowedTypes.Contains(selectedType, StringComparer.OrdinalIgnoreCase))
        {
            return Forbid();
        }

        var results = new List<SearchResult>();

        if (canPolicies && (selectedType == "All" || selectedType == "Policy"))
        {
            var policies = await _db.Policies
                .Include(p => p.PolicyCategory)
                .Where(p => (p.Title.Contains(query) || p.Code.Contains(query) || p.Owner.Contains(query)) &&
                            (selectedStatus == "All" || p.Status.ToString() == selectedStatus) &&
                            (selectedCategory == "All" || p.PolicyCategory!.Name == selectedCategory))
                .Select(p => new SearchResult(
                    "Policy",
                    p.Title,
                    Url.Action("Details", "Policies", new { id = p.Id })!,
                    p.PolicyCategory != null ? p.PolicyCategory.Name : "-",
                    p.Status.ToString(),
                    p.EffectiveDateUtc))
                .ToListAsync();
            results.AddRange(policies);
        }

        if (canProcurement && (selectedType == "All" || selectedType == "Supplier"))
        {
            var suppliers = await _db.Suppliers
                .Include(s => s.SupplierCategory)
                .Where(s => (s.Name.Contains(query) || (s.Certifications != null && s.Certifications.Contains(query)) || s.ContactEmail.Contains(query)) &&
                            (selectedStatus == "All" || s.Status.ToString() == selectedStatus) &&
                            (selectedCategory == "All" || s.SupplierCategory!.Name == selectedCategory))
                .Select(s => new SearchResult(
                    "Supplier",
                    $"{s.Name} ({s.ContactEmail})",
                    Url.Action("Details", "Suppliers", new { id = s.Id })!,
                    s.SupplierCategory != null ? s.SupplierCategory.Name : "-",
                    s.Status.ToString(),
                    s.CreatedAtUtc))
                .ToListAsync();
            results.AddRange(suppliers);
        }

        if (canReports && (selectedType == "All" || selectedType == "Report"))
        {
            var reports = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Where(r => (r.ReportType.Contains(query) || r.Summary.Contains(query) || r.ReportStatus!.Name.Contains(query)) &&
                            (selectedStatus == "All" || r.ReportStatus!.Name == selectedStatus) &&
                            (selectedCategory == "All" || r.ReportType == selectedCategory))
                .Select(r => new SearchResult(
                    "Report",
                    $"{r.ReportType} [{r.ReportStatus!.Name}]",
                    Url.Action("Details", "RegulatoryReports", new { id = r.Id })!,
                    r.ReportType,
                    r.ReportStatus!.Name,
                    r.SubmittedAtUtc ?? r.CreatedAtUtc))
                .ToListAsync();
            results.AddRange(reports);
        }

        if (canCompliance && (selectedType == "All" || selectedType == "Compliance Check"))
        {
            var checks = await _db.ComplianceChecks
                .Include(c => c.ComplianceStatus)
                .Include(c => c.ComplianceCategory)
                .Where(c => c.Notes.Contains(query) &&
                            (selectedStatus == "All" || c.ComplianceStatus!.Name == selectedStatus) &&
                            (selectedCategory == "All" || c.ComplianceCategory!.Name == selectedCategory))
                .Select(c => new SearchResult(
                    "Compliance Check",
                    c.Notes,
                    Url.Action("Details", "ComplianceChecks", new { id = c.Id })!,
                    c.ComplianceCategory != null ? c.ComplianceCategory.Name : "-",
                    c.ComplianceStatus != null ? c.ComplianceStatus.Name : "-",
                    c.CheckedAtUtc))
                .ToListAsync();
            results.AddRange(checks);
        }

        if (canProcurement && (selectedType == "All" || selectedType == "Order"))
        {
            var orders = await _db.ProcurementOrders
                .Include(o => o.ProcurementStatus)
                .Where(o => o.OrderNumber.Contains(query) &&
                            (selectedStatus == "All" || o.ProcurementStatus!.Name == selectedStatus))
                .Select(o => new SearchResult(
                    "Order",
                    o.OrderNumber,
                    Url.Action("Details", "ProcurementOrders", new { id = o.Id })!,
                    "Procurement",
                    o.ProcurementStatus != null ? o.ProcurementStatus.Name : "-",
                    o.OrderDateUtc))
                .ToListAsync();
            results.AddRange(orders);
        }

        if (canAudit && (selectedType == "All" || selectedType == "Audit Log"))
        {
            var logsQuery = _db.AuditLogs.AsQueryable();
            if (!HasFullAuditVisibility())
            {
                var actor = User.Identity?.Name ?? string.Empty;
                logsQuery = string.IsNullOrWhiteSpace(actor)
                    ? logsQuery.Where(_ => false)
                    : logsQuery.Where(a => a.Actor == actor);
            }

            var logs = await logsQuery
                .Where(a => (a.Action.Contains(query) || a.Actor.Contains(query) || a.EntityName.Contains(query)) &&
                            (selectedStatus == "All" || a.Action.Contains(selectedStatus)) &&
                            (selectedCategory == "All" || a.EntityName == selectedCategory))
                .Select(a => new SearchResult(
                    "Audit Log",
                    $"{a.Action} by {a.Actor}",
                    Url.Action("Details", "AuditLogs", new { id = a.Id })!,
                    a.EntityName,
                    a.Action,
                    a.TimestampUtc))
                .ToListAsync();
            results.AddRange(logs);
        }

        var ordered = results
            .OrderByDescending(r => r.Date)
            .ThenBy(r => r.Type);

        return View(ordered);
    }

    private bool HasFullAuditVisibility() =>
        User.IsInRole(RoleNames.SuperAdmin) ||
        User.IsInRole(RoleNames.Admin) ||
        User.IsInRole(RoleNames.ComplianceManager) ||
        User.IsInRole(RoleNames.Auditor);

    public record SearchResult(string Type, string Title, string Url, string Category, string Status, DateTime? Date);
}
