namespace FurniComply.Web;

internal static class SafeErrorMessages
{
    public const string Generic =
        "An unexpected error occurred. Please try again or contact an administrator.";

    public const string PdfUnavailable =
        "PDF generation is temporarily unavailable.";

    public const string QrGenerationFailed =
        "Could not generate the QR code. Please try again later.";

    public const string MailTcpProbeFailed =
        "Could not establish a TCP connection to the configured mail host.";

    public static string ForUser(Exception exception, string? fallback = null)
    {
        if (exception is InvalidOperationException { Message: { Length: > 0 } message } &&
            IsAppAuthoredMessage(message))
        {
            return message;
        }

        return fallback ?? Generic;
    }

    private static bool IsAppAuthoredMessage(string message) =>
        message.StartsWith("Backup failed:", StringComparison.Ordinal) ||
        message.StartsWith("Only SQLite is supported", StringComparison.Ordinal);
}