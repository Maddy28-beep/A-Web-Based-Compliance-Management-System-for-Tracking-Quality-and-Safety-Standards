using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class ComplianceAlertRule : BaseEntity
{
    public string RuleKey { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ThresholdValue { get; set; }
    public bool IsEnabled { get; set; } = true;
    public ComplianceAlertSeverity Severity { get; set; } = ComplianceAlertSeverity.Warning;
}
