using FurniComply.Domain.Enums;

namespace FurniComply.Web.ViewModels.Capa;

public class CapaDashboardViewModel
{
    public required string SelectedFilter { get; init; }
    public string? SelectedAssignee { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public bool IncludeUnassignedOption { get; init; }
    public required IReadOnlyList<string> AssigneeOptions { get; init; }
    public int OpenCount { get; init; }
    public int OverdueCount { get; init; }
    public int EvidenceSubmittedCount { get; init; }
    public int ClosedCount { get; init; }
    public required IReadOnlyList<CapaDashboardRowViewModel> Items { get; init; }
}

public class CapaDashboardRowViewModel
{
    public Guid Id { get; init; }
    public Guid? ComplianceCheckId { get; init; }
    public Guid? SupplierId { get; init; }
    public CorrectiveActionType IssueType { get; init; }
    public string Title { get; init; } = string.Empty;
    public string PolicyTitle { get; init; } = "-";
    public string SupplierName { get; init; } = "-";
    public string CheckStatus { get; init; } = "-";
    public CorrectiveActionStatus CapaStatus { get; init; }
    public string AssignedTo { get; init; } = "-";
    public string AssignedToFullName { get; init; } = "-";
    public DateTime DueAtUtc { get; init; }
    public DateTime? EvidenceSubmittedAtUtc { get; init; }
    public DateTime? ClosedAtUtc { get; init; }
}
