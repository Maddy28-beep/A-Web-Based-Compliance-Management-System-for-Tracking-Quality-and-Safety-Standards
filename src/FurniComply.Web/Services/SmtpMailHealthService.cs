using System.Net.Sockets;
using FurniComply.Web;
using Microsoft.Extensions.Logging;

namespace FurniComply.Web.Services;

public sealed class MailDiagnosticsViewModel
{
    public bool SmtpHostConfigured { get; init; }
    public string? SmtpHost { get; init; }
    public int SmtpPort { get; init; }
    public string? From { get; init; }
    public string? UserName { get; init; }
    public bool PasswordPresent { get; init; }
    public int PasswordLength { get; init; }
    public bool UseSsl { get; init; }
    public bool ConfigurationLooksComplete { get; init; }
    public bool? TcpConnectSucceeded { get; init; }
    public string? TcpConnectDetail { get; init; }
    public IReadOnlyList<string> Notes { get; init; } = Array.Empty<string>();
}

/// <summary>
/// Reads <c>Mail:*</c> from configuration and optionally probes SMTP host reachability (does not send mail or test credentials).
/// </summary>
public sealed class SmtpMailHealthService
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmtpMailHealthService> _logger;

    public SmtpMailHealthService(IConfiguration config, ILogger<SmtpMailHealthService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<MailDiagnosticsViewModel> GetDiagnosticsAsync(CancellationToken cancellationToken = default)
    {
        var host = _config["Mail:SmtpHost"];
        var port = _config.GetValue<int>("Mail:SmtpPort", 587);
        var from = _config["Mail:From"];
        var user = _config["Mail:UserName"];
        var pass = _config["Mail:Password"];
        var useSsl = _config.GetValue<bool>("Mail:UseSsl", true);

        var hostOk = !string.IsNullOrWhiteSpace(host);
        var userOk = !string.IsNullOrWhiteSpace(user);
        var passOk = !string.IsNullOrWhiteSpace(pass);
        var complete = hostOk && userOk && passOk && !string.IsNullOrWhiteSpace(from);

        bool? tcpOk = null;
        string? tcpDetail = null;
        if (hostOk && port > 0 && port <= 65535)
        {
            try
            {
                using var tcp = new TcpClient();
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                linked.CancelAfter(TimeSpan.FromSeconds(5));
                await tcp.ConnectAsync(host!, port, linked.Token);
                tcpOk = true;
                tcpDetail = $"Connected to {host}:{port} (TCP only — does not verify SMTP login).";
            }
            catch (Exception ex)
            {
                tcpOk = false;
                _logger.LogWarning(ex, "SMTP TCP probe failed for {Host}:{Port}", host, port);
                tcpDetail = SafeErrorMessages.MailTcpProbeFailed;
            }
        }
        else
        {
            tcpDetail = "Skipped (host or port invalid).";
        }

        var notes = new List<string>
        {
            "Password reset uses the same Mail: settings as CAPA notifications (System.Net.Mail.SmtpClient).",
            "Microsoft 365 / Outlook often requires “SMTP AUTH” enabled for the mailbox. Your IT admin may need to turn it on.",
            "If TCP connects but mail never arrives, check junk folder and SMTP login. Gmail requires an App Password (16 characters, no spaces) when 2-Step Verification is on.",
            "If outbound mail fails, check Mail:* credentials and firewall rules for SMTP port 587.",
        };

        return new MailDiagnosticsViewModel
        {
            SmtpHostConfigured = hostOk,
            SmtpHost = host,
            SmtpPort = port,
            From = from,
            UserName = user,
            PasswordPresent = passOk,
            PasswordLength = pass?.Length ?? 0,
            UseSsl = useSsl,
            ConfigurationLooksComplete = complete,
            TcpConnectSucceeded = tcpOk,
            TcpConnectDetail = tcpDetail,
            Notes = notes,
        };
    }
}
