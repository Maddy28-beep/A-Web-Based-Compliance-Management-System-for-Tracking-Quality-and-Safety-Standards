using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Audit.Read")]
public class AuditLogsController : Controller
{
    private readonly AppDbContext _db;

    public AuditLogsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? actor, string? entity, string? auditAction, DateTime? from, DateTime? to)
    {
        var query = _db.AuditLogs.AsQueryable();

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
        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");
        ViewBag.Actors = await _db.AuditLogs
            .Select(a => a.Actor)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Entities = await _db.AuditLogs
            .Select(a => a.EntityName)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Actions = await _db.AuditLogs
            .Select(a => a.Action)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

        return View(logs);
    }

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

        var query = _db.AuditLogs.AsQueryable();
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
        ViewBag.Actors = await _db.AuditLogs
            .Select(a => a.Actor)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Entities = await _db.AuditLogs
            .Select(a => a.EntityName)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();
        ViewBag.Actions = await _db.AuditLogs
            .Select(a => a.Action)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

        return View("Index", logs);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var log = await _db.AuditLogs.FirstOrDefaultAsync(a => a.Id == id);
        if (log == null)
        {
            return NotFound();
        }

        return View(log);
    }

    [Authorize(Policy = "Audit.Export")]
    public async Task<IActionResult> ExportCsv(DateTime? from, DateTime? to)
    {
        var query = _db.AuditLogs.AsQueryable();

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
        csv.AppendLine("TimestampUtc,Actor,Action,EntityName,EntityId,Details");
        foreach (var log in logs)
        {
            csv
                .Append(EscapeCsv(log.TimestampUtc.ToString("O"))).Append(',')
                .Append(EscapeCsv(log.Actor)).Append(',')
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
}
