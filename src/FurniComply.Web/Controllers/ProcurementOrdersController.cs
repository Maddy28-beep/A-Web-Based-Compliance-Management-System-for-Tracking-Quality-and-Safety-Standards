using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FurniComply.Application.Interfaces;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurniComply.Web.Services;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Procurement.Read")]
public class ProcurementOrdersController : Controller
{
    private readonly AppDbContext _db;
    private readonly ISupplierPerformanceService _performance;
    private readonly ISupplierComplianceScoreService _complianceScore;
    private readonly IProcurementService _procurement;

    public ProcurementOrdersController(
        AppDbContext db,
        ISupplierPerformanceService performance,
        ISupplierComplianceScoreService complianceScore,
        IProcurementService procurement)
    {
        _db = db;
        _performance = performance;
        _complianceScore = complianceScore;
        _procurement = procurement;
    }


    public async Task<IActionResult> Index(string? status)
    {
        var query = _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .AsQueryable();

        var hasComplianceApprovalBlocker = await _db.CorrectiveActions.AnyAsync(c =>
            c.Status != CorrectiveActionStatus.Closed &&
            c.ComplianceCheck != null &&
            c.ComplianceCheck.ComplianceStatus != null &&
            c.ComplianceCheck.ComplianceStatus.Name == "Non-Compliant");
        ViewBag.HasComplianceApprovalBlocker = hasComplianceApprovalBlocker;

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (string.Equals(status, "Open", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(o =>
                    o.ProcurementStatus != null &&
                    o.ProcurementStatus.Name != "Closed" &&
                    o.ProcurementStatus.Name != "Cancelled");
            }
            else
            {
                query = query.Where(o => o.ProcurementStatus != null && o.ProcurementStatus.Name == status);
            }

            ViewBag.StatusFilter = status;
        }

        var orders = await query
            .OrderByDescending(o => o.OrderDateUtc)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Archived()
    {
        var orders = await _db.ProcurementOrders
            .IgnoreQueryFilters()
            .Where(o => o.IsDeleted)
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .OrderByDescending(o => o.DeletedAtUtc)
            .ThenByDescending(o => o.OrderDateUtc)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.Items)
            .Include(o => o.ProcurementStatus)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Create()
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        await PopulateSuppliersAsync();
        PopulateStatuses();
        await PopulateItemLookupsAsync();
        return View(new ProcurementOrder
        {
            OrderDateUtc = DateTime.UtcNow,
            OrderNumber = await _procurement.GenerateNextOrderNumberAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Create(
        ProcurementOrder order,
        string? firstItemName,
        int? firstItemQuantity,
        decimal? firstItemUnitCost,
        string? firstItemQualityStandard,
        string? firstItemNotes)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            await PopulateSuppliersAsync();
            PopulateStatuses();
            await PopulateItemLookupsAsync();
            return View(order);
        }

        // ENFORCEMENT LOGIC: Block usage of non-approved or suspended suppliers
        var supplier = await _db.Suppliers.FindAsync(order.SupplierId);
        var canBeUsed = await _performance.CanSupplierBeUsedAsync(order.SupplierId);
        
        if (!canBeUsed)
        {
            var gate = await _procurement.GetOrderApprovalBlockerAsync(new ProcurementOrder { SupplierId = order.SupplierId, Supplier = supplier! }, "create", User.Identity?.Name);
            if (gate.IsBlocked)
            {
                TempData["ErrorMessage"] = gate.Message;
                
                await PopulateSuppliersAsync();
                PopulateStatuses();
                await PopulateItemLookupsAsync();
                return View(order);
            }
        }
        
        if (supplier != null && (supplier.Status == SupplierStatus.Suspended || 
                                     await _db.SupplierComplianceDocuments.AnyAsync(d => d.SupplierId == supplier.Id && (d.DocumentStatus == SupplierDocumentStatus.Expired || d.DocumentStatus == SupplierDocumentStatus.Missing))))
        {
            // If it reached here and has issues, it means override is enabled.
            TempData["SuccessMessage"] = $"Override applied for '{supplier.Name}'.";
        }

        // Always generate on server because the UI field is readonly.
        order.OrderNumber = await _procurement.GenerateNextOrderNumberAsync();
        order.CreatedAtUtc = DateTime.UtcNow;
        order.UpdatedAtUtc = DateTime.UtcNow;

        // Automatically set status to 'Draft' for new orders
        var draftStatusId = await _procurement.GetProcurementStatusIdAsync("Draft");
        if (draftStatusId.HasValue)
        {
            order.ProcurementStatusId = draftStatusId.Value;
        }

        // System-calculated from line items; new orders start at zero.
        order.TotalAmount = 0m;
        
        // Rule 5: Only keep exchange rate if we have items.
        // We'll check this after checking the first item.
        
        _db.ProcurementOrders.Add(order);

        // Optional: allow creating the first line item in the same screen.
        var resolvedFirstItemName = firstItemName?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(resolvedFirstItemName) && firstItemQuantity.HasValue && firstItemUnitCost.HasValue)
        {
            var item = new ProcurementItem
            {
                ProcurementOrderId = order.Id,
                ItemName = resolvedFirstItemName,
                Quantity = Math.Max(1, firstItemQuantity.Value),
                UnitCost = Math.Max(0m, firstItemUnitCost.Value),
                QualityStandard = firstItemQualityStandard?.Trim() ?? string.Empty,
                Notes = firstItemNotes?.Trim() ?? string.Empty
            };

            _db.ProcurementItems.Add(item);
            order.TotalAmount = item.Quantity * item.UnitCost;
        }
        else
        {
            // Rule 5: No items = no rate storage
            order.ExchangeRateUsed = null;
            order.ExchangeRateTimestampUtc = null;
        }

        AddAudit(nameof(ProcurementOrder), order.Id, "Create", $"Created order {order.OrderNumber}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Order {order.OrderNumber} created.";
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var order = await _db.ProcurementOrders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        await PopulateSuppliersAsync();
        PopulateStatuses();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id, ProcurementOrder order)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        if (id != order.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulateSuppliersAsync();
            PopulateStatuses();
            return View(order);
        }

        // ENFORCEMENT LOGIC: Block usage of non-approved or suspended suppliers
        var supplier = await _db.Suppliers.FindAsync(order.SupplierId);
        var canBeUsed = await _performance.CanSupplierBeUsedAsync(order.SupplierId);

        if (!canBeUsed)
        {
            var gate = await _procurement.GetOrderApprovalBlockerAsync(new ProcurementOrder { SupplierId = order.SupplierId, Supplier = supplier! }, "edit", User.Identity?.Name);
            if (gate.IsBlocked)
            {
                TempData["ErrorMessage"] = gate.Message;
                
                await PopulateSuppliersAsync();
                PopulateStatuses();
                return View(order);
            }
        }
        
        if (supplier != null && (supplier.Status == SupplierStatus.Suspended || 
                                     await _db.SupplierComplianceDocuments.AnyAsync(d => d.SupplierId == supplier.Id && (d.DocumentStatus == SupplierDocumentStatus.Expired || d.DocumentStatus == SupplierDocumentStatus.Missing))))
        {
            // If it reached here and has issues, it means override is enabled.
            TempData["SuccessMessage"] = $"Override applied for '{supplier.Name}'.";
        }

        var existing = await _db.ProcurementOrders.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.SupplierId = order.SupplierId;
        existing.OrderDateUtc = order.OrderDateUtc;
        existing.ExchangeRateUsed = order.ExchangeRateUsed;
        existing.ExchangeRateTimestampUtc = order.ExchangeRateTimestampUtc;

        var isOperationalAdmin = User.IsInRole(RoleNames.Admin) && !User.IsInRole(RoleNames.SuperAdmin);
        if (!isOperationalAdmin)
        {
            var draftId = await _procurement.GetProcurementStatusIdAsync("Draft");
            if (draftId.HasValue)
            {
                existing.ProcurementStatusId = draftId.Value;
            }
        }
        else
        {
            existing.ProcurementStatusId = order.ProcurementStatusId;
        }

        var total = await _db.ProcurementItems
            .Where(i => i.ProcurementOrderId == existing.Id && !i.IsDeleted)
            .Select(i => (decimal?)(i.Quantity * i.UnitCost))
            .SumAsync() ?? 0m;
            
        existing.TotalAmount = total;
        
        // Rule 5: If total is 0 (no items), reset exchange rate fields
        if (total == 0)
        {
            existing.ExchangeRateUsed = null;
            existing.ExchangeRateTimestampUtc = null;
        }
        
        existing.UpdatedAtUtc = DateTime.UtcNow;

        AddAudit(nameof(ProcurementOrder), existing.Id, "Edit", $"Updated order {existing.OrderNumber}.");
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var order = await _db.ProcurementOrders.FindAsync(id);
        if (order != null)
        {
            order.IsDeleted = true;
            order.DeletedAtUtc = DateTime.UtcNow;
            order.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(ProcurementOrder), order.Id, "Archive", $"Archived order {order.OrderNumber}.");
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Order {order.OrderNumber} archived.";
        }
        else
        {
            TempData["ErrorMessage"] = "Archive failed: order not found.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Restore(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var order = await _db.ProcurementOrders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        order.IsDeleted = false;
        order.DeletedAtUtc = null;
        order.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(ProcurementOrder), order.Id, "Restore", $"Restored order {order.OrderNumber}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Order {order.OrderNumber} restored.";

        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        // Workflow check
        if (order.ProcurementStatus?.Name != "Draft" && order.ProcurementStatus?.Name != "Rejected")
        {
            TempData["ErrorMessage"] = $"Cannot submit: order is '{order.ProcurementStatus?.Name}'.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var gate = await _procurement.GetOrderApprovalBlockerAsync(order, "submit", User.Identity?.Name);
        if (gate.IsBlocked)
        {
            var actionName = gate.RiskBlockers.Any() ? "Submit Blocked (Risk)" : "Submit Blocked (Documents)";
            AddAudit(nameof(ProcurementOrder), order.Id, actionName, 
                $"Message: {gate.Message}; RiskBlockers: {string.Join(" | ", gate.RiskBlockers)}; DocBlockers: {string.Join(" | ", gate.DocumentBlockers)}");
            await _db.SaveChangesAsync();
            TempData["ErrorMessage"] = gate.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        if (gate.OverrideUsed)
        {
            TempData["ErrorMessage"] = $"SuperAdmin document override applied. Reasons: {gate.OverrideReason}; OverriddenDocs: {string.Join(" | ", gate.OverriddenDocumentBlockers)}; Actor: {User.Identity?.Name}";
        }

        var submittedStatusId = await _procurement.GetProcurementStatusIdAsync("Submitted");
        if (!submittedStatusId.HasValue)
        {
            TempData["ErrorMessage"] = "Submission failed: status not configured.";
            return RedirectToAction(nameof(Details), new { id });
        }

        order.ProcurementStatusId = submittedStatusId.Value;
        order.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(ProcurementOrder), order.Id, "Submit", $"Submitted order {order.OrderNumber} for approval.");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Order {order.OrderNumber} submitted.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        if (!User.IsInRole(RoleNames.Admin)) return Forbid();

        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .Include(o => o.ProcurementStatus)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        // Workflow check
        if (order.ProcurementStatus?.Name != "Submitted")
        {
            TempData["ErrorMessage"] = $"Cannot approve: order must be submitted first.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var gate = await _procurement.GetOrderApprovalBlockerAsync(order, "approve", User.Identity?.Name);
        if (gate.IsBlocked)
        {
            var actionName = gate.RiskBlockers.Any() ? "Approve Blocked (Risk)" : "Approve Blocked (Documents)";
            AddAudit(nameof(ProcurementOrder), order.Id, actionName, 
                $"Message: {gate.Message}; RiskBlockers: {string.Join(" | ", gate.RiskBlockers)}; DocBlockers: {string.Join(" | ", gate.DocumentBlockers)}");
            await _db.SaveChangesAsync();
            TempData["ErrorMessage"] = gate.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        if (gate.OverrideUsed)
        {
            TempData["ErrorMessage"] = $"SuperAdmin document override applied. Reasons: {gate.OverrideReason}; OverriddenDocs: {string.Join(" | ", gate.OverriddenDocumentBlockers)}; Actor: {User.Identity?.Name}";
        }

        var approvedStatusId = await _procurement.GetProcurementStatusIdAsync("Approved");
        if (!approvedStatusId.HasValue)
        {
            TempData["ErrorMessage"] = "Approval failed: status not configured.";
            return RedirectToAction(nameof(Details), new { id });
        }

        order.ProcurementStatusId = approvedStatusId.Value;
        order.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(ProcurementOrder), order.Id, "Approve", $"Approved order {order.OrderNumber}.");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Order {order.OrderNumber} approved.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Approve")]
    public async Task<IActionResult> Reject(Guid id, string reason)
    {
        if (!User.IsInRole(RoleNames.Admin)) return Forbid();

        var order = await _db.ProcurementOrders.FindAsync(id);
        if (order == null) return NotFound();

        var rejectedStatusId = await _procurement.GetProcurementStatusIdAsync("Rejected");
        if (!rejectedStatusId.HasValue) return RedirectToAction(nameof(Details), new { id });

        order.ProcurementStatusId = rejectedStatusId.Value;
        order.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(ProcurementOrder), order.Id, "Reject", $"Rejected order {order.OrderNumber}. Reason: {reason}");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Order {order.OrderNumber} rejected.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.PlaceOrder")]
    public async Task<IActionResult> PlaceOrder(Guid id)
    {
        var order = await _db.ProcurementOrders
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        var gate = await _procurement.GetOrderApprovalBlockerAsync(order, "place", User.Identity?.Name);
        if (gate.IsBlocked)
        {
            var actionName = gate.RiskBlockers.Any() ? "Place Order Blocked (Risk)" : "Place Order Blocked (Documents)";
            AddAudit(nameof(ProcurementOrder), order.Id, actionName, 
                $"Message: {gate.Message}; RiskBlockers: {string.Join(" | ", gate.RiskBlockers)}; DocBlockers: {string.Join(" | ", gate.DocumentBlockers)}");
            await _db.SaveChangesAsync();
            TempData["ErrorMessage"] = gate.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        var orderedStatusId = await _procurement.GetProcurementStatusIdAsync("Ordered");
        if (!orderedStatusId.HasValue) return RedirectToAction(nameof(Details), new { id });

        order.ProcurementStatusId = orderedStatusId.Value;
        order.UpdatedAtUtc = DateTime.UtcNow;

        if (gate.OverrideUsed)
        {
            TempData["ErrorMessage"] = $"SuperAdmin document override applied. Reasons: {gate.OverrideReason}; OverriddenDocs: {string.Join(" | ", gate.OverriddenDocumentBlockers)}; Actor: {User.Identity?.Name}";
        }

        AddAudit(nameof(ProcurementOrder), order.Id, "Place", $"Placed order {order.OrderNumber} with supplier.");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Order {order.OrderNumber} marked as ordered.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private bool IsProcurementOperator()
    {
        return User.IsInRole(RoleNames.Procurement) && !User.IsInRole(RoleNames.SuperAdmin);
    }
    private async Task PopulateSuppliersAsync()
    {
        var suppliers = await _db.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name })
            .ToListAsync();

        ViewBag.SupplierId = new SelectList(suppliers, "Id", "Name");
    }

    private async Task PopulateItemLookupsAsync()
    {
        var rawItems = await _db.ProcurementItems
            .Where(i => !string.IsNullOrWhiteSpace(i.ItemName))
            .Select(i => new
            {
                i.ItemName,
                i.UnitCost,
                i.QualityStandard,
                i.UpdatedAtUtc,
                i.CreatedAtUtc
            })
            .ToListAsync();

        var catalogFromDb = rawItems
            .GroupBy(i => i.ItemName.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g
                .OrderByDescending(i => i.UpdatedAtUtc)
                .ThenByDescending(i => i.CreatedAtUtc)
                .First())
            .Select(i => new
            {
                Name = i.ItemName.Trim(),
                i.UnitCost,
                QualityStandard = i.QualityStandard?.Trim() ?? string.Empty
            })
            .OrderBy(i => i.Name)
            .ToList();

        // Seeded Item Name options (furniture industry) - always available for dropdown
        var seededItems = new[]
        {
            new { Name = "Pine Wood Plank (Standard)", UnitCost = 15.00m, QualityStandard = "ISO 9001" },
            new { Name = "Oak Veneer (Premium)", UnitCost = 45.00m, QualityStandard = "FSC Certified" },
            new { Name = "Teak Solid Wood", UnitCost = 85.00m, QualityStandard = "Legal Timber Certification" },
            new { Name = "Walnut Veneer", UnitCost = 52.00m, QualityStandard = "FSC Certified" },
            new { Name = "MDF Board", UnitCost = 22.00m, QualityStandard = "CARB P2" },
            new { Name = "Plywood (Birch)", UnitCost = 38.00m, QualityStandard = "ISO 9001" },
            new { Name = "Steel Hinge (Heavy Duty)", UnitCost = 5.50m, QualityStandard = "ANSI Grade 1" },
            new { Name = "Drawer Slides", UnitCost = 8.25m, QualityStandard = "REACH Compliant" },
            new { Name = "Brass Handles (Set)", UnitCost = 12.00m, QualityStandard = "ISO 9001" },
            new { Name = "Linen Fabric", UnitCost = 18.00m, QualityStandard = "OEKO-TEX" },
            new { Name = "Leather Upholstery", UnitCost = 45.00m, QualityStandard = "OEKO-TEX" },
            new { Name = "Foam Padding (Medium)", UnitCost = 18.00m, QualityStandard = "CertiPUR-US" },
            new { Name = "Clear Polyurethane", UnitCost = 14.00m, QualityStandard = "Low VOC" },
            new { Name = "Wood Stain - Walnut", UnitCost = 16.00m, QualityStandard = "Low VOC" },
            new { Name = "Epoxy Resin", UnitCost = 28.00m, QualityStandard = "ISO 9001" },
            new { Name = "Aluminum Frame (Anodized)", UnitCost = 32.50m, QualityStandard = "ASTM B221" },
            new { Name = "Tempered Glass (8mm)", UnitCost = 28.00m, QualityStandard = "ISO 20400" },
            new { Name = "Corrugated Boxes", UnitCost = 3.50m, QualityStandard = "ISO 9001" },
            new { Name = "Bubble Wrap (Roll)", UnitCost = 12.00m, QualityStandard = "Recyclable" },
            new { Name = "Wood Glue", UnitCost = 6.50m, QualityStandard = "Low VOC" },
            new { Name = "Screws & Bolts (Assorted)", UnitCost = 4.25m, QualityStandard = "ANSI" }
        };

        // Merge: DB catalog first (user's historical items), then seeded items not already present
        var catalog = catalogFromDb.ToList();
        var existingNames = new HashSet<string>(catalog.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
        foreach (var item in seededItems)
        {
            if (!existingNames.Contains(item.Name))
            {
                catalog.Add(item);
                existingNames.Add(item.Name);
            }
        }
        catalog = catalog.OrderBy(x => x.Name).ToList();

        ViewBag.ItemNameOptions = catalog.Select(x => x.Name).ToList();
        
        var qualityStandards = catalog
            .Where(x => !string.IsNullOrWhiteSpace(x.QualityStandard))
            .Select(x => x.QualityStandard)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
            
        if (!qualityStandards.Any())
        {
            qualityStandards.AddRange(new[] { "ISO 9001", "FSC Certified", "ANSI Grade 1", "OEKO-TEX", "Low VOC", "Legal Timber Certification", "ASTM B221", "ISO 20400", "CertiPUR-US", "REACH Compliant" });
        }
        else if (!qualityStandards.Contains("ISO 9001"))
        {
            qualityStandards.AddRange(new[] { "ISO 9001", "FSC Certified", "ANSI Grade 1", "OEKO-TEX", "Low VOC", "Legal Timber Certification", "ASTM B221", "ISO 20400", "CertiPUR-US", "REACH Compliant" });
        }
        
        ViewBag.QualityStandardOptions = qualityStandards;
        ViewBag.ItemCatalogJson = JsonSerializer.Serialize(catalog);
    }

    private void PopulateStatuses()
    {
        var statuses = _db.ProcurementStatuses
            .OrderBy(s => s.SortOrder)
            .Select(s => new { s.Id, s.Name })
            .ToList();
        ViewBag.ProcurementStatusId = new SelectList(statuses, "Id", "Name");
    }

    private static bool NotesMatchSupplierOrOrder(string? notes, string supplierName, string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            return false;
        }

        var normalizedNotes = NormalizeComparisonText(notes);
        var normalizedSupplier = NormalizeComparisonText(supplierName);
        var normalizedOrder = NormalizeComparisonText(orderNumber);

        if (!string.IsNullOrWhiteSpace(normalizedOrder) && normalizedNotes.Contains(normalizedOrder))
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(normalizedSupplier) && normalizedNotes.Contains(normalizedSupplier))
        {
            return true;
        }

        return false;
    }

    private static string NormalizeComparisonText(string value)
    {
        return new string(value
            .ToLowerInvariant()
            .Where(char.IsLetterOrDigit)
            .ToArray());
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
