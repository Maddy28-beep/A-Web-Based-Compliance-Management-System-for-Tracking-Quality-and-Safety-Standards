namespace FurniComply.Web;

/// <summary>
/// URLs allowed for unauthenticated users when matching the path (fallback if endpoint metadata is not available yet).
/// </summary>
internal static class PublicAnonymousPathHelper
{
    public static bool MatchesPublicPathPrefix(string pathNorm)
    {
        if (string.IsNullOrEmpty(pathNorm))
        {
            return false;
        }

        string[] publicPrefixes =
        [
            "/Home/Index",
            "/Home/Login",
            "/Account/Login",
            "/Account/TwoFactorChallenge",
            "/Account/ForgotPassword",
            "/Account/ForgotPasswordConfirmation",
            "/Account/ResetPassword",
            "/Account/ResetPasswordConfirmation",
            "/Home/RequestAccess",
            "/Home/Privacy",
            "/Home/Error",
            "/css",
            "/js",
            "/images",
            "/lib",
            "/favicon.ico",
        ];
        foreach (var prefix in publicPrefixes)
        {
            if (pathNorm.Equals(prefix, StringComparison.OrdinalIgnoreCase) ||
                pathNorm.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
