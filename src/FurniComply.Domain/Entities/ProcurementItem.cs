using System;

namespace FurniComply.Domain.Entities;

public class ProcurementItem : BaseEntity
{
    public Guid ProcurementOrderId { get; set; }
    public ProcurementOrder? ProcurementOrder { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string QualityStandard { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
