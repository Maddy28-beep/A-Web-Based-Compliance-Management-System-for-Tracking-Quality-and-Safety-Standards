using System;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "System.Admin")]
public class ComplianceAlertRulesController : Controller
{
    private readonly AppDbContext _db;

    public ComplianceAlertRulesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var rules = await _db.ComplianceAlertRules
            .OrderBy(r => r.RuleKey)
            .ToListAsync();

        return View(rules);
    }

    public IActionResult Create()
    {
        return View(new ComplianceAlertRule());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ComplianceAlertRule rule)
    {
        if (!ModelState.IsValid)
        {
            return View(rule);
        }

        _db.ComplianceAlertRules.Add(rule);
        AddAudit(nameof(ComplianceAlertRule), rule.Id, "Create", $"Created alert rule {rule.RuleKey}.");
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var rule = await _db.ComplianceAlertRules.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }

        return View(rule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ComplianceAlertRule rule)
    {
        if (id != rule.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(rule);
        }

        _db.Entry(rule).State = EntityState.Modified;
        AddAudit(nameof(ComplianceAlertRule), rule.Id, "Edit", $"Updated alert rule {rule.RuleKey}.");
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var rule = await _db.ComplianceAlertRules.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }

        return View(rule);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var rule = await _db.ComplianceAlertRules.FindAsync(id);
        if (rule != null)
        {
            rule.IsDeleted = true;
            rule.DeletedAtUtc = DateTime.UtcNow;
            rule.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(ComplianceAlertRule), rule.Id, "Archive", $"Archived alert rule {rule.RuleKey}.");
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void AddAudit(string entityName, Guid entityId, string action, string details)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            Actor = User.Identity?.Name ?? "system",
            TimestampUtc = DateTime.UtcNow,
            Details = details
        });
    }
}
