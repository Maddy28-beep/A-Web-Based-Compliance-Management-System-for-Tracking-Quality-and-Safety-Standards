using System;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class PolicyVersion : BaseEntity
{
    public Guid PolicyId { get; set; }
    public Policy? Policy { get; set; }
    public string VersionNumber { get; set; } = string.Empty;
    public PolicyStatus Status { get; set; } = PolicyStatus.Draft;
    public DateTime EffectiveDateUtc { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
