using System.Text.RegularExpressions;
using FurniComply.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace FurniComply.Web.Services;

public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
{
    private static readonly string[] WeakFragments =
    {
        "password",
        "admin",
        "qwerty",
        "123456",
        "abcdef"
    };

    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
    {
        var errors = new List<IdentityError>();
        var candidate = password ?? string.Empty;

        if (ContainsUserIdentifiers(user, candidate))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordContainsUserData",
                Description = "Password must not contain your email or name."
            });
        }

        if (WeakFragments.Any(fragment => candidate.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordTooCommon",
                Description = "Password contains a common weak pattern. Choose a less predictable password."
            });
        }

        if (Regex.IsMatch(candidate, "(.)\\1\\1\\1"))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordRepeatedCharacters",
                Description = "Password must not contain four identical characters in a row."
            });
        }

        return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
    }

    private static bool ContainsUserIdentifiers(ApplicationUser user, string password)
    {
        var identifiers = new List<string?>
        {
            user.Email,
            user.UserName,
            user.FullName
        };

        foreach (var identifier in identifiers)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                continue;
            }

            foreach (var token in Tokenize(identifier))
            {
                if (token.Length >= 4 && password.Contains(token, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static IEnumerable<string> Tokenize(string value)
    {
        return value
            .Split(new[] { ' ', '.', '_', '-', '@' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }
}
