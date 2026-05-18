using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Services;

public class PasswordHistoryService : IPasswordHistoryService
{
    private const string LoginProvider = "PasswordHistory";
    private const int HistoryLimit = 5;

    private readonly AppDbContext _db;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public PasswordHistoryService(AppDbContext db, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> IsPasswordReuseAsync(ApplicationUser user, string candidatePassword)
    {
        if (!string.IsNullOrWhiteSpace(user.PasswordHash) &&
            PasswordMatches(user, user.PasswordHash, candidatePassword))
        {
            return true;
        }

        var priorHashes = await _db.Set<IdentityUserToken<string>>()
            .AsNoTracking()
            .Where(token => token.UserId == user.Id && token.LoginProvider == LoginProvider)
            .OrderByDescending(token => token.Name)
            .Select(token => token.Value)
            .ToListAsync();

        return priorHashes.Any(hash => !string.IsNullOrWhiteSpace(hash) && PasswordMatches(user, hash!, candidatePassword));
    }

    public async Task RememberPreviousPasswordAsync(ApplicationUser user, string? previousPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(previousPasswordHash))
        {
            return;
        }

        var alreadyStored = await _db.Set<IdentityUserToken<string>>()
            .AnyAsync(token =>
                token.UserId == user.Id &&
                token.LoginProvider == LoginProvider &&
                token.Value == previousPasswordHash);

        if (!alreadyStored)
        {
            _db.Set<IdentityUserToken<string>>().Add(new IdentityUserToken<string>
            {
                UserId = user.Id,
                LoginProvider = LoginProvider,
                Name = $"{DateTime.UtcNow.Ticks:D20}",
                Value = previousPasswordHash
            });
        }

        var existingTokens = await _db.Set<IdentityUserToken<string>>()
            .Where(token => token.UserId == user.Id && token.LoginProvider == LoginProvider)
            .OrderByDescending(token => token.Name)
            .ToListAsync();

        if (existingTokens.Count > HistoryLimit)
        {
            _db.Set<IdentityUserToken<string>>().RemoveRange(existingTokens.Skip(HistoryLimit));
        }

        await _db.SaveChangesAsync();
    }

    private bool PasswordMatches(ApplicationUser user, string hash, string candidatePassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hash, candidatePassword);
        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
