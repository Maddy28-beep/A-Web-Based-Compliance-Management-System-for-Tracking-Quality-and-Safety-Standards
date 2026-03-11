using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Application.Interfaces;

namespace FurniComply.Web.Services;

public class SupplierPerformanceService : ISupplierPerformanceService
{
    private readonly AppDbContext _db;

    public SupplierPerformanceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task EvaluatePerformanceAsync(Guid supplierId, int qualityRating, int deliveryRating, decimal defectRate, string remarks, string evaluatedBy, string? productName = null, Guid? productId = null)
    {
        var supplier = await _db.Suppliers.FindAsync(supplierId);
        if (supplier == null) return;

        var performance = new SupplierPerformance
        {
            SupplierId = supplierId,
            EvaluationDateUtc = DateTime.UtcNow,
            ProductName = productName,
            ProductId = productId,
            QualityRating = qualityRating,
            DeliveryRating = deliveryRating,
            DefectRate = defectRate,
            PerformanceScore = (qualityRating + deliveryRating) / 2m,
            Remarks = remarks,
            EvaluatedBy = evaluatedBy
        };

        _db.SupplierPerformances.Add(performance);
        
        // Use the latest score for procurement gating
        supplier.PerformanceScore = performance.PerformanceScore;
        supplier.Rating = supplier.PerformanceScore; // Use score as rating for simplicity

        // Trigger CAPA if score < 3 or high defect rate (>= 10%)
        if (performance.PerformanceScore < 3.0m || defectRate >= 10m)
        {
            var capa = new CorrectiveAction
            {
                SupplierId = supplierId,
                IssueType = CorrectiveActionType.Quality,
                Title = "Performance Improvement Required",
                Description = $"Poor performance score ({performance.PerformanceScore:0.0}) or high defect rate ({defectRate:0.0}%). Remarks: {remarks}",
                Status = CorrectiveActionStatus.Open,
                DueAtUtc = DateTime.UtcNow.AddDays(14),
                AssignedTo = "Procurement Team"
            };
            _db.CorrectiveActions.Add(capa);
        }

        await UpdateSupplierRiskStatusAsync(supplierId);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateSupplierRiskStatusAsync(Guid supplierId)
    {
        var supplier = await _db.Suppliers
            .Include(s => s.ComplianceDocuments)
            .FirstOrDefaultAsync(s => s.Id == supplierId);
            
        if (supplier == null) return;

        // NEVER automatically change status if already OnHold (manual state)
        if (supplier.Status == SupplierStatus.OnHold)
        {
            return;
        }

        var lowRiskId = await _db.RiskLevels.Where(r => r.Name == "Low").Select(r => r.Id).FirstOrDefaultAsync();
        var mediumRiskId = await _db.RiskLevels.Where(r => r.Name == "Medium").Select(r => r.Id).FirstOrDefaultAsync();
        var highRiskId = await _db.RiskLevels.Where(r => r.Name == "High").Select(r => r.Id).FirstOrDefaultAsync();

        // 1. Compliance/Risk Logic: Check for documents, alerts, and CAPAs
        var now = DateTime.UtcNow;
        var hasExpiredDocs = supplier.ComplianceDocuments.Any(d => 
            !d.IsDeleted && 
            (d.DocumentStatus == SupplierDocumentStatus.Expired || 
            (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value < now)));

        var hasRiskAlerts = await _db.ComplianceAlerts.AnyAsync(a => 
            a.EntityId == supplierId && a.IsActive && 
            (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical));

        var hasRiskCapa = await _db.CorrectiveActions.AnyAsync(ca => 
            ca.SupplierId == supplierId && ca.Status != CorrectiveActionStatus.Closed);

        if (hasExpiredDocs || hasRiskAlerts || hasRiskCapa || supplier.PerformanceScore < 3.0m)
        {
            supplier.Status = SupplierStatus.Suspended;
            supplier.RiskLevelId = highRiskId;
            return;
        }

        // Rule 3: Automatic Recovery - If no blockers remain, return to Active
        supplier.Status = SupplierStatus.Active;
        supplier.RiskLevelId = lowRiskId;
    }

    public async Task<decimal> CalculateOverallScoreAsync(Guid supplierId)
    {
        var supplier = await _db.Suppliers.FindAsync(supplierId);
        if (supplier == null) return 0m;

        var performances = await _db.SupplierPerformances
            .Where(p => p.SupplierId == supplierId)
            .ToListAsync();

        if (!performances.Any()) return 0m;

        return performances.Average(p => p.PerformanceScore);
    }

    public async Task<bool> CanSupplierBeUsedAsync(Guid supplierId)
    {
        var supplier = await _db.Suppliers.FindAsync(supplierId);
        if (supplier == null) return false;

        // 1. RISK / QUALITY CHECK (NEVER OVERRIDEABLE)
        // Performance Score < 3.0 is a HARD block
        if (supplier.PerformanceScore > 0m && supplier.PerformanceScore < 3.0m)
        {
            return false;
        }

        // Operational status (OnHold) is a HARD block
        // Suspended status is only a HARD block if there are NO document issues (meaning it's a risk suspension)
        if (supplier.Status == SupplierStatus.OnHold)
        {
            return false;
        }

        if (supplier.ApprovalStatus != SupplierApprovalStatus.Approved)
        {
            return false;
        }

        // Must have at least one compliance check before ordering (HARD block)
        var supplierName = supplier.Name?.Trim() ?? string.Empty;
        var hasComplianceCheck = !string.IsNullOrEmpty(supplierName) &&
            (await _db.ComplianceChecks.AnyAsync(c => c.Notes.Contains(supplierName)) ||
             await _db.CorrectiveActions.AnyAsync(ca => ca.SupplierId == supplierId && ca.ComplianceCheckId.HasValue));
        if (!hasComplianceCheck)
        {
            return false;
        }

        // Active High/Critical alerts are a HARD block
        var hasRiskAlerts = await _db.ComplianceAlerts.AnyAsync(a => 
            a.EntityId == supplierId && 
            a.IsActive && 
            (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical));
        if (hasRiskAlerts)
        {
            return false;
        }

        // Open/Overdue CAPAs are a HARD block
        var hasRiskCapa = await _db.CorrectiveActions.AnyAsync(ca => 
            ca.SupplierId == supplierId && 
            ca.Status != CorrectiveActionStatus.Closed);
        if (hasRiskCapa)
        {
            return false;
        }

        // 2. DOCUMENT CHECK (OVERRIDEABLE)
        var now = DateTime.UtcNow;
        var hasDocBlockers = await _db.SupplierComplianceDocuments
            .AnyAsync(d => d.SupplierId == supplierId && (
                d.DocumentStatus == SupplierDocumentStatus.Missing ||
                d.DocumentStatus == SupplierDocumentStatus.PendingReview ||
                d.DocumentStatus == SupplierDocumentStatus.Expired ||
                (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value < now)));

        // If suspended, it's either because of docs or because of risk.
        // If we reached here, there are NO risk factors, so it must be because of docs.
        var isSuspended = supplier.Status == SupplierStatus.Suspended;

        if (hasDocBlockers || isSuspended)
        {
            // Only allow if SuperAdmin override is active AND has a mandatory reason
            return supplier.IsOverrideActive && !string.IsNullOrWhiteSpace(supplier.OverrideReason);
        }

        return true;
    }
}
