using System;
using System.ComponentModel.DataAnnotations;

namespace FurniComply.Domain.Entities;

public class ProcurementItem : BaseEntity
{
    public Guid ProcurementOrderId { get; set; }
    public ProcurementOrder? ProcurementOrder { get; set; }

    [Required(ErrorMessage = "Item name is required.")]
    [StringLength(200, ErrorMessage = "Item name cannot exceed 200 characters.")]
    public string ItemName { get; set; } = string.Empty;

    [Range(1, 999999, ErrorMessage = "Quantity must be between 1 and 999,999.")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Unit cost cannot be negative.")]
    public decimal UnitCost { get; set; }
    [StringLength(200)]
    public string QualityStandard { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;
}
