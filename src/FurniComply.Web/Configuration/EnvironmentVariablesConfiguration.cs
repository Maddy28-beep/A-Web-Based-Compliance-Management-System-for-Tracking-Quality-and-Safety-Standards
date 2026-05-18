using FurniComply.Web.Services;

namespace FurniComply.Web.Configuration;

internal static class EnvironmentVariablesConfiguration
{
    internal static void Apply(ConfigurationManager config)
    {
        ApplyRecaptcha(config);
        ApplyMappings(config);
    }

    private static void ApplyRecaptcha(ConfigurationManager config)
    {
        var site = RecaptchaKeyNormalizer.Normalize(Environment.GetEnvironmentVariable("RECAPTCHA_SITE_KEY"));
        var secret = RecaptchaKeyNormalizer.Normalize(Environment.GetEnvironmentVariable("RECAPTCHA_SECRET_KEY"));
        if (!string.IsNullOrWhiteSpace(site))
            config["Recaptcha:SiteKey"] = site;
        if (!string.IsNullOrWhiteSpace(secret))
            config["Recaptcha:SecretKey"] = secret;
    }

    private static void ApplyMappings(ConfigurationManager config)
    {
        var mappings = new (string EnvName, string ConfigKey)[]
        {
            ("DB_CONNECTION_STRING", "ConnectionStrings:DefaultConnection"),
            ("ALLOWED_HOSTS", "AllowedHosts"),
            ("APP_PUBLIC_ORIGIN", "App:PublicOrigin"),
            ("APP_IDLE_LOGOUT_SECONDS", "App:IdleLogoutSeconds"),
            ("OPENWEATHER_API_KEY", "ExternalApis:OpenWeatherApiKey"),
            ("MAIL_SMTP_HOST", "Mail:SmtpHost"),
            ("MAIL_SMTP_PORT", "Mail:SmtpPort"),
            ("MAIL_FROM", "Mail:From"),
            ("MAIL_USERNAME", "Mail:UserName"),
            ("MAIL_PASSWORD", "Mail:Password"),
            ("MAIL_USE_SSL", "Mail:UseSsl"),
            ("RECAPTCHA_DISABLE", "Recaptcha:Disable"),
            ("SEED_IDENTITY_ON_STARTUP", "SeedIdentityOnStartup"),
            ("SEED_DATA_ON_STARTUP", "SeedDataOnStartup"),
            ("SEED_LIVE_SCENARIOS_ON_STARTUP", "SeedLiveScenariosOnStartup"),
            ("APP_ENCRYPTION_KEY", "Encryption:MasterKey"),
            ("BACKUP_SUPERADMIN_PASSWORD", "BackupSuperAdmin:Password")
        };

        foreach (var (envName, configKey) in mappings)
            ApplyIfPresent(config, envName, configKey);
    }

    private static void ApplyIfPresent(ConfigurationManager config, string envName, string configKey)
    {
        var value = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(value))
            return;

        if (string.Equals(configKey, "Mail:Password", StringComparison.OrdinalIgnoreCase))
            value = MailPasswordNormalizer.Normalize(value);

        config[configKey] = value;
    }
}
