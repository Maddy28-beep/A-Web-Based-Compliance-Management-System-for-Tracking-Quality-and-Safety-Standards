using System;

namespace FurniComply.Domain.Entities;

public class SupplierEvaluation
{
    public int SupplierEvaluationId { get; set; }

    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public string? EvaluatedByUserId { get; set; }
    public string? EvaluatedByEmail { get; set; }
    public DateTime EvaluatedAtUtc { get; set; }

    public int ScoreSnapshot { get; set; }
    public string BandSnapshot { get; set; } = string.Empty;
    public string ReasonsSnapshot { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
