using System.Text.Json;
using System.Text.Json.Serialization;

namespace FurniComply.Web.Services;

public interface IReCaptchaService
{
    Task<bool> VerifyAsync(string? responseToken, string? remoteIp = null);
    bool IsEnabled { get; }
    string? SiteKey { get; }
    string ScriptUrl { get; }
}

public class ReCaptchaService : IReCaptchaService
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ReCaptchaService> _logger;

    public ReCaptchaService(
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        ILogger<ReCaptchaService> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private string? SiteKeyValue => RecaptchaKeyNormalizer.Normalize(_config["Recaptcha:SiteKey"]);
    private string? SecretKeyValue => RecaptchaKeyNormalizer.Normalize(_config["Recaptcha:SecretKey"]);

    public bool IsEnabled
    {
        get
        {
            if (_config.GetValue("Recaptcha:Disable", false))
                return false;

            var site = SiteKeyValue;
            var secret = SecretKeyValue;
            return !string.IsNullOrWhiteSpace(site) &&
                   !string.IsNullOrWhiteSpace(secret) &&
                   site.Length >= 25 &&
                   secret.Length >= 25;
        }
    }

    public string? SiteKey => IsEnabled ? SiteKeyValue : null;

    public string ScriptUrl => "https://www.google.com/recaptcha/api.js";

    public async Task<bool> VerifyAsync(string? responseToken, string? remoteIp = null)
    {
        if (!IsEnabled)
            return true;

        if (string.IsNullOrWhiteSpace(responseToken))
        {
            _logger.LogWarning("reCAPTCHA response token was missing on form post.");
            return false;
        }

        var secret = SecretKeyValue;
        if (string.IsNullOrWhiteSpace(secret))
            return true;

        try
        {
            var client = _httpClientFactory.CreateClient();
            var fields = new List<KeyValuePair<string, string>>
            {
                new("secret", secret),
                new("response", responseToken)
            };

            if (!string.IsNullOrWhiteSpace(remoteIp))
                fields.Add(new KeyValuePair<string, string>("remoteip", remoteIp));

            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(fields));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReCaptchaVerifyResponse>(json);
            if (result?.Success == true)
                return true;

            _logger.LogWarning(
                "reCAPTCHA siteverify failed. Error codes: {ErrorCodes}",
                result?.ErrorCodes is { Count: > 0 } codes ? string.Join(", ", codes) : "(none)");

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "reCAPTCHA siteverify request failed.");
            return false;
        }
    }

    private sealed class ReCaptchaVerifyResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public List<string>? ErrorCodes { get; set; }
    }
}
