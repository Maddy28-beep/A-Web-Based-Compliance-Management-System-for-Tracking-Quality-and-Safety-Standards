using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FurniComply.Infrastructure.Services;

public sealed class PolicyRetentionBackgroundService : BackgroundService
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromHours(12);
    private static readonly TimeSpan RetentionPeriod = TimeSpan.FromDays(365);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PolicyRetentionBackgroundService> _logger;

    public PolicyRetentionBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<PolicyRetentionBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await PurgeAsync(stoppingToken);

        using var timer = new PeriodicTimer(RefreshInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await PurgeAsync(stoppingToken);
        }
    }

    private async Task PurgeAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var cutoff = DateTime.UtcNow.Subtract(RetentionPeriod);

            var expired = await db.Policies
                .IgnoreQueryFilters()
                .Where(p => p.IsDeleted && p.DeletedAtUtc.HasValue && p.DeletedAtUtc.Value <= cutoff)
                .ToListAsync(cancellationToken);

            if (expired.Count == 0)
            {
                return;
            }

            db.Policies.RemoveRange(expired);
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to purge deleted policies.");
        }
    }
}
