namespace FurniComply.Web.Services;

/// <summary>
/// Gmail and other providers issue app passwords as 16 characters without spaces; users often paste them with spaces.
/// </summary>
internal static class MailPasswordNormalizer
{
    internal static string? Normalize(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return password;

        return string.Concat(password.Where(c => !char.IsWhiteSpace(c)));
    }
}
