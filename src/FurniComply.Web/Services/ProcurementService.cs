using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Application.Interfaces;

namespace FurniComply.Web.Services;

public class ProcurementService : IProcurementService
{
    private readonly AppDbContext _db;
    private readonly ISupplierComplianceScoreService _complianceScore;

    public ProcurementService(AppDbContext db, ISupplierComplianceScoreService complianceScore)
    {
        _db = db;
        _complianceScore = complianceScore;
    }

    public async Task<OrderApprovalGateResult> GetOrderApprovalBlockerAsync(Guid orderId, string operation, string? actorName)
    {
        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return OrderApprovalGateResult.Blocked($"{operation} blocked: order not found.");
        }

        return await GetOrderApprovalBlockerAsync(order, operation, actorName);
    }

    public async Task<OrderApprovalGateResult> GetOrderApprovalBlockerAsync(ProcurementOrder order, string operation, string? actorName)
    {
        var isCreate = string.Equals(operation, "create", StringComparison.OrdinalIgnoreCase);
        var operationPrefix = string.Equals(operation, "submit", StringComparison.OrdinalIgnoreCase)
            ? "Submission"
            : isCreate ? "Creation" : "Approval";
        var documentBlockers = new List<string>();
        var riskBlockers = new List<string>();
        var isScoreBased = false;

        if (order.Supplier == null)
        {
            return OrderApprovalGateResult.Blocked($"{operationPrefix} blocked: supplier record was not found.");
        }

        // 1. Zero Items Check (Skip for Create because order isn't saved yet)
        if (!isCreate)
        {
            var hasItems = await _db.ProcurementItems.AnyAsync(i => i.ProcurementOrderId == order.Id && !i.IsDeleted);
            if (!hasItems)
            {
                return OrderApprovalGateResult.Blocked($"{operationPrefix} blocked: add at least one item before submitting the order.");
            }
        }

        // 2. RISK / QUALITY BLOCKERS (NEVER OVERRIDEABLE)
        if (order.Supplier.PerformanceScore > 0m && order.Supplier.PerformanceScore < 3.0m)
        {
            riskBlockers.Add($"Performance score {order.Supplier.PerformanceScore:0.0} (< 3.0).");
        }

        if (order.Supplier.Status == SupplierStatus.OnHold)
        {
            riskBlockers.Add($"Supplier OnHold.");
        }

        if (order.Supplier.ApprovalStatus != SupplierApprovalStatus.Approved)
        {
            riskBlockers.Add($"Supplier not approved.");
        }

        // Must have at least one compliance check performed before ordering
        var supplierName = order.Supplier.Name?.Trim() ?? string.Empty;
        var hasComplianceCheck = !string.IsNullOrEmpty(supplierName) &&
            (await _db.ComplianceChecks.AnyAsync(c => c.Notes.Contains(supplierName)) ||
             await _db.CorrectiveActions.AnyAsync(ca => ca.SupplierId == order.SupplierId && ca.ComplianceCheckId.HasValue));
        if (!hasComplianceCheck)
        {
            riskBlockers.Add($"No compliance check for '{order.Supplier.Name}'. Perform check first.");
        }

        // Active High/Critical alerts
        var riskAlerts = await _db.ComplianceAlerts
            .Where(a => a.EntityId == order.SupplierId && a.IsActive && (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical))
            .Select(a => $"{a.Severity} Alert: {a.Title}")
            .ToListAsync();
        if (riskAlerts.Any())
        {
            riskBlockers.AddRange(riskAlerts);
        }

        // Open/Overdue CAPAs
        var riskCapas = await _db.CorrectiveActions
            .Where(ca => ca.SupplierId == order.SupplierId && ca.Status != CorrectiveActionStatus.Closed)
            .Select(ca => $"Open CAPA: {ca.Title}")
            .ToListAsync();
        if (riskCapas.Any())
        {
            riskBlockers.AddRange(riskCapas);
        }

        // Score-derived blockers (Risk category)
        var scoreResult = await _complianceScore.GetSupplierComplianceScoreAsync(order.SupplierId);
        if (scoreResult != null)
        {
            if (scoreResult.Score < 70)
            {
                isScoreBased = true;
                riskBlockers.Add($"Compliance score {scoreResult.Score} ({scoreResult.Band}).");
            }
        }

        // 3. DOCUMENT BLOCKERS (OVERRIDEABLE)
        if (scoreResult != null)
        {
            if (scoreResult.MissingRequiredDocuments > 0) documentBlockers.Add($"{scoreResult.MissingRequiredDocuments} missing doc(s).");
            if (scoreResult.ExpiredRequiredDocuments > 0) documentBlockers.Add($"{scoreResult.ExpiredRequiredDocuments} expired doc(s).");
            if (scoreResult.PendingRequiredDocuments > 0) documentBlockers.Add($"{scoreResult.PendingRequiredDocuments} doc(s) pending.");
        }
        
        // If suspended, it's either because of docs or because of risk.
        if (order.Supplier.Status == SupplierStatus.Suspended)
        {
            if (!riskBlockers.Any())
            {
                documentBlockers.Add("Supplier Suspended (docs).");
            }
            else
            {
                riskBlockers.Add("Supplier Suspended.");
            }
        }

        // ENFORCEMENT logic
        if (riskBlockers.Any())
        {
            var message = $"{operationPrefix} blocked: {string.Join(" ", riskBlockers)} No override for risk.";
            return OrderApprovalGateResult.Blocked(message, isScoreBased, documentBlockers, riskBlockers);
        }

        if (documentBlockers.Any())
        {
            if (order.Supplier.IsOverrideActive && !string.IsNullOrWhiteSpace(order.Supplier.OverrideReason))
            {
                return OrderApprovalGateResult.AllowedWithDocumentOverride(
                    documentBlockers,
                    order.Supplier.OverrideReason!.Trim(),
                    order.Supplier.OverriddenBy);
            }

            var message = $"{operationPrefix} blocked (documents): {string.Join(" ", documentBlockers)} SuperAdmin may override.";
            return OrderApprovalGateResult.Blocked(message, isScoreBased, documentBlockers, riskBlockers);
        }

        return OrderApprovalGateResult.Allowed();
    }

    public async Task<Guid?> GetProcurementStatusIdAsync(string statusName)
    {
        return await _db.ProcurementStatuses
            .Where(s => s.Name == statusName)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GenerateNextOrderNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"PO-{year}-";
        var orderNumbers = await _db.ProcurementOrders
            .Where(o => o.OrderNumber.StartsWith(prefix))
            .Select(o => o.OrderNumber)
            .ToListAsync();

        var maxSequence = orderNumbers
            .Select(number =>
            {
                var sequenceText = number.Length > prefix.Length
                    ? number[prefix.Length..]
                    : string.Empty;
                return int.TryParse(sequenceText, out var n) ? n : 0;
            })
            .DefaultIfEmpty(0)
            .Max();

        var nextSequence = maxSequence + 1;
        var nextOrderNumber = $"{prefix}{nextSequence:0000}";

        while (await _db.ProcurementOrders.AnyAsync(o => o.OrderNumber == nextOrderNumber))
        {
            nextSequence++;
            nextOrderNumber = $"{prefix}{nextSequence:0000}";
        }

        return nextOrderNumber;
    }
}
