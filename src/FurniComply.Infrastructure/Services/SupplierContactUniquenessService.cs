using System.Text.RegularExpressions;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Infrastructure.Services;

public sealed class SupplierContactUniquenessService
{
    private readonly AppDbContext _db;

    public SupplierContactUniquenessService(AppDbContext db)
    {
        _db = db;
    }

    public static string? NormalizeEmail(string? email) =>
        string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToUpperInvariant();

    public static string? NormalizePhilippineMobile(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return null;
        }

        var digits = Regex.Replace(phone, @"\D", "");
        if (digits.Length == 12 && digits.StartsWith("63", StringComparison.Ordinal))
        {
            digits = "0" + digits[2..];
        }
        else if (digits.Length == 10 && digits.StartsWith('9'))
        {
            digits = "0" + digits;
        }

        return digits.Length == 11 && digits.StartsWith("09", StringComparison.Ordinal) ? digits : digits;
    }

    public async Task<SupplierContactConflict?> FindSupplierContactConflictAsync(
        string? contactEmail,
        string? phoneNumber,
        Guid? excludeSupplierId = null,
        CancellationToken cancellationToken = default)
    {
        var targetEmail = NormalizeEmail(contactEmail);
        var targetPhone = NormalizePhilippineMobile(phoneNumber);
        if (targetEmail == null && targetPhone == null)
        {
            return null;
        }

        var suppliers = await _db.Suppliers
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .Where(s => excludeSupplierId == null || s.Id != excludeSupplierId)
            .Select(s => new { s.Name, s.ContactEmail, s.PhoneNumber })
            .ToListAsync(cancellationToken);

        foreach (var supplier in suppliers)
        {
            if (targetEmail != null && NormalizeEmail(supplier.ContactEmail) == targetEmail)
            {
                return new SupplierContactConflict(
                    "ContactEmail",
                    supplier.Name,
                    "email",
                    contactEmail!.Trim());
            }

            if (targetPhone != null
                && !string.IsNullOrWhiteSpace(supplier.PhoneNumber)
                && NormalizePhilippineMobile(supplier.PhoneNumber) == targetPhone)
            {
                return new SupplierContactConflict(
                    "PhoneNumber",
                    supplier.Name,
                    "phone number",
                    phoneNumber!.Trim());
            }
        }

        return null;
    }

    public async Task<bool> IsEmailInUseAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeEmail(email);
        if (normalized == null)
        {
            return false;
        }

        var userExists = await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email != null && u.Email.ToUpper() == normalized, cancellationToken);
        if (userExists)
        {
            return true;
        }

        var accessRequestExists = await _db.AccessRequests
            .AsNoTracking()
            .AnyAsync(
                r => r.Status != AccessRequestStatus.Rejected && r.Email.ToUpper() == normalized,
                cancellationToken);
        if (accessRequestExists)
        {
            return true;
        }

        var supplierConflict = await FindSupplierContactConflictAsync(email, null, null, cancellationToken);
        return supplierConflict != null;
    }
}

public sealed record SupplierContactConflict(
    string FieldName,
    string ExistingSupplierName,
    string ContactTypeLabel,
    string Value);
