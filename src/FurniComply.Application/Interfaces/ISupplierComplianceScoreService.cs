using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Application.Models;

namespace FurniComply.Application.Interfaces;

public interface ISupplierComplianceScoreService
{
    Task<SupplierComplianceScoreResult?> GetSupplierComplianceScoreAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupplierComplianceScoreResult>> GetSupplierComplianceScoresAsync(IEnumerable<Guid> supplierIds, CancellationToken cancellationToken = default);
}
