using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Audit.Read")]
public class AuditLogsController : Controller
{
    private static readonly string[] ExcludedSecurityActions =
    {
        "Login Succeeded",
        "Login Failed"
    };

    private readonly AppDbContext _db;

    public AuditLogsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? actor, string? entity, string? auditAction, string? ipAddress, DateTime? from, DateTime? to)
    {
        var query = ApplyDisplayedAuditLogs(ApplyRoleScope(_db.AuditLogs.AsQueryable()));

        if (!string.IsNullOrWhiteSpace(actor))
        {
            query = query.Where(a => a.Actor == actor);
        }

        if (!string.IsNullOrWhiteSpace(entity))
        {
            query = query.Where(a => a.EntityName == entity);
        }

        if (!string.IsNullOrWhiteSpace(auditAction))
        {
            query = query.Where(a => a.Action == auditAction);
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

        var logs = await query
            .OrderByDescending(a => a.TimestampUtc)
            .Take(500)
            .ToListAsync();

        ViewBag.Actor = actor;
        ViewBag.Entity = entity;
        ViewBag.AuditAction = auditAction;
        ViewBag.IpAddress = ipAddress;
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");
        ViewBag.IsScopedToCurrentUser = IsCurrentUserScoped();
        ViewBag.CanViewSensitiveFields = CanViewSensitiveFields();
        var scopedQuery = ApplyDisplayedAuditLogs(ApplyRoleScope(_db.AuditLogs.AsQueryable()));
        ViewBag.Actors = await scopedQuery
            .Select(a => a.Actor)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Entities = await scopedQuery
            .Select(a => a.EntityName)
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

        return View(logs);
    }

    [Authorize(Policy = "System.Admin")]
    public async Task<IActionResult> Overrides(DateTime? from, DateTime? to)
    {
        var overrideActions = new[] 
        { 
            "OverrideEnabled", 
            "OverrideDisabled", 
            "OverrideRequested", 
            "OverrideRequestRejected",
            "ApproveOverride",
            "Submit Override (Documents)",
            "Approve Override (Documents)",
            "Place Order Override (Documents)"
        };

        var query = ApplyDisplayedAuditLogs(ApplyRoleScope(_db.AuditLogs.AsQueryable()));
        query = query.Where(a => overrideActions.Contains(a.Action) || a.Action.Contains("Override"));

        if (from.HasValue)
        {
            query = query.Where(a => a.TimestampUtc >= from.Value.Date);
        }

        if (to.HasValue)
        {
            query = query.Where(a => a.TimestampUtc < to.Value.Date.AddDays(1));
        }

        var logs = await query
            .OrderByDescending(a => a.TimestampUtc)
            .Take(1000)
            .ToListAsync();

        ViewData["Title"] = "Override Logs";
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");

        // Populate selects for the Index view to work correctly
        ViewBag.IsScopedToCurrentUser = IsCurrentUserScoped();
        ViewBag.CanViewSensitiveFields = CanViewSensitiveFields();
        var scopedQuery = ApplyDisplayedAuditLogs(ApplyRoleScope(_db.AuditLogs.AsQueryable()));
        ViewBag.Actors = await scopedQuery
            .Select(a => a.Actor)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Entities = await scopedQuery
            .Select(a => a.EntityName)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Actions = await scopedQuery
            .Select(a => a.Action)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

        return View("Index", logs);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var log = await ApplyRoleScope(_db.AuditLogs.AsQueryable()).FirstOrDefaultAsync(a => a.Id == id);
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
        var query = ApplyDisplayedAuditLogs(ApplyRoleScope(_db.AuditLogs.AsQueryable()));

        if (from.HasValue)
        {
            query = query.Where(a => a.TimestampUtc >= from.Value.Date);
        }

        if (to.HasValue)
        {
            query = query.Where(a => a.TimestampUtc < to.Value.Date.AddDays(1));
        }

        var logs = await query
            .OrderByDescending(a => a.TimestampUtc)
            .Take(2000)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("TimestampUtc,Actor,IpAddress,Action,EntityName,EntityId,Details");
        foreach (var log in logs)
        {
            csv
                .Append(EscapeCsv(log.TimestampUtc.ToString("O"))).Append(',')
                .Append(EscapeCsv(log.Actor)).Append(',')
                .Append(EscapeCsv(log.IpAddress)).Append(',')
                .Append(EscapeCsv(log.Action)).Append(',')
                .Append(EscapeCsv(log.EntityName)).Append(',')
                .Append(EscapeCsv(log.EntityId.ToString())).Append(',')
                .Append(EscapeCsv(log.Details))
                .AppendLine();
        }

        var fileName = $"audit-logs-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
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

    private IQueryable<Domain.Entities.AuditLog> ApplyRoleScope(IQueryable<Domain.Entities.AuditLog> query)
    {
        if (HasFullAuditVisibility())
        {
            return query;
        }

        var actor = User.Identity?.Name ?? string.Empty;
        return string.IsNullOrWhiteSpace(actor)
            ? query.Where(_ => false)
            : query.Where(log => log.Actor == actor);
    }

    private bool HasFullAuditVisibility() =>
        User.IsInRole(RoleNames.SuperAdmin) ||
        User.IsInRole(RoleNames.Admin) ||
        User.IsInRole(RoleNames.ComplianceManager) ||
        User.IsInRole(RoleNames.Auditor);

    private bool IsCurrentUserScoped() => !HasFullAuditVisibility();

    private bool CanViewSensitiveFields() => HasFullAuditVisibility();

    private static IQueryable<Domain.Entities.AuditLog> ApplyDisplayedAuditLogs(
        IQueryable<Domain.Entities.AuditLog> query) =>
        query.Where(log => !ExcludedSecurityActions.Contains(log.Action));
}
