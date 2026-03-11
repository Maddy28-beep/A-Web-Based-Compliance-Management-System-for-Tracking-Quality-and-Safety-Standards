using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Procurement.Read")]
public class ProcurementItemsController : Controller
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public ProcurementItemsController(AppDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Create(Guid? orderId)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        ViewBag.LockOrderSelection = orderId.HasValue;
        await PopulateOrdersAsync(orderId);
        await PopulateSuggestionsAsync();
        return View(new ProcurementItem
        {
            ProcurementOrderId = orderId ?? Guid.Empty
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Create(ProcurementItem item, string? customItemName)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        item.Notes ??= string.Empty;
        item.ItemName = ResolveItemName(item.ItemName, customItemName);
        ModelState.Remove(nameof(ProcurementItem.Notes));
        if (string.IsNullOrWhiteSpace(item.ItemName))
        {
            ModelState.AddModelError(nameof(ProcurementItem.ItemName), "Item name is required.");
        }

        // Rule 4: Block if Quantity or UnitCost <= 0
        if (item.Quantity <= 0)
        {
            ModelState.AddModelError(nameof(ProcurementItem.Quantity), "Quantity must be greater than 0.");
        }
        if (item.UnitCost <= 0)
        {
            ModelState.AddModelError(nameof(ProcurementItem.UnitCost), "Unit Cost must be greater than 0.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateOrdersAsync(item.ProcurementOrderId);
            await PopulateSuggestionsAsync();
            return View(item);
        }

        _db.ProcurementItems.Add(item);
        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(item.ProcurementOrderId);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", "ProcurementOrders", new { id = item.ProcurementOrderId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        var item = await _db.ProcurementItems.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await PopulateOrdersAsync(item.ProcurementOrderId);
        await PopulateSuggestionsAsync();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id, ProcurementItem item, string? customItemName)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        if (id != item.Id)
        {
            return BadRequest();
        }

        item.Notes ??= string.Empty;
        item.ItemName = ResolveItemName(item.ItemName, customItemName);
        ModelState.Remove(nameof(ProcurementItem.Notes));
        if (string.IsNullOrWhiteSpace(item.ItemName))
        {
            ModelState.AddModelError(nameof(ProcurementItem.ItemName), "Item name is required.");
        }

        // Rule 4: Block if Quantity or UnitCost <= 0
        if (item.Quantity <= 0)
        {
            ModelState.AddModelError(nameof(ProcurementItem.Quantity), "Quantity must be greater than 0.");
        }
        if (item.UnitCost <= 0)
        {
            ModelState.AddModelError(nameof(ProcurementItem.UnitCost), "Unit Cost must be greater than 0.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateOrdersAsync(item.ProcurementOrderId);
            await PopulateSuggestionsAsync();
            return View(item);
        }

        var existing = await _db.ProcurementItems.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.ProcurementOrderId = item.ProcurementOrderId;
        existing.ItemName = item.ItemName;
        existing.Quantity = item.Quantity;
        existing.UnitCost = item.UnitCost;
        existing.QualityStandard = item.QualityStandard;
        existing.Notes = item.Notes;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(item.ProcurementOrderId);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", "ProcurementOrders", new { id = item.ProcurementOrderId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        var item = await _db.ProcurementItems
            .Include(i => i.ProcurementOrder)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        // Explicitly block Admin from operational procurement tasks
        if (User.IsInRole(RoleNames.Admin)) return Forbid();

        var item = await _db.ProcurementItems.FindAsync(id);
        if (item != null)
        {
            var orderId = item.ProcurementOrderId;
            item.IsDeleted = true;
            item.DeletedAtUtc = DateTime.UtcNow;
            item.UpdatedAtUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await RecalculateOrderTotalAsync(orderId);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "ProcurementOrders", new { id = orderId });
        }

        return RedirectToAction("Index", "ProcurementOrders");
    }

    private async Task PopulateOrdersAsync(Guid? selectedId)
    {
        var orders = await _db.ProcurementOrders
            .OrderByDescending(o => o.OrderDateUtc)
            .Select(o => new { o.Id, o.OrderNumber })
            .ToListAsync();

        ViewBag.ProcurementOrderId = new SelectList(orders, "Id", "OrderNumber", selectedId);
    }

    private async Task PopulateSuggestionsAsync()
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

        var catalog = rawItems
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
            .Distinct()
            .OrderBy(x => x.Name)
            .Take(30)
            .ToList();

        if (!catalog.Any())
        {
            catalog.AddRange(new[]
            {
                new { Name = "Pine Wood Plank (Standard)", UnitCost = 15.00m, QualityStandard = "ISO 9001" },
                new { Name = "Oak Veneer (Premium)", UnitCost = 45.00m, QualityStandard = "FSC Certified" },
                new { Name = "Steel Hinge (Heavy Duty)", UnitCost = 5.50m, QualityStandard = "ANSI Grade 1" },
                new { Name = "Fabric Upholstery (Velvet)", UnitCost = 25.00m, QualityStandard = "OEKO-TEX" },
                new { Name = "Varnish (Eco-Friendly)", UnitCost = 12.00m, QualityStandard = "Low VOC" },
                new { Name = "Teak Lumber (A-Grade)", UnitCost = 85.00m, QualityStandard = "Legal Timber Certification" },
                new { Name = "Aluminum Frame (Anodized)", UnitCost = 32.50m, QualityStandard = "ASTM B221" },
                new { Name = "Tempered Glass (8mm)", UnitCost = 28.00m, QualityStandard = "ISO 20400" },
                new { Name = "Polyurethane Foam (Medium)", UnitCost = 18.00m, QualityStandard = "CertiPUR-US" },
                new { Name = "Nylon Glides (Set of 4)", UnitCost = 2.75m, QualityStandard = "REACH Compliant" }
            });
        }

        ViewBag.ItemNameSuggestions = catalog.Select(x => x.Name).ToList();
        
        var qualityStandards = catalog
            .Where(x => !string.IsNullOrWhiteSpace(x.QualityStandard))
            .Select(x => x.QualityStandard)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        if (!qualityStandards.Contains("ISO 9001"))
        {
            qualityStandards.AddRange(new[] { "ISO 9001", "FSC Certified", "ANSI Grade 1", "OEKO-TEX", "Low VOC", "Legal Timber Certification", "ASTM B221", "ISO 20400", "CertiPUR-US", "REACH Compliant" });
            qualityStandards = qualityStandards.Distinct().OrderBy(x => x).ToList();
        }

        ViewBag.QualityStandardSuggestions = qualityStandards;
        ViewBag.ItemCatalogJson = JsonSerializer.Serialize(catalog);
    }

    private async Task RecalculateOrderTotalAsync(Guid orderId)
    {
        var order = await _db.ProcurementOrders.FindAsync(orderId);
        if (order == null)
        {
            return;
        }

        var total = await _db.ProcurementItems
            .Where(i => i.ProcurementOrderId == orderId && !i.IsDeleted)
            .Select(i => (decimal?)(i.Quantity * i.UnitCost))
            .SumAsync() ?? 0m;

        order.TotalAmount = total;
        
        // Rule 5: If total is 0 (no items), reset exchange rate fields
        if (total == 0)
        {
            order.ExchangeRateUsed = null;
            order.ExchangeRateTimestampUtc = null;
        }
        else if (order.ExchangeRateUsed == null)
        {
            // First item added, capture the rate
            await CaptureExchangeRateAsync(order);
        }
        
        order.UpdatedAtUtc = DateTime.UtcNow;
    }

    private async Task CaptureExchangeRateAsync(ProcurementOrder order)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<ErApiResponse>("https://open.er-api.com/v6/latest/USD");
            if (response?.Rates != null && string.Equals(response.Result, "success", StringComparison.OrdinalIgnoreCase))
            {
                if (response.Rates.TryGetValue("PHP", out var rate))
                {
                    order.ExchangeRateUsed = (decimal)rate;
                    order.ExchangeRateTimestampUtc = DateTime.UtcNow;
                }
            }
        }
        catch
        {
            // Best effort, don't crash the save
        }
    }

    private sealed class ErApiResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; } = string.Empty;

        [JsonPropertyName("rates")]
        public Dictionary<string, double>? Rates { get; set; }
    }

    private static string ResolveItemName(string? selectedName, string? customItemName)
    {
        if (string.Equals(selectedName, "__new__", StringComparison.OrdinalIgnoreCase))
        {
            return customItemName?.Trim() ?? string.Empty;
        }

        return selectedName?.Trim() ?? string.Empty;
    }
}
