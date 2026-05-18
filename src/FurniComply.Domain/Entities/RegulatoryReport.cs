using System;
using System.ComponentModel.DataAnnotations;

namespace FurniComply.Domain.Entities;

public class RegulatoryReport : BaseEntity
{
    public Guid ReportStatusId { get; set; }
    public ReportStatusMaster? ReportStatus { get; set; }
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    [Required(ErrorMessage = "Report type is required.")]
    [StringLength(100, ErrorMessage = "Report type cannot exceed 100 characters.")]
    public string ReportType { get; set; } = string.Empty;
    public DateTime PeriodStartUtc { get; set; }
    public DateTime PeriodEndUtc { get; set; }
    public DateTime? SubmittedAtUtc { get; set; }

    [StringLength(2000, ErrorMessage = "Summary cannot exceed 2000 characters.")]
    public string Summary { get; set; } = string.Empty;
}
