using System.ComponentModel.DataAnnotations;

namespace FurniComply.Domain.Entities;

public class AccessRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Reason { get; set; }

    [StringLength(100)]
    public string? PreferredRole { get; set; }

    public AccessRequestStatus Status { get; set; } = AccessRequestStatus.Pending;

    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }
    [StringLength(500)]
    public string? RejectionReason { get; set; }
}

public enum AccessRequestStatus
{
    Pending,
    Approved,
    Rejected
}
