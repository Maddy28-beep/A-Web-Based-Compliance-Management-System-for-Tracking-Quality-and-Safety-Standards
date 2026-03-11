namespace FurniComply.Web.Services;

/// <summary>
/// Sends notifications when CAPA tasks are assigned. Email is sent only when SMTP is configured in appsettings.
/// </summary>
public interface IAssignmentNotificationService
{
    Task NotifyAssigneeAsync(string assigneeEmail, string assigneeName, string capaTitle, string dueDate, string detailsUrl);
}
