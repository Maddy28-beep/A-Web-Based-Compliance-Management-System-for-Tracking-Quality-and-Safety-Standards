using System;
using System.Collections.Generic;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class Policy : BaseEntity
{
    public Guid PolicyCategoryId { get; set; }
    public PolicyCategory? PolicyCategory { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public DateTime EffectiveDateUtc { get; set; } = DateTime.UtcNow;
    public PolicyStatus Status { get; set; } = PolicyStatus.Draft;
    public string Owner { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<PolicyVersion> PolicyVersions { get; set; } = new List<PolicyVersion>();
}
