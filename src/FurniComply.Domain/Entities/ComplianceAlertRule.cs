using System.ComponentModel.DataAnnotations;
using FurniComply.Domain.Enums;

namespace FurniComply.Domain.Entities;

public class ComplianceAlertRule : BaseEntity
{
    [Required(ErrorMessage = "Rule key is required.")]
    [StringLength(100, ErrorMessage = "Rule key cannot exceed 100 characters.")]
    public string RuleKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;
    public int ThresholdValue { get; set; }
    public bool IsEnabled { get; set; } = true;
    public ComplianceAlertSeverity Severity { get; set; } = ComplianceAlertSeverity.Warning;
}
