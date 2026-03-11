using System;
using System.Collections.Generic;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class Supplier : BaseEntity
{
    public Guid SupplierCategoryId { get; set; }
    public SupplierCategory? SupplierCategory { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public SupplierStatus Status { get; set; } = SupplierStatus.Active;
    public decimal Rating { get; set; }
    public string Certifications { get; set; } = string.Empty;
    public SupplierApprovalStatus ApprovalStatus { get; set; } = SupplierApprovalStatus.Pending;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedOnUtc { get; set; }
    public DateTime? RenewalDueUtc { get; set; }
    public DateTime? LastReviewUtc { get; set; }
    
    public Guid? RiskLevelId { get; set; }
    public RiskLevel? RiskLevel { get; set; }
    
    public decimal PerformanceScore { get; set; }
    
    public bool IsOverrideActive { get; set; }
    public string? OverrideReason { get; set; }
    public string? OverriddenBy { get; set; }
    
    // Override Request Logic
    public bool OverrideRequestActive { get; set; }
    public string? OverrideRequestReason { get; set; }
    public string? RequestedBy { get; set; }
    public DateTime? RequestedAtUtc { get; set; }

    public ICollection<SupplierComplianceDocument> ComplianceDocuments { get; set; } = new List<SupplierComplianceDocument>();
    public ICollection<SupplierPerformance> Performances { get; set; } = new List<SupplierPerformance>();
    public ICollection<SupplierEvaluation> Evaluations { get; set; } = new List<SupplierEvaluation>();
}
