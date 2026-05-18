using FurniComply.Infrastructure.Identity;

namespace FurniComply.Web.Services;

public interface IPasswordHistoryService
{
    Task<bool> IsPasswordReuseAsync(ApplicationUser user, string candidatePassword);
    Task RememberPreviousPasswordAsync(ApplicationUser user, string? previousPasswordHash);
}
