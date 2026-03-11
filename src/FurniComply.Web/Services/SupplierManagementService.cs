using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Application.Interfaces;

namespace FurniComply.Web.Services;

public class SupplierManagementService : ISupplierManagementService
{
    private readonly AppDbContext _db;
    private readonly ISupplierPerformanceService _performance;

    public SupplierManagementService(AppDbContext db, ISupplierPerformanceService performance)
    {
        _db = db;
        _performance = performance;
    }

    public async Task<List<SupplierTimelineItem>> BuildSupplierTimelineAsync(Guid supplierId, string supplierName, Func<string, string, object, string?> urlHelper)
    {
        var timeline = new List<SupplierTimelineItem>();

        var documents = await _db.SupplierComplianceDocuments
            .Where(d => d.SupplierId == supplierId)
            .OrderByDescending(d => d.UpdatedAtUtc)
            .Select(d => new
            {
                d.Id,
                d.DocumentType,
                d.DocumentStatus,
                d.ExpiryDateUtc,
                d.CreatedAtUtc,
                d.UpdatedAtUtc
            })
            .ToListAsync();

        foreach (var doc in documents)
        {
            var eventDate = doc.UpdatedAtUtc ?? doc.CreatedAtUtc;
            var isExpired = doc.DocumentStatus == SupplierDocumentStatus.Expired ||
                            (doc.ExpiryDateUtc.HasValue && doc.ExpiryDateUtc.Value.Date < DateTime.UtcNow.Date);
            var severity = isExpired || doc.DocumentStatus == SupplierDocumentStatus.Missing
                ? "High"
                : doc.DocumentStatus == SupplierDocumentStatus.PendingReview
                    ? "Medium"
                    : "Info";
            var title = (doc.UpdatedAtUtc.HasValue ? "Document Updated" : "Document Uploaded") + $": {doc.DocumentType}";
            var description = $"Status: {doc.DocumentStatus}. Expiry: {(doc.ExpiryDateUtc.HasValue ? doc.ExpiryDateUtc.Value.ToString("yyyy-MM-dd") : "-")}.";

            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = eventDate,
                EventType = "Document",
                Severity = severity,
                Title = title,
                Description = description,
                SourceId = doc.Id.ToString(),
                SourceRoute = urlHelper("DocumentEdit", "Suppliers", new { id = doc.Id })
            });
        }

        var capaCheckIds = await _db.CorrectiveActions
            .Where(c => c.SupplierId == supplierId && c.ComplianceCheckId.HasValue)
            .Select(c => c.ComplianceCheckId!.Value)
            .Distinct()
            .ToListAsync();

        var checks = await _db.ComplianceChecks
            .Include(c => c.Policy)
            .Include(c => c.ComplianceStatus)
            .Include(c => c.RiskLevel)
            .Where(c => capaCheckIds.Contains(c.Id) || c.Notes.Contains(supplierName))
            .ToListAsync();

        foreach (var check in checks)
        {
            var riskName = check.RiskLevel?.Name ?? "-";
            var statusName = check.ComplianceStatus?.Name ?? "-";
            var isRiskHigh = riskName.Equals("High", StringComparison.OrdinalIgnoreCase) || riskName.Equals("Critical", StringComparison.OrdinalIgnoreCase);
            var severity = isRiskHigh
                ? "High"
                : riskName.Equals("Medium", StringComparison.OrdinalIgnoreCase)
                    ? "Medium"
                    : "Info";

            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = check.CheckedAtUtc,
                EventType = "ComplianceCheck",
                Severity = severity,
                Title = $"Compliance Check: {(check.Policy?.Code ?? "N/A")} - {(check.Policy?.Title ?? "Policy")}",
                Description = $"Status: {statusName}. Risk: {riskName}.",
                SourceId = check.Id.ToString(),
                SourceRoute = urlHelper("Details", "ComplianceChecks", new { id = check.Id })
            });
        }

        var capas = await _db.CorrectiveActions
            .Where(c => c.SupplierId == supplierId)
            .Select(c => new
            {
                c.Id,
                c.Title,
                c.Status,
                c.DueAtUtc,
                c.ClosedAtUtc,
                c.CreatedAtUtc,
                c.UpdatedAtUtc
            })
            .ToListAsync();

        foreach (var capa in capas)
        {
            var now = DateTime.UtcNow;
            var isOverdue = capa.Status == CorrectiveActionStatus.Overdue || (capa.Status != CorrectiveActionStatus.Closed && capa.DueAtUtc < now);
            var severity = isOverdue
                ? "High"
                : capa.Status == CorrectiveActionStatus.Closed
                    ? "Info"
                    : "Medium";

            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = capa.ClosedAtUtc ?? capa.UpdatedAtUtc ?? capa.CreatedAtUtc,
                EventType = "CAPA",
                Severity = severity,
                Title = $"CAPA: {capa.Title}",
                Description = $"Status: {capa.Status}. Due: {capa.DueAtUtc:yyyy-MM-dd}.",
                SourceId = capa.Id.ToString(),
                SourceRoute = urlHelper("Details", "Capa", new { id = capa.Id })
            });
        }

        var capaIds = capas.Select(c => c.Id).ToHashSet();
        var checkIds = checks.Select(c => c.Id).ToHashSet();

        var alerts = await _db.ComplianceAlerts
            .Where(a =>
                (a.EntityType == "Supplier" && a.EntityId == supplierId) ||
                (a.EntityType == "ComplianceCheck" && a.EntityId.HasValue && checkIds.Contains(a.EntityId.Value)) ||
                (a.EntityType == "CorrectiveAction" && a.EntityId.HasValue && capaIds.Contains(a.EntityId.Value)))
            .Select(a => new
            {
                a.Id,
                a.Severity,
                a.Title,
                a.Message,
                a.IsActive,
                a.TriggeredAtUtc,
                a.ResolvedAtUtc
            })
            .ToListAsync();

        foreach (var alert in alerts)
        {
            var severity = (alert.Severity == ComplianceAlertSeverity.Critical || alert.Severity == ComplianceAlertSeverity.High)
                ? "High"
                : alert.Severity == ComplianceAlertSeverity.Warning
                    ? "Medium"
                    : "Info";

            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = alert.ResolvedAtUtc ?? alert.TriggeredAtUtc,
                EventType = "Alert",
                Severity = severity,
                Title = alert.IsActive ? $"Alert Raised: {alert.Title}" : $"Alert Resolved: {alert.Title}",
                Description = alert.Message,
                SourceId = alert.Id.ToString(),
                SourceRoute = urlHelper("Index", "Dashboard", new { })
            });
        }

        var evaluations = await _db.SupplierEvaluations
            .Where(e => e.SupplierId == supplierId)
            .OrderByDescending(e => e.EvaluatedAtUtc)
            .ToListAsync();

        foreach (var eval in evaluations)
        {
            var severity = eval.BandSnapshot.Equals("Blocked", StringComparison.OrdinalIgnoreCase) ||
                           eval.BandSnapshot.Equals("High Risk", StringComparison.OrdinalIgnoreCase)
                ? "High"
                : eval.BandSnapshot.Equals("At Risk", StringComparison.OrdinalIgnoreCase)
                    ? "Medium"
                    : "Info";

            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = eval.EvaluatedAtUtc,
                EventType = "Evaluation",
                Severity = severity,
                Title = "Supplier Evaluated",
                Description = $"Score: {eval.ScoreSnapshot}/100 ({eval.BandSnapshot}). Notes: {(string.IsNullOrWhiteSpace(eval.Notes) ? "-" : eval.Notes)}",
                SourceId = eval.SupplierEvaluationId.ToString(),
                SourceRoute = urlHelper("Details", "Suppliers", new { id = supplierId }) + "#evaluation-history",
                Actor = eval.EvaluatedByEmail
            });
        }

        var overrideAndSupplierAudits = await _db.AuditLogs
            .Where(a => a.EntityName == nameof(Supplier) && a.EntityId == supplierId &&
                        (a.Action == "OverrideEnabled" || a.Action == "OverrideDisabled"))
            .OrderByDescending(a => a.TimestampUtc)
            .ToListAsync();

        foreach (var audit in overrideAndSupplierAudits)
        {
            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = audit.TimestampUtc,
                EventType = "Override",
                Severity = "Medium",
                Title = audit.Action == "OverrideEnabled" ? "Override Enabled" : "Override Disabled",
                Description = audit.Details,
                SourceId = audit.Id.ToString(),
                SourceRoute = urlHelper("Details", "Suppliers", new { id = supplierId }),
                Actor = audit.Actor
            });
        }

        var supplierIdToken = supplierId.ToString();
        var blockedOrderAudits = await _db.AuditLogs
            .Where(a =>
                a.EntityName == nameof(ProcurementOrder) &&
                (a.Action == "Submit Blocked" || a.Action == "Submit Blocked (Score)" ||
                 a.Action == "Approve Blocked" || a.Action == "Approve Blocked (Score)") &&
                a.Details.Contains(supplierIdToken))
            .OrderByDescending(a => a.TimestampUtc)
            .Take(50)
            .ToListAsync();

        foreach (var audit in blockedOrderAudits)
        {
            timeline.Add(new SupplierTimelineItem
            {
                DateUtc = audit.TimestampUtc,
                EventType = "Audit",
                Severity = "High",
                Title = audit.Action,
                Description = audit.Details,
                SourceId = audit.EntityId.ToString(),
                SourceRoute = urlHelper("Details", "ProcurementOrders", new { id = audit.EntityId }),
                Actor = audit.Actor
            });
        }

        return timeline
            .OrderByDescending(item => item.DateUtc)
            .ToList();
    }

    public async Task<string?> SaveDocumentFileAsync(IFormFile? upload, string? existingUrl, string webRootPath)
    {
        if (upload == null || upload.Length == 0)
        {
            return existingUrl;
        }

        var uploadsRoot = Path.Combine(webRootPath, "uploads", "supplier-docs");
        Directory.CreateDirectory(uploadsRoot);

        var safeName = Path.GetFileName(upload.FileName);
        var fileName = $"{Guid.NewGuid():N}_{safeName}";
        var filePath = Path.Combine(uploadsRoot, fileName);

        await using var stream = File.Create(filePath);
        await upload.CopyToAsync(stream);

        return $"/uploads/supplier-docs/{fileName}";
    }

    public async Task UpdateSupplierRenewalDueUtcAsync(Guid supplierId)
    {
        var supplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
        if (supplier == null)
        {
            return;
        }

        var earliestExpiry = await _db.SupplierComplianceDocuments
            .Where(d => d.SupplierId == supplierId && !d.IsDeleted && d.ExpiryDateUtc.HasValue)
            .Select(d => d.ExpiryDateUtc)
            .OrderBy(d => d)
            .FirstOrDefaultAsync();

        supplier.RenewalDueUtc = earliestExpiry;
        supplier.UpdatedAtUtc = DateTime.UtcNow;
    }

    public async Task<string?> GetSupplierApprovalBlockerAsync(Supplier supplier)
    {
        var now = DateTime.UtcNow;

        // If a Super Admin has already overridden this supplier's documents,
        // we allow the Admin to proceed with the Approval status.
        if (supplier.IsOverrideActive)
        {
            return null;
        }

        var hasDocuments = await _db.SupplierComplianceDocuments
            .AnyAsync(d => d.SupplierId == supplier.Id);
        if (!hasDocuments)
        {
            return $"Approval blocked: supplier '{supplier.Name}' has no compliance documents.";
        }

        var hasInvalidDocument = await _db.SupplierComplianceDocuments.AnyAsync(d =>
            d.SupplierId == supplier.Id &&
            (d.DocumentStatus == SupplierDocumentStatus.Missing ||
             d.DocumentStatus == SupplierDocumentStatus.PendingReview ||
             d.DocumentStatus == SupplierDocumentStatus.Expired ||
             (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value < now)));
        if (hasInvalidDocument)
        {
            return $"Approval blocked: supplier '{supplier.Name}' has missing/pending/expired compliance documents.";
        }

        var hasCriticalAlert = await _db.ComplianceAlerts.AnyAsync(a =>
            a.IsActive &&
            a.EntityType == "Supplier" &&
            a.EntityId == supplier.Id &&
            (a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High));
        if (hasCriticalAlert)
        {
            return $"Approval blocked: supplier '{supplier.Name}' has active high/critical compliance alerts.";
        }

        return null;
    }

    public async Task<int> UpsertRequiredDocumentAsync(Guid supplierId, string documentType, IFormFile? upload, DateTime? expiryDateUtc, string webRootPath, string? actorName)
    {
        var hasUpload = upload is { Length: > 0 };
        var hasExpiry = expiryDateUtc.HasValue;
        if (!hasUpload && !hasExpiry)
        {
            return 0;
        }

        var document = await _db.SupplierComplianceDocuments
            .Where(d => d.SupplierId == supplierId && d.DocumentType == documentType)
            .OrderByDescending(d => d.UpdatedAtUtc)
            .ThenByDescending(d => d.CreatedAtUtc)
            .FirstOrDefaultAsync();

        if (document == null)
        {
            document = new SupplierComplianceDocument
            {
                SupplierId = supplierId,
                DocumentType = documentType,
                DocumentStatus = SupplierDocumentStatus.Missing,
                Notes = "Upload required."
            };
            _db.SupplierComplianceDocuments.Add(document);
        }

        if (hasUpload)
        {
            document.FileUrl = await SaveDocumentFileAsync(upload, document.FileUrl, webRootPath);
            document.Notes = "Uploaded and ready for review.";
        }

        if (hasExpiry)
        {
            document.ExpiryDateUtc = expiryDateUtc;
        }

        document.DocumentStatus = ResolveDocumentStatus(document.FileUrl, document.ExpiryDateUtc);

        document.UpdatedAtUtc = DateTime.UtcNow;
        
        _db.AuditLogs.Add(new AuditLog
        {
            EntityName = nameof(SupplierComplianceDocument),
            EntityId = document.Id,
            Action = "Edit",
            Actor = actorName ?? "system",
            TimestampUtc = DateTime.UtcNow,
            Details = $"Updated document {documentType}."
        });

        return 1;
    }

    private SupplierDocumentStatus ResolveDocumentStatus(string? fileUrl, DateTime? expiryDateUtc)
    {
        if (string.IsNullOrWhiteSpace(fileUrl) || !fileUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
        {
            return SupplierDocumentStatus.Missing;
        }

        if (expiryDateUtc.HasValue && expiryDateUtc.Value.Date < DateTime.UtcNow.Date)
        {
            return SupplierDocumentStatus.Expired;
        }

        if (expiryDateUtc.HasValue && expiryDateUtc.Value.Date >= DateTime.UtcNow.Date)
        {
            return SupplierDocumentStatus.Verified;
        }

        return SupplierDocumentStatus.PendingReview;
    }
}
