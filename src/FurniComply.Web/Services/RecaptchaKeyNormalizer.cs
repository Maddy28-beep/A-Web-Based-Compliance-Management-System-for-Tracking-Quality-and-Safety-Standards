namespace FurniComply.Web.Services;

public static class RecaptchaKeyNormalizer
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var s = value.Trim().Trim('"', '\'');
        if (s.Length == 0)
            return null;

        if (s.Equals("YOUR_RECAPTCHA_SITE_KEY", StringComparison.OrdinalIgnoreCase) ||
            s.Equals("YOUR_RECAPTCHA_SECRET_KEY", StringComparison.OrdinalIgnoreCase))
            return null;

        return s;
    }
}
