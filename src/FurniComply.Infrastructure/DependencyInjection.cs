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
        var connectionString = configuration.GetConnectionString("DefaultConnection")?.Trim();
        if (string.IsNullOrEmpty(connectionString) ||
            connectionString.StartsWith("${", StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' is missing, empty, or still contains an unreplaced placeholder (for example ${DB_CONNECTION}). " +
                "Set ConnectionStrings:DefaultConnection in appsettings.json, appsettings.{Environment}.json, user secrets, or the environment (e.g. ConnectionStrings__DefaultConnection).");
        }

        if (!connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only SQLite is supported for local development. " +
                "Set ConnectionStrings:DefaultConnection to a path such as " +
                "Data Source=App_Data/FurniComply.local.db (see .env.example).");
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<SupplierContactUniquenessService>();
        services.AddHostedService<ComplianceAlertBackgroundService>();
        services.AddHostedService<PolicyRetentionBackgroundService>();

        return services;
    }
}
