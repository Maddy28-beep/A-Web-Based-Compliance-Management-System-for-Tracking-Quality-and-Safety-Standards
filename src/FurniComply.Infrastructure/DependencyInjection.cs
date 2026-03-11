using FurniComply.Application.Interfaces;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FurniComply.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (!string.IsNullOrWhiteSpace(connectionString) &&
                connectionString.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddHostedService<ComplianceAlertBackgroundService>();
        services.AddHostedService<PolicyRetentionBackgroundService>();

        return services;
    }
}
