using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FurniComply.Infrastructure.Services;

public sealed class ComplianceAlertBackgroundService : BackgroundService
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromMinutes(15);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ComplianceAlertBackgroundService> _logger;

    public ComplianceAlertBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ComplianceAlertBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RefreshAlertsAsync(stoppingToken);

        using var timer = new PeriodicTimer(RefreshInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RefreshAlertsAsync(stoppingToken);
        }
    }

    private async Task RefreshAlertsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var now = DateTime.UtcNow;

            var rules = await db.ComplianceAlertRules
                .AsNoTracking()
                .Where(r => r.IsEnabled)
                .ToListAsync(cancellationToken);

            if (rules.Count == 0)
            {
                return;
            }

            var generated = new List<ComplianceAlert>();
            foreach (var rule in rules)
            {
                switch (rule.RuleKey)
                {
                    case ComplianceAlertRuleKeys.NonCompliantChecks:
                    {
                        var nonCompliantCount = await db.ComplianceChecks
                            .CountAsync(c => c.ComplianceStatus != null && c.ComplianceStatus.Name == "Non-Compliant", cancellationToken);
                        if (nonCompliantCount >= Math.Max(1, rule.ThresholdValue))
                        {
                            generated.Add(new ComplianceAlert
                            {
                                Id = Guid.NewGuid(),
                                ComplianceAlertRuleId = rule.Id,
                                Severity = rule.Severity,
                                Title = "Open non-compliant checks",
                                Message = $"{nonCompliantCount} checks require closure.",
                                EntityType = "ComplianceCheck",
                                IsActive = true,
                                TriggeredAtUtc = now,
                                CreatedAtUtc = now
                            });
                        }

                        break;
                    }
                    case ComplianceAlertRuleKeys.SuppliersOnHold:
                    {
                        var onHoldCount = await db.Suppliers
                            .CountAsync(s => s.Status == SupplierStatus.OnHold, cancellationToken);
                        if (onHoldCount >= Math.Max(1, rule.ThresholdValue))
                        {
                            generated.Add(new ComplianceAlert
                            {
                                Id = Guid.NewGuid(),
                                ComplianceAlertRuleId = rule.Id,
                                Severity = rule.Severity,
                                Title = "Suppliers on hold",
                                Message = $"{onHoldCount} suppliers are currently on hold.",
                                EntityType = "Supplier",
                                IsActive = true,
                                TriggeredAtUtc = now,
                                CreatedAtUtc = now
                            });
                        }

                        break;
                    }
                    case ComplianceAlertRuleKeys.SupplierRenewalDueSoon:
                    {
                        var dueBy = now.AddDays(Math.Max(1, rule.ThresholdValue));
                        var dueSoon = await db.Suppliers
                            .AsNoTracking()
                            .Where(s => s.RenewalDueUtc.HasValue && s.RenewalDueUtc.Value >= now && s.RenewalDueUtc.Value <= dueBy)
                            .Select(s => new { s.Id, s.Name, s.RenewalDueUtc })
                            .ToListAsync(cancellationToken);

                        foreach (var supplier in dueSoon)
                        {
                            generated.Add(new ComplianceAlert
                            {
                                Id = Guid.NewGuid(),
                                ComplianceAlertRuleId = rule.Id,
                                Severity = rule.Severity,
                                Title = "Supplier renewal due soon",
                                Message = $"{supplier.Name} renewal due on {supplier.RenewalDueUtc:yyyy-MM-dd}.",
                                EntityType = "Supplier",
                                EntityId = supplier.Id,
                                IsActive = true,
                                TriggeredAtUtc = now,
                                CreatedAtUtc = now
                            });
                        }

                        break;
                    }
                    case ComplianceAlertRuleKeys.SupplierRenewalOverdue:
                    {
                        var overdue = await db.Suppliers
                            .AsNoTracking()
                            .Where(s => s.RenewalDueUtc.HasValue && s.RenewalDueUtc.Value < now)
                            .Select(s => new { s.Id, s.Name, s.RenewalDueUtc })
                            .ToListAsync(cancellationToken);

                        foreach (var supplier in overdue)
                        {
                            generated.Add(new ComplianceAlert
                            {
                                Id = Guid.NewGuid(),
                                ComplianceAlertRuleId = rule.Id,
                                Severity = rule.Severity,
                                Title = "Supplier renewal overdue",
                                Message = $"{supplier.Name} renewal was due {supplier.RenewalDueUtc:yyyy-MM-dd}.",
                                EntityType = "Supplier",
                                EntityId = supplier.Id,
                                IsActive = true,
                                TriggeredAtUtc = now,
                                CreatedAtUtc = now
                            });
                        }

                        break;
                    }
                }
            }

            var activeAlerts = await db.ComplianceAlerts
                .Where(a => a.IsActive)
                .ToListAsync(cancellationToken);

            var generatedKeys = generated.ToDictionary(BuildAlertKey, a => a, StringComparer.OrdinalIgnoreCase);
            var activeByKey = activeAlerts.ToDictionary(BuildAlertKey, a => a, StringComparer.OrdinalIgnoreCase);

            foreach (var activeAlert in activeAlerts)
            {
                if (!generatedKeys.ContainsKey(BuildAlertKey(activeAlert)))
                {
                    activeAlert.IsActive = false;
                    activeAlert.ResolvedAtUtc = now;
                    activeAlert.UpdatedAtUtc = now;
                }
            }

            foreach (var generatedAlert in generated)
            {
                if (activeByKey.ContainsKey(BuildAlertKey(generatedAlert)))
                {
                    continue;
                }

                db.ComplianceAlerts.Add(generatedAlert);
            }

            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh compliance alerts.");
        }
    }

    private static string BuildAlertKey(ComplianceAlert alert)
    {
        return $"{alert.ComplianceAlertRuleId:N}:{alert.EntityType}:{alert.EntityId?.ToString("N") ?? "none"}:{alert.Title}:{alert.Message}";
    }
}
