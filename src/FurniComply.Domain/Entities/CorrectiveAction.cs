using System;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class CorrectiveAction : BaseEntity
{
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    
    public Guid? ComplianceCheckId { get; set; }
    public ComplianceCheck? ComplianceCheck { get; set; }
    
    public CorrectiveActionType IssueType { get; set; } = CorrectiveActionType.Compliance;
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CorrectiveActionPlan { get; set; }
    public string? PreventiveActionPlan { get; set; }
    
    public CorrectiveActionStatus Status { get; set; } = CorrectiveActionStatus.Open;
    public string? AssignedTo { get; set; }
    public DateTime? AssignedAtUtc { get; set; }
    public DateTime DueAtUtc { get; set; }
    public DateTime? EvidenceSubmittedAtUtc { get; set; }
    public string? EvidenceNotes { get; set; }
    public DateTime? ClosedAtUtc { get; set; }
    public string? ClosedBy { get; set; }
}
