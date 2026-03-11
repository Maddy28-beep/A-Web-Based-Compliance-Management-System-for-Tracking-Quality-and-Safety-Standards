using System;
using System.Collections.Generic;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;

namespace FurniComply.Web.Models;

public class SupplierDocumentsIndexViewModel
{
    public Guid? SupplierId { get; set; }
    public SupplierDocumentStatus? Status { get; set; }
    public string? DocumentType { get; set; }
    public string? Search { get; set; }
    /// <summary>When true, shows only suppliers with documents expiring within 30 days or already expired.</summary>
    public bool Expiring { get; set; }
    public List<SupplierComplianceDocument> Documents { get; set; } = new();
    public List<Supplier> Suppliers { get; set; } = new();
    public List<string> DocumentTypes { get; set; } = new();

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalSupplierCount { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalSupplierCount / (double)PageSize) : 0;
}
