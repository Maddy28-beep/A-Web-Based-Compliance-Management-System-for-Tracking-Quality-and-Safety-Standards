using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Application.Interfaces;
using FurniComply.Application.Models;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Infrastructure.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AnalyticsSnapshot> GetSnapshotAsync(DateTime? from = null, DateTime? to = null, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var rangeEnd = to ?? monthStart.AddMonths(1);
        var rangeStart = from ?? rangeEnd.AddMonths(-6);

        var activePolicies = await _db.Policies.CountAsync(p => p.Status == PolicyStatus.Active, cancellationToken);
        var complianceChecksThisMonth = await _db.ComplianceChecks.CountAsync(c => c.CheckedAtUtc >= monthStart, cancellationToken);
        
        // Filter core counts by date range if provided
        var openNonCompliance = await _db.ComplianceChecks.CountAsync(
            c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEnd,
            cancellationToken);
        var compliantChecks = await _db.ComplianceChecks.CountAsync(
            c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEnd,
            cancellationToken);
        var nonCompliantChecks = await _db.ComplianceChecks.CountAsync(
            c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant" && c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEnd,
            cancellationToken);
        
        var suppliersOnHold = await _db.Suppliers.CountAsync(s => s.Status == SupplierStatus.OnHold, cancellationToken);
        var pendingSupplierApprovals = await _db.Suppliers.CountAsync(
            s => s.ApprovalStatus == SupplierApprovalStatus.Pending,
            cancellationToken);
        var pendingPolicyApprovals = await _db.Policies.CountAsync(p => p.Status == PolicyStatus.Draft, cancellationToken);
        var pendingReportApprovals = await _db.RegulatoryReports.CountAsync(
            r => r.ReportStatus != null && r.ReportStatus.Name == "Submitted",
            cancellationToken);
        var pendingProcurementApprovals = await _db.ProcurementOrders.CountAsync(
            p => p.ProcurementStatus != null && p.ProcurementStatus.Name == "Draft",
            cancellationToken);
        var openReports = await _db.RegulatoryReports.CountAsync(
            r => r.ReportStatus != null && (r.ReportStatus.Name == "Draft" || r.ReportStatus.Name == "Submitted") && r.CreatedAtUtc >= rangeStart && r.CreatedAtUtc < rangeEnd,
            cancellationToken);
        var procurementOrdersOpen = await _db.ProcurementOrders.CountAsync(
            p => p.ProcurementStatus != null && p.ProcurementStatus.Name != "Closed" && p.ProcurementStatus.Name != "Cancelled" && p.OrderDateUtc >= rangeStart && p.OrderDateUtc < rangeEnd,
            cancellationToken);

        var monthlyGroups = await _db.ComplianceChecks
            .AsNoTracking()
            .Where(c => c.CheckedAtUtc >= rangeStart && c.CheckedAtUtc < rangeEnd)
            .GroupBy(c => new { c.CheckedAtUtc.Year, c.CheckedAtUtc.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var monthlySeries = new List<MonthlyCount>();
        var monthsCount = ((rangeEnd.Year - rangeStart.Year) * 12) + rangeEnd.Month - rangeStart.Month;
        for (var i = 0; i < Math.Max(1, monthsCount); i++)
        {
            var month = rangeStart.AddMonths(i);
            var match = monthlyGroups.Find(g => g.Year == month.Year && g.Month == month.Month);
            var label = month.ToString("MMM yyyy", CultureInfo.InvariantCulture);
            monthlySeries.Add(new MonthlyCount(label, match?.Count ?? 0));
        }

        var alerts = await _db.ComplianceAlerts
            .AsNoTracking()
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.Severity)
            .ThenByDescending(a => a.TriggeredAtUtc)
            .Take(8)
            .Select(a => new ComplianceAlertItem(
                a.Id,
                a.Title,
                a.Message,
                a.Severity.ToString(),
                a.IsAcknowledged,
                a.TriggeredAtUtc))
            .ToListAsync(cancellationToken);

        var insights = new List<string>();
        
        // 1. Compliance Insight
        if (nonCompliantChecks > 0)
        {
            var percentage = (double)nonCompliantChecks / (compliantChecks + nonCompliantChecks) * 100;
            insights.Add($"{percentage:F1}% of audits are non-compliant, requiring immediate CAPA follow-up.");
        }
        else if (compliantChecks > 0)
        {
            insights.Add("100% compliance rate achieved across recent checks.");
        }

        // 2. Supplier Risk Insight
        if (suppliersOnHold > 0)
        {
            insights.Add($"{suppliersOnHold} suppliers are currently On Hold due to critical document expiry or failed audits.");
        }

        // 3. Procurement Volume Insight
        if (procurementOrdersOpen > 10)
        {
            insights.Add("High procurement volume detected (10+ open orders), suggesting increased operational load.");
        }

        // 4. Monthly Trend Insight
        if (monthlySeries.Count >= 2)
        {
            var last = monthlySeries[^1].Count;
            var prev = monthlySeries[^2].Count;
            if (last > prev)
            {
                insights.Add($"Audit activity increased by {last - prev} checks this month compared to last month.");
            }
            else if (last < prev)
            {
                insights.Add($"Audit activity slowed down this month (down by {prev - last} checks).");
            }
        }

        return new AnalyticsSnapshot(
            activePolicies,
            complianceChecksThisMonth,
            openNonCompliance,
            suppliersOnHold,
            pendingSupplierApprovals,
            pendingPolicyApprovals,
            pendingReportApprovals,
            pendingProcurementApprovals,
            openReports,
            procurementOrdersOpen,
            compliantChecks,
            nonCompliantChecks,
            monthlySeries,
            alerts,
            insights
        );
    }
}


