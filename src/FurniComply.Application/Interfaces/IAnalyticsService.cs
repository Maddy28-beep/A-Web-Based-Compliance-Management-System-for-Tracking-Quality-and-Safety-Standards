using System.Threading;
using System.Threading.Tasks;
using FurniComply.Application.Models;

namespace FurniComply.Application.Interfaces;

public interface IAnalyticsService
{
    Task<AnalyticsSnapshot> GetSnapshotAsync(System.DateTime? from = null, System.DateTime? to = null, CancellationToken cancellationToken = default);
}
