using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurniComply.Domain.Entities;

public class ProcurementOrder : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public Guid ProcurementStatusId { get; set; }
    public ProcurementStatusMaster? ProcurementStatus { get; set; }

    [StringLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;

    [Range(0, double.MaxValue, ErrorMessage = "Total amount cannot be negative.")]
    public decimal TotalAmount { get; set; }
    public decimal? ExchangeRateUsed { get; set; }
    public DateTime? ExchangeRateTimestampUtc { get; set; }
    public List<ProcurementItem> Items { get; set; } = new();
}
