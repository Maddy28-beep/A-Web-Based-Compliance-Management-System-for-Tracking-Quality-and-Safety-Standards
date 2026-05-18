using Microsoft.AspNetCore.Identity;

namespace FurniComply.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsBackupAccount { get; set; }
    public bool IsHidden { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
