using Microsoft.AspNetCore.Identity;

namespace FurniComply.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
