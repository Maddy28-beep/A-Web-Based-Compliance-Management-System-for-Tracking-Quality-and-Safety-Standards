using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FurniComply.Infrastructure.Services;

public sealed class SensitiveDataProtectionService
{
    private readonly AppDbContext _dbContext;
    private readonly IEncryptionService _encryptionService;
    private readonly ILogger<SensitiveDataProtectionService> _logger;

    public SensitiveDataProtectionService(
        AppDbContext dbContext,
        IEncryptionService encryptionService,
        ILogger<SensitiveDataProtectionService> logger)
    {
        _dbContext = dbContext;
        _encryptionService = encryptionService;
        _logger = logger;
    }

    public async Task ProtectExistingSensitiveDataAsync(CancellationToken cancellationToken = default)
    {
        var updatedRecords = 0;

        updatedRecords += await ProtectSuppliersAsync(cancellationToken);
        updatedRecords += await ProtectAccessRequestsAsync(cancellationToken);
        updatedRecords += await ProtectSecurityLogsAsync(cancellationToken);
        updatedRecords += await ProtectAuditLogsAsync(cancellationToken);

        if (updatedRecords == 0)
        {
            _logger.LogInformation("Sensitive data protection check completed. No plaintext rows needed encryption.");
            return;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Sensitive data protection completed. Encrypted {Count} plaintext field values.", updatedRecords);
    }

    private async Task<int> ProtectSuppliersAsync(CancellationToken cancellationToken)
    {
        var changed = 0;
        var suppliers = await _dbContext.Suppliers.IgnoreQueryFilters().ToListAsync(cancellationToken);
        foreach (var supplier in suppliers)
        {
            changed += EncryptIfNeeded(supplier, supplier.ContactEmail, encrypted => supplier.ContactEmail = encrypted);
            changed += EncryptIfNeeded(supplier, supplier.PhoneNumber, encrypted => supplier.PhoneNumber = encrypted);
            changed += EncryptIfNeeded(supplier, supplier.Address, encrypted => supplier.Address = encrypted);
        }

        return changed;
    }

    private async Task<int> ProtectAccessRequestsAsync(CancellationToken cancellationToken)
    {
        var changed = 0;
        var accessRequests = await _dbContext.AccessRequests.ToListAsync(cancellationToken);
        foreach (var accessRequest in accessRequests)
        {
            changed += EncryptIfNeeded(accessRequest, accessRequest.Email, encrypted => accessRequest.Email = encrypted);
            changed += EncryptIfNeeded(accessRequest, accessRequest.FullName, encrypted => accessRequest.FullName = encrypted);
            changed += EncryptIfNeeded(accessRequest, accessRequest.Reason, encrypted => accessRequest.Reason = encrypted);
            changed += EncryptIfNeeded(accessRequest, accessRequest.RejectionReason, encrypted => accessRequest.RejectionReason = encrypted);
        }

        return changed;
    }

    private async Task<int> ProtectSecurityLogsAsync(CancellationToken cancellationToken)
    {
        var changed = 0;
        var securityLogs = await _dbContext.SecurityLogs.IgnoreQueryFilters().ToListAsync(cancellationToken);
        foreach (var securityLog in securityLogs)
        {
            changed += EncryptIfNeeded(securityLog, securityLog.IpAddress, encrypted => securityLog.IpAddress = encrypted);
        }

        return changed;
    }

    private async Task<int> ProtectAuditLogsAsync(CancellationToken cancellationToken)
    {
        var changed = 0;
        var auditLogs = await _dbContext.AuditLogs.IgnoreQueryFilters().ToListAsync(cancellationToken);
        foreach (var auditLog in auditLogs)
        {
            changed += EncryptIfNeeded(auditLog, auditLog.IpAddress, encrypted => auditLog.IpAddress = encrypted);
        }

        return changed;
    }

    private int EncryptIfNeeded(object entity, string? currentValue, Action<string?> assignEncryptedValue)
    {
        if (string.IsNullOrWhiteSpace(currentValue) || _encryptionService.IsEncrypted(currentValue))
        {
            return 0;
        }

        assignEncryptedValue(_encryptionService.EncryptSensitiveData(currentValue));
        _logger.LogDebug("Encrypted plaintext sensitive field on entity {EntityType}.", entity.GetType().Name);
        return 1;
    }
}
