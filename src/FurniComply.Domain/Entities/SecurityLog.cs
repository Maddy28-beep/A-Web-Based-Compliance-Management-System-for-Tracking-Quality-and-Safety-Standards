using System;

namespace FurniComply.Domain.Entities;

public class SecurityLog : BaseEntity
{
    public string Category { get; set; } = "Authentication";
    public string Action { get; set; } = string.Empty;
    public string Actor { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = string.Empty;
}
