using System;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class SupplierComplianceDocument : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public SupplierDocumentStatus DocumentStatus { get; set; } = SupplierDocumentStatus.Missing;
    public DateTime? ExpiryDateUtc { get; set; }
    public string? FileUrl { get; set; }
    public string Notes { get; set; } = string.Empty;
}
