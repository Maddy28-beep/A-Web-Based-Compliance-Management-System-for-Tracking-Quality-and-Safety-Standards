using System;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;

namespace FurniComply.Application.Interfaces;

public interface ISupplierPerformanceService
{
    Task EvaluatePerformanceAsync(Guid supplierId, int qualityRating, int deliveryRating, decimal defectRate, string remarks, string evaluatedBy, string? productName = null, Guid? productId = null);
    Task<decimal> CalculateOverallScoreAsync(Guid supplierId);
    Task UpdateSupplierRiskStatusAsync(Guid supplierId);
    Task<bool> CanSupplierBeUsedAsync(Guid supplierId);
}
