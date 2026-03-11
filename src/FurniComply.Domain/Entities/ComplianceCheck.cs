using System.ComponentModel.DataAnnotations;

namespace FurniComply.Domain.Entities;

public class ComplianceCheck : BaseEntity
{
    [Required]
    public Guid PolicyId { get; set; }
    public Policy? Policy { get; set; }
    [Required]
    public Guid ComplianceStatusId { get; set; }
    public ComplianceStatusMaster? ComplianceStatus { get; set; }
    public Guid ComplianceCategoryId { get; set; }
    public ComplianceCategory? ComplianceCategory { get; set; }
    public Guid RiskLevelId { get; set; }
    public RiskLevel? RiskLevel { get; set; }
    public DateTime CheckedAtUtc { get; set; } = DateTime.UtcNow;
    public string Notes { get; set; } = string.Empty;
}
