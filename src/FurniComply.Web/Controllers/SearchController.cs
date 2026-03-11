using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class SearchController : Controller
{
    private readonly AppDbContext _db;

    public SearchController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? q, string? type, string? status, string? category)
    {
        ViewBag.Query = q?.Trim() ?? string.Empty;
        ViewBag.Type = type ?? "All";
        ViewBag.Status = status ?? "All";
        ViewBag.Category = category ?? "All";

        ViewBag.Types = new[] { "All", "Policy", "Supplier", "Report", "Compliance Check", "Order", "Audit Log" };
        ViewBag.Statuses = new[] { "All", "Active", "Draft", "Submitted", "Accepted", "Rejected", "Pending", "Compliant", "Non-Compliant", "OnHold", "Ordered", "Approved" };

        var policyCategories = await _db.PolicyCategories.Select(c => c.Name).ToListAsync();
        var supplierCategories = await _db.SupplierCategories.Select(c => c.Name).ToListAsync();
        var reportTypes = await _db.RegulatoryReports.Select(r => r.ReportType).Distinct().ToListAsync();
        var categories = new List<string> { "All" };
        categories.AddRange(policyCategories);
        categories.AddRange(supplierCategories);
        categories.AddRange(reportTypes);
        ViewBag.Categories = categories.Distinct().OrderBy(x => x).ToList();

        if (string.IsNullOrWhiteSpace(q))
        {
            return View(Enumerable.Empty<SearchResult>());
        }

        var query = q.Trim();
        var selectedType = type ?? "All";
        var selectedStatus = status ?? "All";
        var selectedCategory = category ?? "All";

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

        var suppliers = await _db.Suppliers
            .Include(s => s.SupplierCategory)
            .Where(s => (s.Name.Contains(query) || s.Certifications.Contains(query) || s.ContactEmail.Contains(query)) &&
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

        var logs = await _db.AuditLogs
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

        var results = policies
            .Concat(suppliers)
            .Concat(reports)
            .Concat(checks)
            .Concat(orders)
            .Concat(logs)
            .Where(r => selectedType == "All" || r.Type == selectedType)
            .OrderByDescending(r => r.Date)
            .ThenBy(r => r.Type);

        return View(results);
    }

    public record SearchResult(string Type, string Title, string Url, string Category, string Status, DateTime? Date);
}
