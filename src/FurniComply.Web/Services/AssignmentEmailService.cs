using System.Net;
using System.Net.Mail;

namespace FurniComply.Web.Services;

/// <summary>
/// Sends email notifications when CAPA tasks are assigned. Only sends when Mail:SmtpHost is configured in appsettings.
/// </summary>
public class AssignmentEmailService : IAssignmentNotificationService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AssignmentEmailService> _logger;

    public AssignmentEmailService(IConfiguration config, ILogger<AssignmentEmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task NotifyAssigneeAsync(string assigneeEmail, string assigneeName, string capaTitle, string dueDate, string detailsUrl)
    {
        var host = _config["Mail:SmtpHost"];
        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogDebug("Mail:SmtpHost not configured; skipping assignment email.");
            return;
        }

        try
        {
            var port = _config.GetValue<int>("Mail:SmtpPort", 587);
            var from = _config["Mail:From"] ?? "noreply@furnicomply.local";
            var user = _config["Mail:UserName"];
            var pass = _config["Mail:Password"];
            var useSsl = _config.GetValue<bool>("Mail:UseSsl", true);

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = useSsl,
                Credentials = !string.IsNullOrWhiteSpace(user)
                    ? new NetworkCredential(user, pass)
                    : null
            };

            var subject = $"CAPA Assignment – FurniComply Compliance Management";
            var body = $@"Dear {assigneeName},

You have been assigned a Corrective and Preventive Action (CAPA) task through the FurniComply Compliance Management Platform.

Task Title: {capaTitle}
Due Date: {dueDate}

Please review the task details and complete the required actions by the due date. Your timely response helps maintain our compliance standards and supports our supply chain quality objectives.

To view full details and submit your response, please follow this link:
{detailsUrl}

If you have any questions regarding this assignment, please contact your Compliance Manager.

Best regards,
FurniComply Compliance Management Platform".Trim();

            var mail = new MailMessage(from, assigneeEmail, subject, body);
            await client.SendMailAsync(mail);
            _logger.LogInformation("Assignment email sent to {Email}", assigneeEmail);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send assignment email to {Email}; assignment still saved.", assigneeEmail);
        }
    }
}
