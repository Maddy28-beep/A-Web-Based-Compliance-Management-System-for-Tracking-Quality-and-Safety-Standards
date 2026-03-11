using System;

namespace FurniComply.Domain.Entities;

public class SupplierPerformance : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    
    public DateTime EvaluationDateUtc { get; set; } = DateTime.UtcNow;
    
    public string? ProductName { get; set; }
    public Guid? ProductId { get; set; }
    
    public int QualityRating { get; set; } // 1-5
    public int DeliveryRating { get; set; } // 1-5
    public decimal DefectRate { get; set; } // Percentage
    
    public decimal PerformanceScore { get; set; } // (Quality + Delivery) / 2
    
    public string Remarks { get; set; } = string.Empty;
    public string EvaluatedBy { get; set; } = string.Empty;
}
