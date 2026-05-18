using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FurniComply.Web.Services;

public interface IPasswordResetEmailSender
{
    /// <summary>Returns true if SMTP is configured and the message was sent (or queued successfully).</summary>
    Task<bool> TrySendResetLinkAsync(string toEmail, string? recipientName, string resetLink);

    /// <summary>Sends a short test message to verify SMTP credentials (same Mail:* settings as password reset).</summary>
    Task<bool> TrySendTestMessageAsync(string toEmail);
}

public class PasswordResetEmailSender : IPasswordResetEmailSender
{
    private readonly IConfiguration _config;
    private readonly ILogger<PasswordResetEmailSender> _logger;
    private readonly IWebHostEnvironment _env;

    public PasswordResetEmailSender(
        IConfiguration config,
        ILogger<PasswordResetEmailSender> logger,
        IWebHostEnvironment env)
    {
        _config = config;
        _logger = logger;
        _env = env;
    }

    public Task<bool> TrySendTestMessageAsync(string toEmail) =>
        TrySendMailAsync(
            toEmail,
            "FurniComply SMTP test",
            $@"Hello,

This is a test message from your local FurniComply app. If you received this email, password-reset mail should work too.

- FurniComply".Trim(),
            resetLinkForDevCopy: null,
            devCopyNote: "SMTP test message.");

    public async Task<bool> TrySendResetLinkAsync(string toEmail, string? recipientName, string resetLink)
    {
        if (string.IsNullOrWhiteSpace(_config["Mail:SmtpHost"]))
        {
            _logger.LogWarning("Mail:SmtpHost not configured; password reset email was not sent to {Email}.", toEmail);
            await WriteDevelopmentFallbackAsync(toEmail, resetLink, "Mail:SmtpHost is empty.");
            return false;
        }

        await WriteDevelopmentCopyAsync(toEmail, resetLink, "Generated before SMTP send.");

        var greet = string.IsNullOrWhiteSpace(recipientName) ? "Hello" : $"Hello {recipientName}";
        var subject = "Reset your FurniComply password";
        var body = $@"{greet},

We received a request to reset the password for your FurniComply account.

Open this link (or paste it into your browser) to choose a new password. This link will expire after a short time for security reasons:

{resetLink}

If you did not request a password reset, you can ignore this email.

- FurniComply".Trim();

        var sent = await TrySendMailAsync(toEmail, subject, body, resetLink, "Password reset email.");
        if (!sent)
            await WriteDevelopmentFallbackAsync(toEmail, resetLink, "SMTP send failed. See application logs for details.");
        return sent;
    }

    private async Task<bool> TrySendMailAsync(
        string toEmail,
        string subject,
        string body,
        string? resetLinkForDevCopy,
        string devCopyNote)
    {
        var host = _config["Mail:SmtpHost"];
        if (string.IsNullOrWhiteSpace(host))
            return false;

        var port = _config.GetValue<int>("Mail:SmtpPort", 587);
        var from = _config["Mail:From"] ?? "noreply@furnicomply.local";
        var user = _config["Mail:UserName"];
        var pass = MailPasswordNormalizer.Normalize(_config["Mail:Password"]);
        var useSsl = _config.GetValue<bool>("Mail:UseSsl", true);

        try
        {
            using var client = new SmtpClient(host, port)
            {
                EnableSsl = useSsl,
                Credentials = !string.IsNullOrWhiteSpace(user)
                    ? new NetworkCredential(user, pass)
                    : null
            };

            var mail = new MailMessage(from, toEmail, subject, body);
            await client.SendMailAsync(mail);
            _logger.LogInformation("Email sent to {Email} ({Subject})", toEmail, subject);
            if (!string.IsNullOrWhiteSpace(resetLinkForDevCopy))
                await WriteDevelopmentCopyAsync(toEmail, resetLinkForDevCopy, "SMTP send reported success.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to send email to {Email}. SMTP: {Host}:{Port}. Message: {SmtpMessage}",
                toEmail,
                host,
                port,
                ex.GetBaseException().Message);

            if (await TrySendWithPythonFallbackAsync(host, port, from, user, pass, useSsl, toEmail, subject, body))
            {
                if (!string.IsNullOrWhiteSpace(resetLinkForDevCopy))
                    await WriteDevelopmentCopyAsync(toEmail, resetLinkForDevCopy, "Python SMTP fallback reported success.");
                return true;
            }

            return false;
        }
    }

    private async Task<bool> TrySendWithPythonFallbackAsync(
        string host,
        int port,
        string from,
        string? user,
        string? pass,
        bool useSsl,
        string toEmail,
        string subject,
        string body)
    {
        try
        {
            var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "smtp_fallback.py");
            if (!File.Exists(scriptPath))
            {
                _logger.LogWarning("Python SMTP fallback script was not found at {Path}", scriptPath);
                return false;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                WorkingDirectory = AppContext.BaseDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.ArgumentList.Add(scriptPath);
            startInfo.Environment["SMTP_HOST"] = host;
            startInfo.Environment["SMTP_PORT"] = port.ToString();
            startInfo.Environment["SMTP_FROM"] = from;
            startInfo.Environment["SMTP_USERNAME"] = user ?? string.Empty;
            startInfo.Environment["SMTP_PASSWORD"] = pass ?? string.Empty;
            startInfo.Environment["SMTP_USE_SSL"] = useSsl ? "true" : "false";
            startInfo.Environment["SMTP_TO"] = toEmail;
            startInfo.Environment["SMTP_SUBJECT"] = subject;
            startInfo.Environment["SMTP_BODY"] = body;
            startInfo.Environment["PYTHONIOENCODING"] = "utf-8";

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                return false;
            }

            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            var stdout = (await stdoutTask).Trim();
            var stderr = (await stderrTask).Trim();

            if (process.ExitCode == 0)
            {
                _logger.LogInformation(
                    "Password reset email sent to {Email} via Python SMTP fallback. Output: {Output}",
                    toEmail,
                    string.IsNullOrWhiteSpace(stdout) ? "(none)" : stdout);
                return true;
            }

            _logger.LogWarning(
                "Python SMTP fallback failed for {Email}. Exit code {ExitCode}. Stdout: {Stdout}. Stderr: {Stderr}",
                toEmail,
                process.ExitCode,
                stdout,
                stderr);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Python SMTP fallback could not send a password reset email to {Email}", toEmail);
            return false;
        }
    }

    private async Task WriteDevelopmentCopyAsync(string toEmail, string resetLink, string note)
    {
        try
        {
            var appData = Path.Combine(_env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(appData);
            var path = Path.Combine(appData, "last-password-reset-link.txt");
            var sb = new StringBuilder();
            sb.AppendLine($"Generated (UTC): {DateTime.UtcNow:O}");
            sb.AppendLine($"Email: {toEmail}");
            sb.AppendLine($"Note: {note}");
            sb.AppendLine();
            sb.AppendLine("Paste this URL into your browser to set a new password:");
            sb.AppendLine(resetLink);
            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
            _logger.LogInformation("Password reset link copy written to {Path}.", path);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Could not write development password-reset link copy.");
        }
    }

    /// <summary>
    /// When email cannot be sent, save the link on disk and log it so you can test without working SMTP.
    /// </summary>
    private async Task WriteDevelopmentFallbackAsync(string toEmail, string resetLink, string reason)
    {
        _logger.LogWarning(
            "Password reset email was not sent ({Reason}). Recovery link logged for operators.",
            reason);

        try
        {
            var appData = Path.Combine(_env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(appData);
            var path = Path.Combine(appData, "last-password-reset-link.txt");
            var sb = new StringBuilder();
            sb.AppendLine($"Generated (UTC): {DateTime.UtcNow:O}");
            sb.AppendLine($"Email: {toEmail}");
            sb.AppendLine($"Reason email was not sent: {reason}");
            sb.AppendLine();
            sb.AppendLine("Paste this URL into your browser to set a new password:");
            sb.AppendLine(resetLink);
            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
            _logger.LogWarning("Same link written to {Path}.", path);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Could not write dev password-reset fallback file.");
        }
    }
}
