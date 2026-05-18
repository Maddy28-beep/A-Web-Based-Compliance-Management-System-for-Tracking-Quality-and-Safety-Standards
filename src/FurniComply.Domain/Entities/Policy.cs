using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class Policy : BaseEntity
{
    public Guid PolicyCategoryId { get; set; }
    public PolicyCategory? PolicyCategory { get; set; }

    [Required(ErrorMessage = "Policy code is required.")]
    [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Policy title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(20)]
    public string Version { get; set; } = "1.0";
    public DateTime EffectiveDateUtc { get; set; } = DateTime.UtcNow;
    public PolicyStatus Status { get; set; } = PolicyStatus.Draft;

    [StringLength(100, ErrorMessage = "Owner cannot exceed 100 characters.")]
    public string Owner { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    public ICollection<PolicyVersion> PolicyVersions { get; set; } = new List<PolicyVersion>();
}
