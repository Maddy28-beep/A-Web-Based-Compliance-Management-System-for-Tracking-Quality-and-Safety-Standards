using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FurniComply.Application.Interfaces;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FurniComply.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IAnalyticsService _analytics;
    private readonly ISupplierComplianceScoreService _complianceScore;
    private readonly FurniComply.Infrastructure.Persistence.AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IAnalyticsService analytics,
        ISupplierComplianceScoreService complianceScore,
        FurniComply.Infrastructure.Persistence.AppDbContext db,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<DashboardController> logger)
    {
        _analytics = analytics;
        _complianceScore = complianceScore;
        _db = db;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IActionResult> Index([FromQuery] string? from, [FromQuery] string? to)
    {
        DateTime? fromDate = DateTime.TryParse(from, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var f) ? f : null;
        DateTime? toDate = DateTime.TryParse(to, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var t) ? t : null;
        ViewBag.DateFilterFrom = from;
        ViewBag.DateFilterTo = to;

        if (User.IsInRole(RoleNames.SuperAdmin))
        {
            return View("RoleDashboard", await BuildRoleDashboardSafelyAsync(
                "Super Admin",
                () => BuildSuperAdminDashboardAsync(fromDate, toDate)));
        }

        if (User.IsInRole(RoleNames.Admin))
        {
            return View("RoleDashboard", await BuildRoleDashboardSafelyAsync(
                "Admin",
                () => BuildAdminDashboardAsync(fromDate, toDate)));
        }

        if (User.IsInRole(RoleNames.ComplianceManager))
        {
            return View("RoleDashboard", await BuildRoleDashboardSafelyAsync(
                "Compliance Officer",
                () => BuildComplianceDashboardAsync(fromDate, toDate)));
        }

        if (User.IsInRole(RoleNames.DepartmentHead))
        {
            return View("RoleDashboard", await BuildRoleDashboardSafelyAsync(
                "Department Head",
                () => BuildDepartmentHeadDashboardAsync(fromDate, toDate)));
        }

        if (User.IsInRole(RoleNames.Procurement))
        {
            return View("RoleDashboard", await BuildRoleDashboardSafelyAsync(
                "Procurement Officer",
                () => BuildProcurementDashboardAsync(fromDate, toDate)));
        }

        var toDateExcl = toDate.HasValue ? toDate.Value.Date.AddDays(1) : (DateTime?)null;
        var snapshot = await _analytics.GetSnapshotAsync(fromDate, toDateExcl ?? toDate);
        var weather = await GetWeatherAsync();
        var rates = await GetExchangeRatesAsync("PHP");
        var holidays = await GetHolidaysAsync();

        var viewModel = new DashboardViewModel(snapshot, weather, rates, holidays);
        return View(viewModel);
    }

    private async Task<RoleDashboardViewModel> BuildRoleDashboardSafelyAsync(
        string roleName,
        Func<Task<RoleDashboardViewModel>> builder)
    {
        try
        {
            return await builder();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build role dashboard for {RoleName}", roleName);
            var fallbackMetrics = GetFallbackMetricsForRole(roleName);
            return new RoleDashboardViewModel(
                roleName,
                GetFallbackFocusForRole(roleName),
                fallbackMetrics,
                Array.Empty<RoleDashboardItem>(),
                new[]
                {
                    new RoleDashboardItem(
                        "Dashboard Data Error",
                        "Some dashboard data could not be loaded. Please check database schema/migrations and server logs.",
                        "warn")
                });
        }
    }

    private List<RoleDashboardMetric> GetFallbackMetricsForRole(string roleName)
    {
        return new List<RoleDashboardMetric>();
    }

    private static string GetFallbackFocusForRole(string roleName) =>
        roleName switch
        {
            "Super Admin" => "System governance and platform control only (no operations).",
            "Admin" => "Business oversight and final internal approvals.",
            "Compliance Officer" => "Operational compliance control and daily action queue.",
            "Department Head" => "Department execution view. Data is scoped to your assigned policies.",
            "Procurement Officer" => "Supplier lifecycle control and document completeness.",
            _ => "Operational dashboard overview."
        };

    private async Task<RoleDashboardViewModel> BuildSuperAdminDashboardAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var rangeStart = from.HasValue ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc) : now.AddHours(-24);
        var rangeEndExcl = to.HasValue ? DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc) : now.AddSeconds(1);

        var totalUsers = await _db.Users.CountAsync();
        var totalRoles = await _db.Roles.CountAsync();
        var failedLoginAttempts = await _db.Users.CountAsync(u => u.AccessFailedCount > 0);
        var activeWorkflowRules = await _db.ComplianceAlertRules.CountAsync(r => r.IsEnabled);
        var recentAuditCount = await _db.AuditLogs.CountAsync(a => a.TimestampUtc >= rangeStart && a.TimestampUtc < rangeEndExcl);
        var errorLogCount = await _db.AuditLogs.CountAsync(a => a.Action.Contains("Error") && a.TimestampUtc >= rangeStart && a.TimestampUtc < rangeEndExcl);
        var pendingOverrideRequests = await _db.Suppliers.CountAsync(s => s.OverrideRequestActive);

        var recentAudits = await _db.AuditLogs
            .Where(a => a.TimestampUtc >= rangeStart && a.TimestampUtc < rangeEndExcl)
            .OrderByDescending(a => a.TimestampUtc)
            .Take(5)
            .ToListAsync();

        var recentAuditItems = recentAudits
            .Select(a => new RoleDashboardItem(
                $"{a.EntityName} - {a.Action}",
                $"{a.Actor} at {a.TimestampUtc:yyyy-MM-dd HH:mm} UTC",
                "neutral",
                Url.Action("Details", "AuditLogs", new { id = a.Id })))
            .ToList();

        var overrideRequests = await _db.Suppliers
            .Where(s => s.OverrideRequestActive)
            .OrderByDescending(s => s.RequestedAtUtc)
            .Take(5)
            .ToListAsync();

        var overrideRequestItems = overrideRequests
            .Select(s => new RoleDashboardItem(
                $"Override: {s.Name}",
                $"Requested by {s.RequestedBy}: {s.OverrideRequestReason}",
                "risk",
                Url.Action("Details", "Suppliers", new { id = s.Id })))
            .ToList();

        var commonKpis = await GetCommonKpiMetricsAsync(from, to);
        var metrics = commonKpis.Concat(new List<RoleDashboardMetric>
        {
            new("Total Users", totalUsers.ToString(), "Identity inventory", Url.Action("Users", "Admin")),
            new("Active Roles", totalRoles.ToString(), "Role catalog health", Url.Action("Users", "Admin")),
            new("Failed Login Attempts", failedLoginAttempts.ToString(), "Current account lockout signals", Url.Action("Users", "Admin")),
            new("Override Requests", pendingOverrideRequests.ToString(), "Suppliers waiting for document exception", Url.Action("Index", "Suppliers"))
        }).ToList();

        return new RoleDashboardViewModel(
            "Super Admin",
            "System governance and platform control only (no operations).",
            metrics,
            recentAuditItems.Concat(overrideRequestItems).ToList(),
            new List<RoleDashboardItem>
            {
                new("System Error Logs", $"{errorLogCount} recent entries tagged as error", "warn", Url.Action("Index", "AuditLogs")),
                new("Workflow Rules Active", $"{activeWorkflowRules} active alert/workflow rules", "neutral", Url.Action("Index", "ComplianceAlertRules")),
                new("Pending Doc Overrides", $"{pendingOverrideRequests} suppliers requesting document exception", pendingOverrideRequests > 0 ? "risk" : "good", Url.Action("Index", "Suppliers"))
            });
    }

    private async Task<RoleDashboardViewModel> BuildAdminDashboardAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var rangeStart = from.HasValue ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc) : now.AddMonths(-1);
        var rangeEndExcl = to.HasValue ? DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc) : now.AddDays(1);

        var totalChecks = await _db.ComplianceChecks.CountAsync(c => c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl);
        var compliantChecks = await _db.ComplianceChecks.CountAsync(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl);
        var openFindings = await _db.ComplianceChecks.CountAsync(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl);
        var pendingPolicies = await _db.Policies.CountAsync(p => !p.IsDeleted && p.Status == PolicyStatus.Draft);
        var reportsPendingApproval = await _db.RegulatoryReports.CountAsync(r => r.ReportStatus != null && r.ReportStatus.Name == "Submitted");
        var suppliersPending = await _db.Suppliers.CountAsync(s => s.ApprovalStatus == SupplierApprovalStatus.Pending);
        var openCapa = await _db.CorrectiveActions.CountAsync(c => c.Status != CorrectiveActionStatus.Closed && c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl);
        var overdueCapa = await _db.CorrectiveActions.CountAsync(c =>
            (c.Status == CorrectiveActionStatus.Overdue ||
            (c.Status != CorrectiveActionStatus.Closed &&
             c.Status != CorrectiveActionStatus.EvidenceSubmitted &&
             c.DueAtUtc < now)) && c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl);
        var activeHighAlerts = await _db.ComplianceAlerts.CountAsync(a =>
            a.IsActive &&
            (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical) &&
            a.TriggeredAtUtc >= rangeStart && a.TriggeredAtUtc < rangeEndExcl);
        var suppliersWithRiskSignals = await _db.Suppliers.CountAsync(s =>
            s.Status == SupplierStatus.OnHold ||
            s.Status == SupplierStatus.Suspended ||
            _db.SupplierComplianceDocuments.Any(d =>
                d.SupplierId == s.Id &&
                !d.IsDeleted &&
                (d.DocumentStatus == SupplierDocumentStatus.Missing ||
                 d.DocumentStatus == SupplierDocumentStatus.PendingReview ||
                 d.DocumentStatus == SupplierDocumentStatus.Expired ||
                 (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value < now))) ||
            _db.ComplianceAlerts.Any(a =>
                a.IsActive &&
                a.EntityType == "Supplier" &&
                a.EntityId == s.Id &&
                (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical)));
        var supplierScoreSummary = await BuildSupplierScoreSummaryAsync();
        var highRiskSuppliers = supplierScoreSummary.HighRiskCount;

        var complianceRate = totalChecks == 0 ? 0 : (decimal)compliantChecks / totalChecks * 100m;

        var rankings = await _db.ComplianceChecks
            .Include(c => c.ComplianceStatus)
            .Where(c => c.Policy != null && c.Policy.Owner != "")
            .GroupBy(c => c.Policy!.Owner)
            .Select(g => new
            {
                Owner = g.Key,
                Total = g.Count(),
                Compliant = g.Count(x => x.ComplianceStatus != null && x.ComplianceStatus.Name == "Compliant")
            })
            .OrderByDescending(x => x.Total == 0 ? 0 : (decimal)x.Compliant / x.Total)
            .Take(5)
            .ToListAsync();

        var rankingItems = rankings.Select(r =>
            new RoleDashboardItem(
                r.Owner,
                $"{(r.Total == 0 ? 0 : (decimal)r.Compliant / r.Total * 100m):0.#}% compliance ({r.Compliant}/{r.Total})",
                "good"))
            .ToList();

        var commonKpis = await GetCommonKpiMetricsAsync(from, to);
        var metrics = commonKpis.Concat(new List<RoleDashboardMetric>
        {
            new("Open Findings", openFindings.ToString(), "Company-wide non-compliant checks", Url.Action("Index", "ComplianceChecks")),
            new("Open CAPA", openCapa.ToString(), "Corrective actions not yet closed", Url.Action("Index", "Capa")),
            new("Suppliers Blocked (<50)", supplierScoreSummary.BlockedCount.ToString(), "Cannot be used for submit/approve", Url.Action("Index", "Suppliers")),
            new("High Risk Suppliers", highRiskSuppliers.ToString(), "Requires immediate attention", Url.Action("Index", "Suppliers"))
        }).ToList();

        var alertRows = await _db.ComplianceAlerts
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.TriggeredAtUtc)
            .Take(5)
            .ToListAsync();

        var alertItems = alertRows
            .Select(a => new RoleDashboardItem(
                a.Title,
                a.Message,
                a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High
                    ? "risk"
                    : a.Severity == ComplianceAlertSeverity.Warning
                        ? "warn"
                        : "neutral",
                a.EntityType == "Supplier" && a.EntityId.HasValue ? Url.Action("Details", "Suppliers", new { id = a.EntityId }) : Url.Action("Index", "ComplianceChecks")))
            .ToList();

        if (alertItems.Count == 0)
        {
            alertItems.Add(new RoleDashboardItem(
                "No Active Alerts",
                "No unresolved compliance alerts at this time.",
                "good"));
        }

        var riskiestSupplierItems = supplierScoreSummary.RiskiestSuppliers
            .Select(s => new RoleDashboardItem(
                s.SupplierName,
                $"Score {s.Score}/100 ({s.Band})",
                s.Band == "Blocked" || s.Band == "High Risk" ? "risk" : "warn",
                Url.Action("Details", "Suppliers", new { id = s.SupplierId })))
            .ToList();

        return new RoleDashboardViewModel(
            "Admin",
            "Business oversight and final internal approvals.",
            metrics,
            rankingItems,
            new List<RoleDashboardItem>
            {
                new("Suppliers Pending Approval", suppliersPending.ToString(), "warn", Url.Action("Index", "Suppliers")),
                new("Suppliers with Risk Signals", suppliersWithRiskSignals.ToString(), suppliersWithRiskSignals > 0 ? "risk" : "good", Url.Action("Index", "Suppliers")),
                new("CAPA Overdue", overdueCapa.ToString(), overdueCapa > 0 ? "risk" : "good", Url.Action("Index", "Capa")),
                new("Active High/Critical Alerts", activeHighAlerts.ToString(), activeHighAlerts > 0 ? "risk" : "neutral", Url.Action("Index", "ComplianceChecks"))
            }.Concat(riskiestSupplierItems).Concat(alertItems).ToList());
    }

    private async Task<RoleDashboardViewModel> BuildComplianceDashboardAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var rangeStart = from.HasValue ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc) : now.AddMonths(-1);
        var rangeEndExcl = to.HasValue ? DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc) : now.AddDays(1);

        var openFindings = await _db.ComplianceChecks.CountAsync(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl);
        var highPriorityFindings = await _db.ComplianceChecks.CountAsync(c =>
            c.ComplianceStatus != null &&
            c.ComplianceStatus.Name == "Non-Compliant" &&
            c.RiskLevel != null &&
            (c.RiskLevel.Name == "High" || c.RiskLevel.Name == "Critical") &&
            c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl);
        var capaOverdue = await _db.ComplianceAlerts.CountAsync(a =>
            a.IsActive && (a.Title.Contains("CAPA") || a.Message.Contains("CAPA")) &&
            (a.Title.Contains("overdue") || a.Message.Contains("overdue")) &&
            a.TriggeredAtUtc >= rangeStart && a.TriggeredAtUtc < rangeEndExcl);
        var upcomingDeadlines = await _db.RegulatoryReports.CountAsync(r =>
            r.PeriodEndUtc >= now &&
            r.PeriodEndUtc <= now.AddDays(30) &&
            (r.ReportStatus == null || r.ReportStatus.Name != "Accepted"));
        var supplierDocsForReview = await _db.SupplierComplianceDocuments.CountAsync(d => d.DocumentStatus == SupplierDocumentStatus.PendingReview);
        var escalatedViolations = await _db.ComplianceAlerts.CountAsync(a => a.IsActive && (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical) && a.TriggeredAtUtc >= rangeStart && a.TriggeredAtUtc < rangeEndExcl);
        var supplierScoreSummary = await BuildSupplierScoreSummaryAsync();
        var highRiskSuppliers = supplierScoreSummary.HighRiskCount;

        var alertRows = await _db.ComplianceAlerts
            .Where(a => a.IsActive && a.TriggeredAtUtc >= rangeStart && a.TriggeredAtUtc < rangeEndExcl)
            .OrderByDescending(a => a.TriggeredAtUtc)
            .Take(5)
            .ToListAsync();

        var alertItems = alertRows
            .Select(a => new RoleDashboardItem(
                a.Title,
                a.Message,
                a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High
                    ? "risk"
                    : a.Severity == ComplianceAlertSeverity.Warning
                        ? "warn"
                        : "neutral",
                a.EntityType == "Supplier" && a.EntityId.HasValue ? Url.Action("Details", "Suppliers", new { id = a.EntityId }) : Url.Action("Index", "ComplianceChecks")))
            .ToList();

        // Action Queue: Non-Compliant findings that need review/CAPA
        var pendingFindingsRaw = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Where(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEndExcl)
            .OrderByDescending(c => c.CheckedAtUtc)
            .Take(5)
            .ToListAsync();

        var actionItems = pendingFindingsRaw
            .Select(c => new RoleDashboardItem(
                c.Policy?.Title ?? "Compliance Finding",
                $"Non-Compliant logged on {c.CheckedAtUtc:yyyy-MM-dd}",
                "risk",
                Url.Action("Details", "ComplianceChecks", new { id = c.Id })))
            .ToList();

        var commonKpis = await GetCommonKpiMetricsAsync(from, to);
        var metrics = commonKpis.Concat(new List<RoleDashboardMetric>
        {
            new("Open Findings", openFindings.ToString(), "All non-compliant checks", Url.Action("Index", "ComplianceChecks")),
            new("CAPA Overdue", capaOverdue.ToString(), "Overdue CAPA signals", Url.Action("Index", "Compliance")),
            new("Suppliers Blocked (<50)", supplierScoreSummary.BlockedCount.ToString(), "Cannot pass procurement gate", Url.Action("Index", "Suppliers")),
            new("High Risk Suppliers", highRiskSuppliers.ToString(), "Requires preventive actions", Url.Action("Index", "Suppliers"))
        }).ToList();

        var riskiestSupplierItems = supplierScoreSummary.RiskiestSuppliers
            .Select(s => new RoleDashboardItem(
                s.SupplierName,
                $"Score {s.Score}/100 ({s.Band})",
                s.Band == "Blocked" || s.Band == "High Risk" ? "risk" : "warn",
                Url.Action("Details", "Suppliers", new { id = s.SupplierId })))
            .ToList();

        return new RoleDashboardViewModel(
            "Compliance Officer",
            "Operational compliance control and daily action queue.",
            metrics,
            actionItems,
            riskiestSupplierItems.Concat(alertItems).ToList());
    }

    private async Task<RoleDashboardViewModel> BuildDepartmentHeadDashboardAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var rangeStart = from ?? now.AddMonths(-1);
        var rangeEnd = to ?? now;
        var ownerKey = User.Identity?.Name ?? string.Empty;

        var ownedPolicyIds = await _db.Policies
            .Where(p => !p.IsDeleted && p.Owner == ownerKey)
            .Select(p => p.Id)
            .ToListAsync();

        var assignedPolicies = ownedPolicyIds.Count;
        var openCapa = await _db.ComplianceChecks
            .CountAsync(c =>
                ownedPolicyIds.Contains(c.PolicyId) &&
                c.ComplianceStatus != null &&
                c.ComplianceStatus.Name == "Non-Compliant");
        var compliant = await _db.ComplianceChecks
            .CountAsync(c =>
                ownedPolicyIds.Contains(c.PolicyId) &&
                c.ComplianceStatus != null &&
                c.ComplianceStatus.Name == "Compliant");
        var total = await _db.ComplianceChecks.CountAsync(c => ownedPolicyIds.Contains(c.PolicyId));
        var complianceScore = total == 0 ? 0 : (decimal)compliant / total * 100m;
        var overdueTasks = await _db.ComplianceChecks
            .CountAsync(c =>
                ownedPolicyIds.Contains(c.PolicyId) &&
                c.ComplianceStatus != null &&
                c.ComplianceStatus.Name == "Non-Compliant" &&
                c.CheckedAtUtc < now.AddDays(-14));

        var dueMonitoring = await _db.Policies.CountAsync(p =>
            !p.IsDeleted &&
            p.Owner == ownerKey &&
            !_db.ComplianceChecks.Any(c => c.PolicyId == p.Id && c.CheckedAtUtc >= now.AddDays(-30)));

        var recentOwnedChecksRows = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Where(c => ownedPolicyIds.Contains(c.PolicyId))
            .OrderByDescending(c => c.CheckedAtUtc)
            .Take(5)
            .ToListAsync();

        var recentOwnedChecks = recentOwnedChecksRows
            .Select(c => new RoleDashboardItem(
                c.Policy != null ? c.Policy.Title : "Policy",
                $"{c.CheckedAtUtc:yyyy-MM-dd} - {(c.ComplianceStatus != null ? c.ComplianceStatus.Name : "Unknown")}",
                c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" ? "risk" : "good",
                Url.Action("Details", "ComplianceChecks", new { id = c.Id })))
            .ToList();

        var commonKpis = await GetCommonKpiMetricsAsync(from, to);
        var metrics = commonKpis.Concat(new List<RoleDashboardMetric>
        {
            new("Assigned Policies", assignedPolicies.ToString(), "Scoped to your ownership", Url.Action("Index", "Policies")),
            new("Monitoring Tasks Due", dueMonitoring.ToString(), "No check in last 30 days", Url.Action("Index", "ComplianceChecks")),
            new("Open CAPA / Findings", openCapa.ToString(), "Non-compliant checks in your scope", Url.Action("Index", "ComplianceChecks")),
            new("Department Compliance Score", $"{complianceScore:0.#}%", "Your scoped portfolio only")
        }).ToList();

        return new RoleDashboardViewModel(
            "Department Head",
            "Department execution view. Data is scoped to your assigned policies.",
            metrics,
            recentOwnedChecks,
            new List<RoleDashboardItem>());
    }

    private async Task<RoleDashboardViewModel> BuildProcurementDashboardAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var hasDateFilter = from.HasValue || to.HasValue;
        var rangeStart = from.HasValue ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc) : now.AddMonths(-1);
        var rangeEndExcl = to.HasValue ? DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc) : now.AddDays(1);

        int expiringCerts, totalSuppliers, activeSuppliers, suspendedSuppliers, compliantSuppliers, docIssuesCount;
        int openCapa, pendingOrders, approvedOrders, ordersInRange;
        SupplierScoreSummary supplierScoreSummary;

        if (hasDateFilter)
        {
            ordersInRange = await _db.ProcurementOrders.CountAsync(o =>
                !o.IsDeleted && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl);
            var draftStatus = await _db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
            var approvedStatuses = await _db.ProcurementStatuses
                .Where(s => s.Name == "Approved" || s.Name == "Ordered" || s.Name == "Received")
                .Select(s => s.Id)
                .ToListAsync();
            pendingOrders = draftStatus != null
                ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && o.ProcurementStatusId == draftStatus.Id && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl)
                : 0;
            approvedOrders = approvedStatuses.Any()
                ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && approvedStatuses.Contains(o.ProcurementStatusId) && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl)
                : 0;

            var allActiveSupplierIds = await _db.ProcurementOrders
                .Where(o => !o.IsDeleted && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl)
                .Select(o => o.SupplierId)
                .Distinct()
                .ToListAsync();

            var capaSupplierIds = await _db.CorrectiveActions
                .Where(c => c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl && c.SupplierId != null)
                .Select(c => c.SupplierId!.Value)
                .Distinct()
                .ToListAsync();

            allActiveSupplierIds = allActiveSupplierIds.Union(capaSupplierIds).Distinct().ToList();

            totalSuppliers = allActiveSupplierIds.Count;
            activeSuppliers = await _db.Suppliers
                .Where(s => allActiveSupplierIds.Contains(s.Id) && s.Status == SupplierStatus.Active && s.ApprovalStatus == SupplierApprovalStatus.Approved)
                .CountAsync();
            suspendedSuppliers = await _db.Suppliers
                .Where(s => allActiveSupplierIds.Contains(s.Id) && (s.Status == SupplierStatus.Suspended || s.Status == SupplierStatus.OnHold))
                .CountAsync();
            openCapa = await _db.CorrectiveActions.CountAsync(c =>
                c.Status != CorrectiveActionStatus.Closed && c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl);
            var scopedIds = allActiveSupplierIds.Count > 0 ? allActiveSupplierIds : await _db.Suppliers.Where(s => !s.IsDeleted).Select(s => s.Id).ToListAsync();
            var scopedScores = await _complianceScore.GetSupplierComplianceScoresAsync(scopedIds);
            compliantSuppliers = scopedScores.Count(s => s.Score >= 90);
            docIssuesCount = scopedScores.Count(s => s.Score < 70);
            expiringCerts = await _db.SupplierComplianceDocuments.CountAsync(d =>
                !d.IsDeleted && d.ExpiryDateUtc.HasValue && allActiveSupplierIds.Contains(d.SupplierId) &&
                (d.ExpiryDateUtc.Value <= now.AddDays(30) || d.ExpiryDateUtc.Value < now));
            supplierScoreSummary = await BuildSupplierScoreSummaryAsync();
        }
        else
        {
            var draftStatus = await _db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
            var approvedStatuses = await _db.ProcurementStatuses
                .Where(s => s.Name == "Approved" || s.Name == "Ordered" || s.Name == "Received")
                .Select(s => s.Id)
                .ToListAsync();
            pendingOrders = draftStatus != null
                ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && o.ProcurementStatusId == draftStatus.Id)
                : 0;
            approvedOrders = approvedStatuses.Any()
                ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && approvedStatuses.Contains(o.ProcurementStatusId))
                : 0;
            ordersInRange = await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted);

            totalSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted);
            activeSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted && s.Status == SupplierStatus.Active && s.ApprovalStatus == SupplierApprovalStatus.Approved);
            suspendedSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted && (s.Status == SupplierStatus.Suspended || s.Status == SupplierStatus.OnHold));
            openCapa = await _db.CorrectiveActions.CountAsync(c => c.Status != CorrectiveActionStatus.Closed);

            supplierScoreSummary = await BuildSupplierScoreSummaryAsync();
            compliantSuppliers = supplierScoreSummary.CompliantCount;
            docIssuesCount = supplierScoreSummary.BlockedCount + supplierScoreSummary.HighRiskCount;

            expiringCerts = await _db.SupplierComplianceDocuments.CountAsync(d =>
                !d.IsDeleted && d.ExpiryDateUtc.HasValue &&
                (d.ExpiryDateUtc.Value <= now.AddDays(30) || d.ExpiryDateUtc.Value < now));
        }
        var metrics = new List<RoleDashboardMetric>
        {
            new("Expiring Certs", expiringCerts.ToString(), hasDateFilter ? "Docs expiring in range" : "Docs expiring in 30 days", Url.Action("Documents", "Suppliers", new { expiring = true })),
            new("Total Suppliers", totalSuppliers.ToString(), hasDateFilter ? "With activity in period" : "All suppliers", Url.Action("Index", "Suppliers")),
            new("Active Suppliers", activeSuppliers.ToString(), "Approved and active", Url.Action("Index", "Suppliers")),
            new("Suspended Suppliers", suspendedSuppliers.ToString(), "On hold or suspended", Url.Action("Index", "Suppliers")),
            new("Suppliers Compliant", compliantSuppliers.ToString(), "Score 90+", Url.Action("Index", "Suppliers")),
            new("Suppliers With Document Issues", docIssuesCount.ToString(), "Blocked or high risk", Url.Action("Documents", "Suppliers")),
            new("Open CAPA", openCapa.ToString(), hasDateFilter ? "Created in period" : "Corrective actions", Url.Action("Index", "Capa")),
            new("Pending Orders", pendingOrders.ToString(), "Awaiting approval", Url.Action("Index", "ProcurementOrders")),
            new("Approved Orders", approvedOrders.ToString(), hasDateFilter ? "Approved in period" : "Approved/Ordered", Url.Action("Index", "ProcurementOrders")),
            new("Orders in Range", ordersInRange.ToString(), "Orders in period", Url.Action("Index", "ProcurementOrders")),
            new("Suppliers Blocked (<50)", supplierScoreSummary.BlockedCount.ToString(), "Submit blocked", Url.Action("Index", "Suppliers")),
            new("High Risk Suppliers", supplierScoreSummary.HighRiskCount.ToString(), "Needs action", Url.Action("Index", "Suppliers"))
        };

        // Action Queue: Pending Supplier Approvals OR Poor Performance (in range when filter applied)
        var pendingSuppliersQuery = _db.Suppliers
            .Where(s => !s.IsDeleted && (s.ApprovalStatus == SupplierApprovalStatus.Pending || s.PerformanceScore < 3.0m));
        if (from.HasValue || to.HasValue)
            pendingSuppliersQuery = pendingSuppliersQuery.Where(s => s.CreatedAtUtc >= rangeStart && s.CreatedAtUtc < rangeEndExcl);
        var pendingSuppliersRaw = await pendingSuppliersQuery
            .OrderByDescending(s => s.CreatedAtUtc)
            .Take(5)
            .ToListAsync();

        var actionItems = pendingSuppliersRaw
            .Select(s => new RoleDashboardItem(
                s.Name,
                s.ApprovalStatus == SupplierApprovalStatus.Pending 
                     ? $"Pending Approval - Rating {s.Rating:0.0}"
                     : $"Poor Performance - Score {s.PerformanceScore:0.0}",
                 s.PerformanceScore < 3.0m ? "risk" : "warn",
                 Url.Action("Details", "Suppliers", new { id = s.Id })))
             .ToList();

        var riskiestSupplierItems = supplierScoreSummary.RiskiestSuppliers
            .Select(s => new RoleDashboardItem(
                s.SupplierName,
                $"Score {s.Score}/100 ({s.Band})",
                s.Band == "Blocked" || s.Band == "High Risk" ? "risk" : "warn",
                Url.Action("Details", "Suppliers", new { id = s.SupplierId })))
            .ToList();

        // Alerts: Active High/Critical Alerts for Suppliers or Orders (in range when filter applied)
        var alertQuery = _db.ComplianceAlerts
            .Where(a => a.IsActive && (a.EntityType == "Supplier" || a.EntityType == "ProcurementOrder"));
        if (from.HasValue || to.HasValue)
            alertQuery = alertQuery.Where(a => a.TriggeredAtUtc >= rangeStart && a.TriggeredAtUtc < rangeEndExcl);
        var alertRows = await alertQuery
            .OrderByDescending(a => a.TriggeredAtUtc)
            .Take(5)
            .ToListAsync();

        var alerts = alertRows
            .Select(a => new RoleDashboardItem(
                a.Title,
                a.Message,
                a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High ? "risk" : "warn",
                a.EntityType == "Supplier" && a.EntityId.HasValue ? Url.Action("Details", "Suppliers", new { id = a.EntityId }) : null))
            .ToList();

        return new RoleDashboardViewModel(
            "Procurement Officer",
            "Supplier lifecycle control and document completeness.",
            metrics,
            actionItems.Concat(riskiestSupplierItems).ToList(),
            alerts);
    }

    private async Task<List<RoleDashboardMetric>> GetCommonKpiMetricsAsync(DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;
        var hasFilter = from.HasValue || to.HasValue;
        var rangeStart = from.HasValue ? DateTime.SpecifyKind(from.Value.Date, DateTimeKind.Utc) : now.AddMonths(-1);
        var rangeEndExcl = to.HasValue ? DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc) : now.AddDays(1);

        int expiringCerts, totalSuppliers, activeSuppliers, suspendedSuppliers, compliantSuppliers, docIssuesCount;
        int openCapa, pendingOrders, approvedOrders, ordersInRange;

        if (hasFilter)
        {
            var orderSupplierIds = await _db.ProcurementOrders
                .Where(o => !o.IsDeleted && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl)
                .Select(o => o.SupplierId)
                .Distinct()
                .ToListAsync();
            var capaSupplierIds = await _db.CorrectiveActions
                .Where(c => c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl && c.SupplierId != null)
                .Select(c => c.SupplierId!.Value)
                .Distinct()
                .ToListAsync();
            var activeIds = orderSupplierIds.Union(capaSupplierIds).Distinct().ToList();

            totalSuppliers = activeIds.Count;
            activeSuppliers = await _db.Suppliers.CountAsync(s => activeIds.Contains(s.Id) && s.Status == SupplierStatus.Active && s.ApprovalStatus == SupplierApprovalStatus.Approved);
            suspendedSuppliers = await _db.Suppliers.CountAsync(s => activeIds.Contains(s.Id) && (s.Status == SupplierStatus.Suspended || s.Status == SupplierStatus.OnHold));
            openCapa = await _db.CorrectiveActions.CountAsync(c => c.Status != CorrectiveActionStatus.Closed && c.CreatedAtUtc >= rangeStart && c.CreatedAtUtc < rangeEndExcl);

            var draftStatus = await _db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
            var approvedStatusIds = await _db.ProcurementStatuses.Where(s => s.Name == "Approved" || s.Name == "Ordered" || s.Name == "Received").Select(s => s.Id).ToListAsync();
            pendingOrders = draftStatus != null ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && o.ProcurementStatusId == draftStatus.Id && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl) : 0;
            approvedOrders = approvedStatusIds.Any() ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && approvedStatusIds.Contains(o.ProcurementStatusId) && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl) : 0;
            ordersInRange = await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && o.OrderDateUtc >= rangeStart && o.OrderDateUtc < rangeEndExcl);

            var scopedIds = activeIds.Count > 0 ? activeIds : new List<Guid>();
            if (scopedIds.Count > 0)
            {
                var scores = await _complianceScore.GetSupplierComplianceScoresAsync(scopedIds);
                compliantSuppliers = scores.Count(s => s.Score >= 90);
                docIssuesCount = scores.Count(s => s.Score < 70);
            }
            else
            {
                compliantSuppliers = 0;
                docIssuesCount = 0;
            }
            expiringCerts = await _db.SupplierComplianceDocuments.CountAsync(d =>
                !d.IsDeleted && d.ExpiryDateUtc.HasValue && activeIds.Contains(d.SupplierId) &&
                (d.ExpiryDateUtc.Value <= now.AddDays(30) || d.ExpiryDateUtc.Value < now));
        }
        else
        {
            totalSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted);
            activeSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted && s.Status == SupplierStatus.Active && s.ApprovalStatus == SupplierApprovalStatus.Approved);
            suspendedSuppliers = await _db.Suppliers.CountAsync(s => !s.IsDeleted && (s.Status == SupplierStatus.Suspended || s.Status == SupplierStatus.OnHold));
            openCapa = await _db.CorrectiveActions.CountAsync(c => c.Status != CorrectiveActionStatus.Closed);

            var draftStatus = await _db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
            var approvedStatusIds = await _db.ProcurementStatuses.Where(s => s.Name == "Approved" || s.Name == "Ordered" || s.Name == "Received").Select(s => s.Id).ToListAsync();
            pendingOrders = draftStatus != null ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && o.ProcurementStatusId == draftStatus.Id) : 0;
            approvedOrders = approvedStatusIds.Any() ? await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted && approvedStatusIds.Contains(o.ProcurementStatusId)) : 0;
            ordersInRange = await _db.ProcurementOrders.CountAsync(o => !o.IsDeleted);

            var summary = await BuildSupplierScoreSummaryAsync();
            compliantSuppliers = summary.CompliantCount;
            docIssuesCount = summary.BlockedCount + summary.HighRiskCount;
            expiringCerts = await _db.SupplierComplianceDocuments.CountAsync(d =>
                !d.IsDeleted && d.ExpiryDateUtc.HasValue &&
                (d.ExpiryDateUtc.Value <= now.AddDays(30) || d.ExpiryDateUtc.Value < now));
        }

        return new List<RoleDashboardMetric>
        {
            new("Expiring Certs", expiringCerts.ToString(), hasFilter ? "In period" : "In 30 days", Url.Action("Documents", "Suppliers", new { expiring = true })),
            new("Total Suppliers", totalSuppliers.ToString(), hasFilter ? "With activity" : "All", Url.Action("Index", "Suppliers")),
            new("Active Suppliers", activeSuppliers.ToString(), "Approved", Url.Action("Index", "Suppliers")),
            new("Suspended Suppliers", suspendedSuppliers.ToString(), "On hold", Url.Action("Index", "Suppliers")),
            new("Suppliers Compliant", compliantSuppliers.ToString(), "Score 90+", Url.Action("Index", "Suppliers")),
            new("Suppliers With Document Issues", docIssuesCount.ToString(), "Blocked/risk", Url.Action("Documents", "Suppliers")),
            new("Open CAPA", openCapa.ToString(), hasFilter ? "In period" : "All", Url.Action("Index", "Capa")),
            new("Pending Orders", pendingOrders.ToString(), "Draft", Url.Action("Index", "ProcurementOrders")),
            new("Approved Orders", approvedOrders.ToString(), hasFilter ? "In period" : "All", Url.Action("Index", "ProcurementOrders")),
            new("Orders in Range", ordersInRange.ToString(), "In period", Url.Action("Index", "ProcurementOrders"))
        };
    }

    private async Task<SupplierScoreSummary> BuildSupplierScoreSummaryAsync()
    {
        var supplierIds = await _db.Suppliers
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .Select(s => s.Id)
            .ToListAsync();

        var scores = await _complianceScore.GetSupplierComplianceScoresAsync(supplierIds);
        var blockedCount = scores.Count(s => s.Score < 50);
        var highRiskCount = scores.Count(s => s.Score >= 50 && s.Score < 70);
        var compliantCount = scores.Count(s => s.Score >= 90);
        
        var riskiest = scores
            .OrderBy(s => s.Score)
            .Take(5)
            .Select(s => new RiskySupplierItem(s.SupplierId, s.SupplierName, s.Score, s.Band))
            .ToList();

        return new SupplierScoreSummary(blockedCount, highRiskCount, compliantCount, riskiest);
    }

    private record SupplierScoreSummary(
        int BlockedCount,
        int HighRiskCount,
        int CompliantCount,
        List<RiskySupplierItem> RiskiestSuppliers);

    private record RiskySupplierItem(
        Guid SupplierId,
        string SupplierName,
        int Score,
        string Band);

    [HttpGet]
    public async Task<IActionResult> Widgets(string? baseCurrency)
    {
        var baseCode = string.IsNullOrWhiteSpace(baseCurrency) ? "PHP" : baseCurrency.ToUpperInvariant();
        if (baseCode != "USD" && baseCode != "PHP")
        {
            baseCode = "PHP";
        }

        var weather = await GetWeatherAsync();
        var rates = await GetExchangeRatesAsync(baseCode);
        var holidays = await GetHolidaysAsync();

        return Ok(new
        {
            weather,
            exchangeRates = rates,
            holidays
        });
    }

    [HttpGet]
    public async Task<IActionResult> TrendData(int months = 6, string metric = "checks", [FromQuery] string? from = null, [FromQuery] string? to = null)
    {
        var nowUtc = DateTime.UtcNow;
        DateTime? fromDate = DateTime.TryParse(from, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var f) ? f : null;
        DateTime? toDate = DateTime.TryParse(to, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var t) ? t : null;
        var start = fromDate ?? nowUtc.AddMonths(-(months - 1)).Date;
        var endExcl = toDate.HasValue ? toDate.Value.Date.AddDays(1) : nowUtc.Date.AddDays(1);
        
        var queryMonths = new List<DateTime>();
        var current = new DateTime(start.Year, start.Month, 1);
        var last = new DateTime(endExcl.Year, endExcl.Month, 1).AddMonths(-1);
        
        while (current <= last)
        {
            queryMonths.Add(current);
            current = current.AddMonths(1);
        }

        var labels = queryMonths.Select(m => m.ToString("MMM yyyy")).ToList();
        var values = new List<int>();
        string title = "Monthly Activity";

        switch (metric.ToLowerInvariant())
        {
            case "reports":
                title = "Monthly Reports";
                foreach (var month in queryMonths)
                {
                    var nextMonth = month.AddMonths(1);
                    values.Add(await _db.RegulatoryReports.CountAsync(r => r.PeriodEndUtc >= month && r.PeriodEndUtc < nextMonth));
                }
                break;
            case "orders":
                title = "Monthly Orders";
                foreach (var month in queryMonths)
                {
                    var nextMonth = month.AddMonths(1);
                    values.Add(await _db.ProcurementOrders.CountAsync(o => o.OrderDateUtc >= month && o.OrderDateUtc < nextMonth && !o.IsDeleted));
                }
                break;
            default:
                title = "Monthly Checks";
                foreach (var month in queryMonths)
                {
                    var nextMonth = month.AddMonths(1);
                    values.Add(await _db.ComplianceChecks.CountAsync(c => c.CheckedAtUtc >= month && c.CheckedAtUtc < nextMonth));
                }
                break;
        }

        return Ok(new { labels, values, title });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcknowledgeAlert(Guid id)
    {
        var alert = await _db.ComplianceAlerts.FindAsync(id);
        if (alert == null)
        {
            return NotFound();
        }

        alert.IsAcknowledged = true;
        alert.UpdatedAtUtc = DateTime.UtcNow;
        _db.AuditLogs.Add(new FurniComply.Domain.Entities.AuditLog
        {
            EntityName = nameof(FurniComply.Domain.Entities.ComplianceAlert),
            EntityId = alert.Id,
            Action = "Acknowledge",
            Actor = User.Identity?.Name ?? "system",
            TimestampUtc = DateTime.UtcNow,
            Details = "Alert acknowledged."
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveAlert(Guid id)
    {
        var alert = await _db.ComplianceAlerts.FindAsync(id);
        if (alert == null)
        {
            return NotFound();
        }

        alert.IsActive = false;
        alert.ResolvedAtUtc = DateTime.UtcNow;
        alert.UpdatedAtUtc = DateTime.UtcNow;
        _db.AuditLogs.Add(new FurniComply.Domain.Entities.AuditLog
        {
            EntityName = nameof(FurniComply.Domain.Entities.ComplianceAlert),
            EntityId = alert.Id,
            Action = "Resolve",
            Actor = User.Identity?.Name ?? "system",
            TimestampUtc = DateTime.UtcNow,
            Details = "Alert resolved."
        });
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<WeatherWidget?> GetWeatherAsync()
    {
        var apiKey = _configuration["ExternalApis:OpenWeatherApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return null;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.openweathermap.org/data/2.5/weather?q=Davao%20City,PH&appid={apiKey}&units=metric";
            var response = await client.GetFromJsonAsync<OpenWeatherResponse>(url);
            if (response == null || response.Main == null || response.Weather == null || response.Weather.Length == 0)
            {
                return null;
            }

            var iconCode = response.Weather[0].Icon;
            var iconUrl = string.IsNullOrWhiteSpace(iconCode)
                ? null
                : $"https://openweathermap.org/img/wn/{iconCode}@2x.png";

            var uv = await GetUvIndexAsync(apiKey, response.Coord);
            var uvAlert = uv.HasValue ? GetUvAlert(uv.Value) : null;

            return new WeatherWidget(
                "Davao City",
                response.Main.Temp,
                response.Weather[0].Description,
                response.Main.Humidity,
                response.Wind?.Speed ?? 0,
                iconUrl,
                uv,
                uvAlert
            );
        }
        catch
        {
            return null;
        }
    }

    private async Task<ExchangeRateWidget?> GetExchangeRatesAsync(string baseCurrency)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://open.er-api.com/v6/latest/{baseCurrency}";
            var response = await client.GetFromJsonAsync<ErApiResponse>(url);
            if (response?.Rates == null || !string.Equals(response.Result, "success", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            response.Rates.TryGetValue("USD", out var usd);
            response.Rates.TryGetValue("PHP", out var php);

            var rates = new List<ExchangeRateItem>
            {
                new("USD", usd),
                new("PHP", php)
            };

            return new ExchangeRateWidget(baseCurrency, rates);
        }
        catch
        {
            return null;
        }
    }

    private async Task<HolidayWidget?> GetHolidaysAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://date.nager.at/api/v3/NextPublicHolidays/PH";
            var response = await client.GetFromJsonAsync<List<NagerHoliday>>(url);
            if (response == null)
            {
                return null;
            }

            var items = new List<HolidayItem>();
            foreach (var holiday in response)
            {
                if (items.Count >= 5)
                {
                    break;
                }

                items.Add(new HolidayItem(holiday.LocalName ?? holiday.Name ?? "Holiday", holiday.Date));
            }

            return new HolidayWidget("PH", items);
        }
        catch
        {
            return null;
        }
    }

    private sealed class OpenWeatherResponse
    {
        [JsonPropertyName("weather")]
        public OpenWeatherCondition[]? Weather { get; set; }

        [JsonPropertyName("main")]
        public OpenWeatherMain? Main { get; set; }

        [JsonPropertyName("wind")]
        public OpenWeatherWind? Wind { get; set; }

        [JsonPropertyName("coord")]
        public OpenWeatherCoord? Coord { get; set; }
    }

    private sealed class OpenWeatherCondition
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    private sealed class OpenWeatherMain
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    private sealed class OpenWeatherWind
    {
        [JsonPropertyName("speed")]
        public decimal Speed { get; set; }
    }

    private sealed class OpenWeatherCoord
    {
        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }

        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }
    }

    private sealed class ErApiResponse
    {
        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }
    }

    private sealed class NagerHoliday
    {
        [JsonPropertyName("localName")]
        public string? LocalName { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;
    }

    private async Task<decimal?> GetUvIndexAsync(string apiKey, OpenWeatherCoord? coord)
    {
        if (coord == null)
        {
            return null;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.openweathermap.org/data/3.0/onecall?lat={coord.Lat}&lon={coord.Lon}&appid={apiKey}&exclude=minutely,hourly,daily,alerts&units=metric";
            var response = await client.GetFromJsonAsync<OpenWeatherOneCallResponse>(url);
            return response?.Current?.Uvi;
        }
        catch
        {
            return null;
        }
    }

    private static string GetUvAlert(decimal uvi)
    {
        if (uvi >= 11) return "Extreme";
        if (uvi >= 8) return "Very High";
        if (uvi >= 6) return "High";
        if (uvi >= 3) return "Moderate";
        return "Low";
    }

    private sealed class OpenWeatherOneCallResponse
    {
        [JsonPropertyName("current")]
        public OpenWeatherCurrent? Current { get; set; }
    }

    private sealed class OpenWeatherCurrent
    {
        [JsonPropertyName("uvi")]
        public decimal Uvi { get; set; }
    }
}
