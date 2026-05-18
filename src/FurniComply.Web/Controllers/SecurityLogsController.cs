using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using FurniComply.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Audit.Read")]
public class SecurityLogsController : Controller
{
    private const int MaxDisplayedLogs = 500;
    private const int MaxExportLogs = 2000;
    private static readonly string[] AllowedSecurityActions =
    {
        "Login Succeeded",
        "Login Failed",
        "Logout"
    };

    private readonly AppDbContext _db;

    public SecurityLogsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? actor, string? category, string? securityAction, string? ipAddress, DateTime? from, DateTime? to)
    {
        var scopedQuery = ApplyDisplayedSecurityLogs(ApplyRoleScope(_db.SecurityLogs.AsQueryable()));
        var query = ApplyFilters(scopedQuery, actor, category, securityAction, ipAddress, from, to);

        var logs = await query
            .OrderByDescending(a => a.TimestampUtc)
            .Take(MaxDisplayedLogs)
            .ToListAsync();

        await PopulateFilterViewDataAsync(actor, category, securityAction, ipAddress, from, to);

        var startOfToday = DateTime.UtcNow.Date;
        var endOfToday = startOfToday.AddDays(1);
        var model = new SecurityLogsIndexViewModel
        {
            Logs = logs,
            TopIpAddresses = logs
                .Where(log => !string.IsNullOrWhiteSpace(log.IpAddress))
                .GroupBy(log => log.IpAddress)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key)
                .Take(5)
                .Select(group => new SecurityLogTopIpViewModel(group.Key, group.Count()))
                .ToList(),
            TotalEvents = logs.Count,
            SuccessfulLoginsToday = logs.Count(log =>
                string.Equals(log.Action, "Login Succeeded", StringComparison.OrdinalIgnoreCase) &&
                log.TimestampUtc >= startOfToday &&
                log.TimestampUtc < endOfToday),
            FailedLoginsToday = logs.Count(log =>
                string.Equals(log.Action, "Login Failed", StringComparison.OrdinalIgnoreCase) &&
                log.TimestampUtc >= startOfToday &&
                log.TimestampUtc < endOfToday),
            UniqueIpCount = logs
                .Where(log => !string.IsNullOrWhiteSpace(log.IpAddress))
                .Select(log => log.IpAddress)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count(),
            IsScopedToCurrentUser = IsCurrentUserScoped(),
            CanViewSensitiveFields = CanViewSensitiveFields()
        };

        return View(model);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var log = await ApplyRoleScope(_db.SecurityLogs.AsQueryable()).FirstOrDefaultAsync(a => a.Id == id);
        if (log == null)
        {
            return NotFound();
        }

        ViewBag.CanViewSensitiveFields = CanViewSensitiveFields();
        return View(log);
    }

    [Authorize(Policy = "Audit.Export")]
    public async Task<IActionResult> ExportCsv(DateTime? from, DateTime? to)
    {
        var query = ApplyFilters(ApplyRoleScope(_db.SecurityLogs.AsQueryable()), null, null, null, null, from, to);
        query = ApplyDisplayedSecurityLogs(query);

        var logs = await query
            .OrderByDescending(a => a.TimestampUtc)
            .Take(MaxExportLogs)
            .ToListAsync();

        var fileName = $"security-logs-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
        return File(Encoding.UTF8.GetBytes(BuildCsv(logs)), "text/csv", fileName);
    }

    private static string EscapeCsv(string? value)
    {
        var safe = value ?? string.Empty;
        if (!safe.Contains(',') && !safe.Contains('"') && !safe.Contains('\n') && !safe.Contains('\r'))
        {
            return safe;
        }

        return $"\"{safe.Replace("\"", "\"\"")}\"";
    }

    private static IQueryable<Domain.Entities.SecurityLog> ApplyFilters(
        IQueryable<Domain.Entities.SecurityLog> query,
        string? actor,
        string? category,
        string? securityAction,
        string? ipAddress,
        DateTime? from,
        DateTime? to)
    {
        if (!string.IsNullOrWhiteSpace(actor))
        {
            query = query.Where(a => a.Actor == actor);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(a => a.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(securityAction))
        {
            query = query.Where(a => a.Action == securityAction);
        }

        if (!string.IsNullOrWhiteSpace(ipAddress))
        {
            query = query.Where(a => a.IpAddress == ipAddress);
        }

        if (from.HasValue)
        {
            query = query.Where(a => a.TimestampUtc >= from.Value.Date);
        }

        if (to.HasValue)
        {
            query = query.Where(a => a.TimestampUtc < to.Value.Date.AddDays(1));
        }

        return query;
    }

    private async Task PopulateFilterViewDataAsync(
        string? actor,
        string? category,
        string? securityAction,
        string? ipAddress,
        DateTime? from,
        DateTime? to)
    {
        ViewBag.Actor = actor;
        ViewBag.Category = category;
        ViewBag.SecurityAction = securityAction;
        ViewBag.IpAddress = ipAddress;
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");
        var scopedQuery = ApplyDisplayedSecurityLogs(ApplyRoleScope(_db.SecurityLogs.AsQueryable()));
        ViewBag.Actors = await scopedQuery
            .Select(a => a.Actor)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Categories = await scopedQuery
            .Select(a => a.Category)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Actions = await scopedQuery
            .Select(a => a.Action)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.IpAddresses = await scopedQuery
            .Where(a => !string.IsNullOrWhiteSpace(a.IpAddress))
            .Select(a => a.IpAddress)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
    }

    private IQueryable<Domain.Entities.SecurityLog> ApplyRoleScope(IQueryable<Domain.Entities.SecurityLog> query)
    {
        if (HasFullSecurityVisibility())
        {
            return query;
        }

        var actor = User.Identity?.Name ?? string.Empty;
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        return query.Where(log =>
            (!string.IsNullOrWhiteSpace(actor) && log.Actor == actor) ||
            (!string.IsNullOrWhiteSpace(userId) && log.UserId == userId));
    }

    private bool HasFullSecurityVisibility() =>
        User.IsInRole(RoleNames.SuperAdmin) ||
        User.IsInRole(RoleNames.Admin) ||
        User.IsInRole(RoleNames.ComplianceManager) ||
        User.IsInRole(RoleNames.Auditor);

    private bool IsCurrentUserScoped() => !HasFullSecurityVisibility();

    private bool CanViewSensitiveFields() =>
        User.IsInRole(RoleNames.SuperAdmin) ||
        User.IsInRole(RoleNames.Admin) ||
        User.IsInRole(RoleNames.ComplianceManager);

    private static IQueryable<Domain.Entities.SecurityLog> ApplyDisplayedSecurityLogs(
        IQueryable<Domain.Entities.SecurityLog> query) =>
        query.Where(log => AllowedSecurityActions.Contains(log.Action));

    private static string BuildCsv(IEnumerable<Domain.Entities.SecurityLog> logs)
    {
        var csv = new StringBuilder();
        csv.AppendLine("TimestampUtc,Category,Actor,UserId,IpAddress,Action,Details");

        foreach (var log in logs)
        {
            csv
                .Append(EscapeCsv(log.TimestampUtc.ToString("O"))).Append(',')
                .Append(EscapeCsv(log.Category)).Append(',')
                .Append(EscapeCsv(log.Actor)).Append(',')
                .Append(EscapeCsv(log.UserId)).Append(',')
                .Append(EscapeCsv(log.IpAddress)).Append(',')
                .Append(EscapeCsv(log.Action)).Append(',')
                .Append(EscapeCsv(log.Details))
                .AppendLine();
        }

        return csv.ToString();
    }
}
