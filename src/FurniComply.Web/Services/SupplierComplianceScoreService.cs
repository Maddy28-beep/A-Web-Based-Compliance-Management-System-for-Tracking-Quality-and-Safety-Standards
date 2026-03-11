using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Application.Interfaces;
using FurniComply.Application.Models;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Services;

public sealed class SupplierComplianceScoreService : ISupplierComplianceScoreService
{
    private static readonly string[] RequiredDocumentTypes =
    {
        "Business Permit",
        "Environmental Certificate",
        "Sustainability Declaration"
    };

    private readonly AppDbContext _db;

    public SupplierComplianceScoreService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SupplierComplianceScoreResult?> GetSupplierComplianceScoreAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var supplier = await _db.Suppliers
            .AsNoTracking()
            .Where(s => s.Id == supplierId)
            .Select(s => new { s.Id, s.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (supplier == null)
        {
            return null;
        }

        var now = DateTime.UtcNow;
        var docRows = await _db.SupplierComplianceDocuments
            .AsNoTracking()
            .Where(d => d.SupplierId == supplierId)
            .Select(d => new
            {
                d.DocumentType,
                d.DocumentStatus,
                d.ExpiryDateUtc,
                d.UpdatedAtUtc,
                d.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        var docsByType = docRows
            .GroupBy(d => d.DocumentType.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g
                .OrderByDescending(d => d.UpdatedAtUtc)
                .ThenByDescending(d => d.CreatedAtUtc)
                .First())
            .ToDictionary(d => d.DocumentType, d => d, StringComparer.OrdinalIgnoreCase);

        var missingRequired = 0;
        var expiredRequired = 0;
        var pendingRequired = 0;
        var reasons = new List<(string Reason, int Weight)>();

        foreach (var requiredType in RequiredDocumentTypes)
        {
            if (!docsByType.TryGetValue(requiredType, out var document) ||
                document.DocumentStatus == SupplierDocumentStatus.Missing)
            {
                missingRequired++;
                reasons.Add(($"Missing {requiredType}", 15));
                continue;
            }

            var isExpired =
                document.DocumentStatus == SupplierDocumentStatus.Expired ||
                (document.ExpiryDateUtc.HasValue && document.ExpiryDateUtc.Value.Date < now.Date);

            if (isExpired)
            {
                expiredRequired++;
                reasons.Add(($"Expired {requiredType}", 15));
                continue;
            }

            if (document.DocumentStatus == SupplierDocumentStatus.PendingReview)
            {
                pendingRequired++;
                reasons.Add(($"Pending review {requiredType}", 10));
            }
        }

        var supplierAlerts = await _db.ComplianceAlerts
            .AsNoTracking()
            .Where(a =>
                a.IsActive &&
                a.EntityType == "Supplier" &&
                a.EntityId == supplierId &&
                (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical))
            .Select(a => a.Severity)
            .ToListAsync(cancellationToken);

        var highAlerts = supplierAlerts.Count(s => s == ComplianceAlertSeverity.High);
        var criticalAlerts = supplierAlerts.Count(s => s == ComplianceAlertSeverity.Critical);

        if (highAlerts > 0)
        {
            reasons.Add(($"{highAlerts} High alert(s)", highAlerts * 10));
        }

        if (criticalAlerts > 0)
        {
            reasons.Add(($"{criticalAlerts} Critical alert(s)", criticalAlerts * 20));
        }

        var openCapaRows = await _db.CorrectiveActions
            .AsNoTracking()
            .Where(c => c.SupplierId == supplierId && c.Status != CorrectiveActionStatus.Closed)
            .Select(c => new { c.Status, c.DueAtUtc })
            .ToListAsync(cancellationToken);

        var overdueCapa = openCapaRows.Count(c =>
            c.Status == CorrectiveActionStatus.Overdue ||
            (c.DueAtUtc < now && c.Status != CorrectiveActionStatus.EvidenceSubmitted));
        var openCapa = Math.Max(0, openCapaRows.Count - overdueCapa);

        if (overdueCapa > 0)
        {
            reasons.Add(($"{overdueCapa} CAPA overdue", overdueCapa * 5));
        }

        if (openCapa > 0)
        {
            reasons.Add(($"{openCapa} CAPA open", openCapa * 2));
        }

        var totalDeductions =
            (missingRequired * 15) +
            (expiredRequired * 15) +
            (pendingRequired * 10) +
            (highAlerts * 10) +
            (criticalAlerts * 20) +
            (overdueCapa * 5) +
            (openCapa * 2);

        var score = Math.Max(0, 100 - totalDeductions);
        var band = GetBand(score);
        var topReasons = reasons
            .OrderByDescending(r => r.Weight)
            .ThenBy(r => r.Reason)
            .Select(r => r.Reason)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(3)
            .ToList();

        return new SupplierComplianceScoreResult(
            supplier.Id,
            supplier.Name,
            score,
            band,
            topReasons,
            missingRequired,
            expiredRequired,
            pendingRequired,
            highAlerts,
            criticalAlerts,
            overdueCapa,
            openCapa);
    }

    public async Task<IReadOnlyList<SupplierComplianceScoreResult>> GetSupplierComplianceScoresAsync(IEnumerable<Guid> supplierIds, CancellationToken cancellationToken = default)
    {
        var distinctIds = supplierIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        var results = new List<SupplierComplianceScoreResult>(distinctIds.Count);
        foreach (var supplierId in distinctIds)
        {
            var score = await GetSupplierComplianceScoreAsync(supplierId, cancellationToken);
            if (score != null)
            {
                results.Add(score);
            }
        }

        return results;
    }

    private static string GetBand(int score)
    {
        if (score >= 90) return "Compliant";
        if (score >= 70) return "Adequate";
        if (score >= 50) return "Needs Attention";
        return "Blocked";
    }
}
