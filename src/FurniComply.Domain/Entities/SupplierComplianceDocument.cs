using System;
using System.ComponentModel.DataAnnotations;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class SupplierComplianceDocument : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    [Required(ErrorMessage = "Document type is required.")]
    [StringLength(120, ErrorMessage = "Document type cannot exceed 120 characters.")]
    public string DocumentType { get; set; } = string.Empty;
    public SupplierDocumentStatus DocumentStatus { get; set; } = SupplierDocumentStatus.Missing;
    public DateTime? ExpiryDateUtc { get; set; }
    [StringLength(400)]
    public string? FileUrl { get; set; }

    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string Notes { get; set; } = string.Empty;
}
