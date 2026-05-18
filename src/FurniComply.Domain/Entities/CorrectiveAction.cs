using System;
using System.ComponentModel.DataAnnotations;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class CorrectiveAction : BaseEntity
{
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public Guid? ComplianceCheckId { get; set; }
    public ComplianceCheck? ComplianceCheck { get; set; }

    public CorrectiveActionType IssueType { get; set; } = CorrectiveActionType.Compliance;

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(220, ErrorMessage = "Title cannot exceed 220 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; set; } = string.Empty;
    [StringLength(2000)]
    public string? CorrectiveActionPlan { get; set; }

    [StringLength(2000)]
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
