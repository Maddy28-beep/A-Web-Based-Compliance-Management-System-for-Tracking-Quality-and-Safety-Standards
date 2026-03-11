using System;
using System.Collections.Generic;
namespace FurniComply.Domain.Entities;

public class ProcurementOrder : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public Guid ProcurementStatusId { get; set; }
    public ProcurementStatusMaster? ProcurementStatus { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public decimal? ExchangeRateUsed { get; set; }
    public DateTime? ExchangeRateTimestampUtc { get; set; }
    public List<ProcurementItem> Items { get; set; } = new();
}
