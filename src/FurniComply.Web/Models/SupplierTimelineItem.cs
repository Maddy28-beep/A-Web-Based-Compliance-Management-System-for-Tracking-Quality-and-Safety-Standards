namespace FurniComply.Web.Models;

public sealed class SupplierTimelineItem
{
    public DateTime DateUtc { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? SourceId { get; set; }
    public string? SourceRoute { get; set; }
    public string? Actor { get; set; }
}
