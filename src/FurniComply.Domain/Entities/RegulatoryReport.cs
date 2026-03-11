using System;
namespace FurniComply.Domain.Entities;

public class RegulatoryReport : BaseEntity
{
    public Guid ReportStatusId { get; set; }
    public ReportStatusMaster? ReportStatus { get; set; }
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime PeriodStartUtc { get; set; }
    public DateTime PeriodEndUtc { get; set; }
    public DateTime? SubmittedAtUtc { get; set; }
    public string Summary { get; set; } = string.Empty;
}
