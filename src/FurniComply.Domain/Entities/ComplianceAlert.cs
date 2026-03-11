using System;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class ComplianceAlert : BaseEntity
{
    public Guid ComplianceAlertRuleId { get; set; }
    public ComplianceAlertRule? ComplianceAlertRule { get; set; }
    public ComplianceAlertSeverity Severity { get; set; } = ComplianceAlertSeverity.Warning;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAcknowledged { get; set; }
    public DateTime TriggeredAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAtUtc { get; set; }
}
