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

[Authorize]
public class PoliciesController : Controller
{
    private readonly AppDbContext _db;

    public PoliciesController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize(Policy = "Policies.Read")]
    public async Task<IActionResult> Index()
    {
        var policiesQuery = _db.Policies
            .Include(p => p.PolicyCategory)
            .AsQueryable();

        if (IsDepartmentHead())
        {
            var owner = CurrentDepartmentOwner();
            policiesQuery = policiesQuery.Where(p => p.Owner == owner);
        }

        var policies = await policiesQuery
            .OrderByDescending(p => p.EffectiveDateUtc)
            .ToListAsync();

        return View(policies);
    }

    [Authorize(Policy = "Policies.Read")]
    public async Task<IActionResult> Deleted()
    {
        var policiesQuery = _db.Policies
            .IgnoreQueryFilters()
            .Include(p => p.PolicyCategory)
            .Where(p => p.IsDeleted)
            .AsQueryable();

        if (IsDepartmentHead())
        {
            var owner = CurrentDepartmentOwner();
            policiesQuery = policiesQuery.Where(p => p.Owner == owner);
        }

        var policies = await policiesQuery
            .OrderByDescending(p => p.DeletedAtUtc)
            .ToListAsync();

        return View(policies);
    }

    [Authorize(Policy = "Policies.Read")]
    public async Task<IActionResult> Details(Guid id)
    {
        var policy = await _db.Policies
            .Include(p => p.PolicyCategory)
            .Include(p => p.PolicyVersions)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (policy == null)
        {
            return NotFound();
        }
        if (IsDepartmentHead() && !CanAccessPolicy(policy.Owner))
        {
            return Forbid();
        }

        return View(policy);
    }

    [Authorize(Policy = "Policies.Write")]
    public IActionResult Create()
    {
        PopulateCategories();
        return View(new Policy { EffectiveDateUtc = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> Create(Policy policy)
    {
        if (!ModelState.IsValid)
        {
            PopulateCategories();
            return View(policy);
        }

        policy.Code = await GenerateNextPolicyCodeAsync();

        var isOperationalAdmin = User.IsInRole(RoleNames.Admin) && !User.IsInRole(RoleNames.SuperAdmin);
        var isComplianceOperator = User.IsInRole(RoleNames.ComplianceManager) && !User.IsInRole(RoleNames.SuperAdmin);
        if (isOperationalAdmin)
        {
            policy.Status = FurniComply.Domain.Enums.PolicyStatus.Active;
        }
        else
        {
            policy.Status = FurniComply.Domain.Enums.PolicyStatus.Draft;
        }

        _db.Policies.Add(policy);
        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Initial policy version."
        });
        AddAudit(nameof(Policy), policy.Id, "Create", $"Created policy {policy.Code} with status {policy.Status}.");
        if (isOperationalAdmin)
        {
            AddAudit(nameof(Policy), policy.Id, "Approve", $"Auto-approved policy {policy.Code} on create by Admin.");
        }
        else if (isComplianceOperator)
        {
            AddAudit(nameof(Policy), policy.Id, "Submit", $"Submitted draft policy {policy.Code} for Admin review.");
        }

        try
        {
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = isOperationalAdmin
                ? $"Policy {policy.Code} created and approved."
                : $"Policy {policy.Code} created as draft.";
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(Policy.Code), "Failed to generate a unique policy code. Please try again.");
            PopulateCategories();
            _db.ChangeTracker.Clear();
            return View(policy);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var policy = await _db.Policies.FindAsync(id);
        if (policy == null)
        {
            return NotFound();
        }

        PopulateCategories();
        return View(policy);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> Edit(Guid id, Policy policy)
    {
        if (id != policy.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            PopulateCategories();
            return View(policy);
        }

        var isOperationalAdmin = User.IsInRole(RoleNames.Admin) && !User.IsInRole(RoleNames.SuperAdmin);
        if (!isOperationalAdmin)
        {
            policy.Status = FurniComply.Domain.Enums.PolicyStatus.Draft;
        }

        _db.Entry(policy).State = EntityState.Modified;
        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Snapshot saved from policy edit."
        });
        AddAudit(nameof(Policy), policy.Id, "Edit", $"Updated policy {policy.Code}.");
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var policy = await _db.Policies
            .Include(p => p.PolicyCategory)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (policy == null)
        {
            return NotFound();
        }

        return View(policy);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var policy = await _db.Policies.FindAsync(id);
        if (policy == null)
        {
            return RedirectToAction(nameof(Index));
        }

        policy.Status = FurniComply.Domain.Enums.PolicyStatus.Retired;
        policy.IsDeleted = true;
        policy.DeletedAtUtc = DateTime.UtcNow;
        policy.UpdatedAtUtc = DateTime.UtcNow;
        _db.Entry(policy).State = EntityState.Modified;
        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Policy retired via delete action."
        });
        AddAudit(nameof(Policy), policy.Id, "Retire", $"Retired policy {policy.Code}.");
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> RollbackLatest(Guid id)
    {
        var policy = await _db.Policies
            .Include(p => p.PolicyVersions)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (policy == null)
        {
            return NotFound();
        }

        var ordered = policy.PolicyVersions
            .OrderByDescending(v => v.EffectiveDateUtc)
            .ThenByDescending(v => v.CreatedAtUtc)
            .ToList();

        if (ordered.Count < 2)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        var rollbackTo = ordered[1];
        policy.Version = rollbackTo.VersionNumber;
        policy.Status = rollbackTo.Status;
        policy.EffectiveDateUtc = rollbackTo.EffectiveDateUtc;
        policy.Owner = rollbackTo.Owner;
        policy.IsDeleted = false;
        policy.DeletedAtUtc = null;
        policy.UpdatedAtUtc = DateTime.UtcNow;

        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = $"Rollback to version {rollbackTo.VersionNumber}."
        });
        AddAudit(nameof(Policy), policy.Id, "Rollback", $"Rollback to version {rollbackTo.VersionNumber}.");

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> RollbackSelect(Guid id)
    {
        var policy = await _db.Policies
            .Include(p => p.PolicyVersions)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (policy == null)
        {
            return NotFound();
        }

        return View(policy);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> RollbackToVersion(Guid policyId, Guid versionId)
    {
        var policy = await _db.Policies
            .Include(p => p.PolicyVersions)
            .FirstOrDefaultAsync(p => p.Id == policyId);
        if (policy == null)
        {
            return NotFound();
        }

        var target = policy.PolicyVersions.FirstOrDefault(v => v.Id == versionId);
        if (target == null)
        {
            return NotFound();
        }

        policy.Version = target.VersionNumber;
        policy.Status = target.Status;
        policy.EffectiveDateUtc = target.EffectiveDateUtc;
        policy.Owner = target.Owner;
        policy.IsDeleted = false;
        policy.DeletedAtUtc = null;
        policy.UpdatedAtUtc = DateTime.UtcNow;

        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = $"Rollback to version {target.VersionNumber}."
        });
        AddAudit(nameof(Policy), policy.Id, "Rollback", $"Rollback to version {target.VersionNumber}.");

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = policy.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Write")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var policy = await _db.Policies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == id);
        if (policy == null)
        {
            return NotFound();
        }

        policy.IsDeleted = false;
        policy.DeletedAtUtc = null;
        policy.UpdatedAtUtc = DateTime.UtcNow;
        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Policy restored."
        });
        AddAudit(nameof(Policy), policy.Id, "Restore", $"Restored policy {policy.Code}.");

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = policy.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var policy = await _db.Policies.FindAsync(id);
        if (policy == null)
        {
            return NotFound();
        }

        policy.Status = FurniComply.Domain.Enums.PolicyStatus.Active;
        policy.UpdatedAtUtc = DateTime.UtcNow;

        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Approved by Admin."
        });

        AddAudit(nameof(Policy), policy.Id, "Approve", $"Approved policy {policy.Code}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Policy {policy.Code} approved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Policies.Approve")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var policy = await _db.Policies.FindAsync(id);
        if (policy == null)
        {
            return NotFound();
        }

        policy.Status = FurniComply.Domain.Enums.PolicyStatus.Draft;
        policy.UpdatedAtUtc = DateTime.UtcNow;

        _db.PolicyVersions.Add(new PolicyVersion
        {
            PolicyId = policy.Id,
            VersionNumber = policy.Version,
            Status = policy.Status,
            EffectiveDateUtc = policy.EffectiveDateUtc,
            Owner = policy.Owner,
            Notes = "Rejected by Admin. Returned to Draft."
        });

        AddAudit(nameof(Policy), policy.Id, "Reject", $"Rejected policy {policy.Code} and returned to Draft.");
        await _db.SaveChangesAsync();
        TempData["ErrorMessage"] = $"Policy {policy.Code} rejected and returned to draft.";
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

    private void PopulateCategories()
    {
        var categories = _db.PolicyCategories
            .OrderBy(c => c.SortOrder)
            .Select(c => new { c.Id, c.Name })
            .ToList();
        ViewBag.PolicyCategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Id", "Name");
    }

    private async Task<string> GenerateNextPolicyCodeAsync()
    {
        var existingCodes = await _db.Policies
            .IgnoreQueryFilters()
            .Select(p => p.Code)
            .ToListAsync();

        var max = existingCodes
            .Select(code =>
            {
                var digits = new string((code ?? string.Empty).Where(char.IsDigit).ToArray());
                return int.TryParse(digits, out var n) ? n : 0;
            })
            .DefaultIfEmpty(0)
            .Max();

        return $"POL-{max + 1:000}";
    }

    private bool IsDepartmentHead() =>
        User.IsInRole(RoleNames.DepartmentHead);

    private string CurrentDepartmentOwner() =>
        User.Identity?.Name ?? string.Empty;

    private bool CanAccessPolicy(string? owner) =>
        !IsDepartmentHead() || string.Equals(owner, CurrentDepartmentOwner(), StringComparison.OrdinalIgnoreCase);
}
