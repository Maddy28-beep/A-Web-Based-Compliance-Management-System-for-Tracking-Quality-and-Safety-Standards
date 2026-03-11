using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FurniComply.Domain.Entities;
using FurniComply.Application.Interfaces;

namespace FurniComply.Web.Services;

public interface IProcurementService
{
    Task<OrderApprovalGateResult> GetOrderApprovalBlockerAsync(Guid orderId, string operation, string? actorName);
    Task<OrderApprovalGateResult> GetOrderApprovalBlockerAsync(ProcurementOrder order, string operation, string? actorName);
    Task<Guid?> GetProcurementStatusIdAsync(string statusName);
    Task<string> GenerateNextOrderNumberAsync();
}

public sealed record OrderApprovalGateResult(
    bool IsBlocked,
    string Message,
    bool IsScoreBased,
    IReadOnlyList<string> DocumentBlockers,
    IReadOnlyList<string> RiskBlockers,
    bool OverrideUsed,
    IReadOnlyList<string> OverriddenDocumentBlockers,
    string? OverrideReason,
    string? OverrideApprovedBy)
{
    public static OrderApprovalGateResult Allowed() =>
        new(false, string.Empty, false, Array.Empty<string>(), Array.Empty<string>(), false, Array.Empty<string>(), null, null);

    public static OrderApprovalGateResult Blocked(
        string message,
        bool isScoreBased = false,
        IReadOnlyList<string>? documentBlockers = null,
        IReadOnlyList<string>? riskBlockers = null) =>
        new(true, message, isScoreBased, documentBlockers ?? Array.Empty<string>(), riskBlockers ?? Array.Empty<string>(),
            false, Array.Empty<string>(), null, null);

    public static OrderApprovalGateResult AllowedWithDocumentOverride(
        IReadOnlyList<string> overriddenDocumentBlockers,
        string overrideReason,
        string? overrideApprovedBy) =>
        new(false, string.Empty, false, overriddenDocumentBlockers, Array.Empty<string>(),
            true, overriddenDocumentBlockers, overrideReason, overrideApprovedBy);
}

public interface ISupplierManagementService
{
    Task<List<SupplierTimelineItem>> BuildSupplierTimelineAsync(Guid supplierId, string supplierName, Func<string, string, object, string?> urlHelper);
    Task<string?> SaveDocumentFileAsync(IFormFile? upload, string? existingUrl, string webRootPath);
    Task UpdateSupplierRenewalDueUtcAsync(Guid supplierId);
    Task<string?> GetSupplierApprovalBlockerAsync(Supplier supplier);
    Task<int> UpsertRequiredDocumentAsync(Guid supplierId, string documentType, IFormFile? upload, DateTime? expiryDateUtc, string webRootPath, string? actorName);
}

public class SupplierTimelineItem
{
    public DateTime DateUtc { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? SourceId { get; set; }
    public string? SourceRoute { get; set; }
    public string? Actor { get; set; }
}
