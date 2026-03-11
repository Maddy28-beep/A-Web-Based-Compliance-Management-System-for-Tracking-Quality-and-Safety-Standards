using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FurniComply.Application.Interfaces;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Web.Models;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurniComply.Web.Services;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Procurement.Read")]
public class SuppliersController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _environment;
    private readonly ISupplierPerformanceService _performance;
    private readonly ISupplierComplianceScoreService _complianceScore;
    private readonly ISupplierManagementService _supplierService;

    public SuppliersController(
        AppDbContext db,
        IWebHostEnvironment environment,
        ISupplierPerformanceService performance,
        ISupplierComplianceScoreService complianceScore,
        ISupplierManagementService supplierService)
    {
        _db = db;
        _environment = environment;
        _performance = performance;
        _complianceScore = complianceScore;
        _supplierService = supplierService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = RoleNames.Procurement)]
    public async Task<IActionResult> EvaluatePerformance(Guid supplierId, string? productName, Guid? productId, int qualityRating, int deliveryRating, decimal defectRate, string remarks)
    {
        await _performance.EvaluatePerformanceAsync(
            supplierId,
            qualityRating,
            deliveryRating,
            defectRate,
            remarks,
            User.Identity?.Name ?? "system",
            productName,
            productId);

        TempData["SuccessMessage"] = $"Performance evaluation for '{productName ?? "Product"}' recorded.";
        return RedirectToAction(nameof(Details), new { id = supplierId });
    }

    private bool IsProcurementOperator() =>
        User.IsInRole(RoleNames.Procurement);

    public async Task<IActionResult> Index(SupplierApprovalStatus? approvalStatus, SupplierStatus? status)
    {
        var query = _db.Suppliers
            .Include(s => s.SupplierCategory)
            .AsQueryable();

        if (approvalStatus.HasValue)
        {
            query = query.Where(s => s.ApprovalStatus == approvalStatus.Value);
            ViewBag.ApprovalStatusFilter = approvalStatus.Value.ToString();
        }

        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status.Value);
            ViewBag.StatusFilter = status.Value.ToString();
        }

        var suppliers = await query
            .OrderByDescending(s => s.UpdatedAtUtc)
            .ThenByDescending(s => s.CreatedAtUtc)
            .ToListAsync();

        return View(suppliers);
    }

    public async Task<IActionResult> Archived()
    {
        var suppliers = await _db.Suppliers
            .IgnoreQueryFilters()
            .Where(s => s.IsDeleted)
            .Include(s => s.SupplierCategory)
            .OrderByDescending(s => s.DeletedAtUtc)
            .ThenBy(s => s.Name)
            .ToListAsync();

        return View(suppliers);
    }

    public async Task<IActionResult> Documents(Guid? supplierId, SupplierDocumentStatus? status, string? documentType, string? search, bool expiring = false, int page = 1, int pageSize = 10)
    {
        var allowedSizes = new[] { 3, 10, 25, 50 };
        pageSize = allowedSizes.Contains(pageSize) ? pageSize : 10;
        page = Math.Max(1, page);

        var now = DateTime.UtcNow;
        var expiringThreshold = now.AddDays(30);

        var query = _db.SupplierComplianceDocuments
            .Include(d => d.Supplier)
            .AsQueryable();

        if (supplierId.HasValue)
        {
            query = query.Where(d => d.SupplierId == supplierId.Value);
        }

        if (expiring)
        {
            // Documents expiring within 30 days or already expired
            query = query.Where(d =>
                d.DocumentStatus == SupplierDocumentStatus.Expired ||
                (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value <= expiringThreshold));
        }

        if (status.HasValue)
        {
            query = query.Where(d => d.DocumentStatus == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(documentType))
        {
            query = query.Where(d => d.DocumentType.Contains(documentType));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d =>
                d.DocumentType.Contains(search) ||
                (d.Supplier != null && d.Supplier.Name.Contains(search)));
        }

        var supplierIdsWithDates = await query
            .GroupBy(d => d.SupplierId)
            .Select(g => new { SupplierId = g.Key, MaxDate = g.Max(d => d.UpdatedAtUtc ?? d.CreatedAtUtc) })
            .OrderByDescending(x => x.MaxDate)
            .ToListAsync();

        var totalSupplierCount = supplierIdsWithDates.Count;
        var pagedIds = supplierIdsWithDates
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.SupplierId)
            .ToList();

        var documents = pagedIds.Count > 0
            ? await query
                .Where(d => pagedIds.Contains(d.SupplierId))
                .OrderByDescending(d => d.UpdatedAtUtc ?? d.CreatedAtUtc)
                .ToListAsync()
            : new List<SupplierComplianceDocument>();

        var viewModel = new SupplierDocumentsIndexViewModel
        {
            SupplierId = supplierId,
            Status = status,
            DocumentType = documentType,
            Search = search,
            Expiring = expiring,
            Documents = documents,
            Suppliers = await _db.Suppliers
                .OrderBy(s => s.Name)
                .ToListAsync(),
            Page = page,
            PageSize = pageSize,
            TotalSupplierCount = totalSupplierCount
        };

        viewModel.DocumentTypes = await _db.SupplierComplianceDocuments
            .Select(d => d.DocumentType)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(Guid id, int timelineTake = 20, int timelineSkip = 0, string? timelineType = null, string? timelineSeverity = null)
    {
        timelineTake = Math.Clamp(timelineTake, 5, 100);
        timelineSkip = Math.Max(0, timelineSkip);

        var supplier = await _db.Suppliers
            .IgnoreQueryFilters()
            .Include(s => s.SupplierCategory)
            .Include(s => s.ComplianceDocuments)
            .Include(s => s.RiskLevel)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            
        if (supplier == null)
        {
            return NotFound();
        }

        ViewBag.Performances = await _db.SupplierPerformances
            .Where(p => p.SupplierId == id)
            .OrderByDescending(p => p.EvaluationDateUtc)
            .ToListAsync();

        var supplierProducts = await _db.ProcurementItems
            .Include(pi => pi.ProcurementOrder)
            .Where(pi => pi.ProcurementOrder != null && pi.ProcurementOrder.SupplierId == id)
            .Select(pi => pi.ItemName)
            .Distinct()
            .OrderBy(n => n)
            .ToListAsync();

        // Seeded Product/Material options for dropdown (furniture industry)
        var seededProducts = new List<string>
        {
            "General Supply",
            "Teak Solid Wood", "Walnut Veneer", "Oak Planks", "Mahogany", "Pine Lumber", "MDF Board", "Plywood", "Particle Board",
            "Linen Fabric", "Cotton Upholstery", "Leather", "Synthetic Leather", "Foam Padding", "Spring Coils",
            "Steel Hinges", "Magnetic Catches", "Drawer Slides", "Brass Handles", "Screws & Bolts", "Cabinet Hardware",
            "Clear Polyurethane", "Wood Stain - Walnut", "Wood Stain - Oak", "Epoxy Resin", "Sandpaper Grit 200", "Wax Polish", "Lacquer",
            "Corrugated Boxes", "Bubble Wrap", "Corner Protectors", "Stretch Film", "Shipping Pallets", "Cardboard",
            "Glass Panels", "Mirror Glass", "Tempered Glass",
            "Metal Brackets", "Wood Glue", "Wood Filler", "Veneer Tape"
        };

        // Add category-based suggestions if list is small or empty
        var suggestedProducts = new List<string>();
        if (supplier.SupplierCategory != null)
        {
            suggestedProducts = supplier.SupplierCategory.Name switch
            {
                "Raw Materials" => new List<string> { "Teak Solid Wood", "Walnut Veneer", "Oak Planks", "Linen Fabric", "Foam Padding" },
                "Hardware" => new List<string> { "Steel Hinges", "Magnetic Catches", "Drawer Slides", "Brass Handles", "Screws & Bolts" },
                "Finishing" => new List<string> { "Clear Polyurethane", "Wood Stain - Walnut", "Epoxy Resin", "Sandpaper Grit 200", "Wax Polish" },
                "Packaging" => new List<string> { "Corrugated Boxes", "Bubble Wrap", "Corner Protectors", "Stretch Film", "Shipping Pallets" },
                _ => new List<string>()
            };
        }

        var combinedProducts = supplierProducts
            .Union(seededProducts)
            .Union(suggestedProducts)
            .Distinct()
            .OrderBy(n => n)
            .ToList();
        ViewBag.SupplierProducts = combinedProducts;

        ViewBag.CorrectiveActions = await _db.CorrectiveActions
            .Where(ca => ca.SupplierId == id || 
                         (_db.ComplianceChecks.Any(cc => cc.Id == ca.ComplianceCheckId && cc.Notes.Contains(supplier.Name))))
            .OrderByDescending(ca => ca.CreatedAtUtc)
            .ToListAsync();

        ViewBag.ComplianceScore = await _complianceScore.GetSupplierComplianceScoreAsync(id);
        ViewBag.Evaluations = await _db.SupplierEvaluations
            .Where(e => e.SupplierId == id)
            .OrderByDescending(e => e.EvaluatedAtUtc)
            .ToListAsync();
        ViewBag.OverrideAudits = await _db.AuditLogs
            .Where(a =>
                a.EntityName == nameof(Supplier) &&
                a.EntityId == id &&
                (a.Action == "OverrideEnabled" || a.Action == "OverrideDisabled"))
            .OrderByDescending(a => a.TimestampUtc)
            .ToListAsync();

        var timelineAll = await _supplierService.BuildSupplierTimelineAsync(id, supplier.Name, (action, controller, values) => Url.Action(action, controller, values));
        var timelineFiltered = timelineAll
            .Where(t => string.IsNullOrWhiteSpace(timelineType) || t.EventType.Equals(timelineType, StringComparison.OrdinalIgnoreCase))
            .Where(t => string.IsNullOrWhiteSpace(timelineSeverity) || t.Severity.Equals(timelineSeverity, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(t => t.DateUtc)
            .ToList();

        ViewBag.TimelineTotal = timelineFiltered.Count;
        ViewBag.TimelineTake = timelineTake;
        ViewBag.TimelineSkip = timelineSkip;
        ViewBag.TimelineType = timelineType ?? string.Empty;
        ViewBag.TimelineSeverity = timelineSeverity ?? string.Empty;
        ViewBag.TimelineItems = timelineFiltered.Skip(timelineSkip).Take(timelineTake).ToList();

        return View(supplier);
    }

    [HttpGet("/api/suppliers/{id:guid}/compliance-score")]
    public async Task<IActionResult> GetComplianceScore(Guid id)
    {
        var score = await _complianceScore.GetSupplierComplianceScoreAsync(id);
        if (score == null)
        {
            return NotFound();
        }

        return Ok(score);
    }

    [HttpGet("/api/suppliers/{id:guid}/timeline")]
    public async Task<IActionResult> GetTimeline(Guid id, int take = 20, int skip = 0, string? type = null, string? severity = null)
    {
        take = Math.Clamp(take, 5, 100);
        skip = Math.Max(0, skip);

        var supplier = await _db.Suppliers
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new { s.Id, s.Name })
            .FirstOrDefaultAsync();
        if (supplier == null)
        {
            return NotFound();
        }

        var all = await _supplierService.BuildSupplierTimelineAsync(id, supplier.Name, (action, controller, values) => Url.Action(action, controller, values));
        var filtered = all
            .Where(t => string.IsNullOrWhiteSpace(type) || t.EventType.Equals(type, StringComparison.OrdinalIgnoreCase))
            .Where(t => string.IsNullOrWhiteSpace(severity) || t.Severity.Equals(severity, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(t => t.DateUtc)
            .ToList();

        return Ok(new
        {
            total = filtered.Count,
            items = filtered.Skip(skip).Take(take)
        });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentCreate(Guid supplierId, string? documentType)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var supplier = await _db.Suppliers.FindAsync(supplierId);
        if (supplier == null)
        {
            return NotFound();
        }

        PopulateDocumentTypes(documentType);
        return View(new SupplierComplianceDocument
        {
            SupplierId = supplierId,
            DocumentType = documentType ?? string.Empty
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentCreate(SupplierComplianceDocument document, List<IFormFile>? uploads)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        document.Notes ??= string.Empty;
        ModelState.Remove(nameof(SupplierComplianceDocument.FileUrl));
        ModelState.Remove(nameof(SupplierComplianceDocument.Notes));

        if (!ModelState.IsValid)
        {
            PopulateDocumentTypes(document.DocumentType);
            return View(document);
        }

        var files = (uploads ?? new List<IFormFile>())
            .Where(f => f != null && f.Length > 0)
            .Take(1)
            .ToList();

        if ((uploads?.Count ?? 0) > 1)
        {
            ModelState.AddModelError(string.Empty, "Upload one file per document type to avoid duplicates.");
            PopulateDocumentTypes(document.DocumentType);
            return View(document);
        }

        var normalizedType = document.DocumentType.Trim();
        var existingForType = await _db.SupplierComplianceDocuments
            .Where(d => d.SupplierId == document.SupplierId && d.DocumentType == normalizedType)
            .OrderByDescending(d => d.UpdatedAtUtc)
            .ThenByDescending(d => d.CreatedAtUtc)
            .FirstOrDefaultAsync();

        if (existingForType != null)
        {
            var currentFileUrl = existingForType.FileUrl;
            existingForType.ExpiryDateUtc = document.ExpiryDateUtc;
            existingForType.Notes = document.Notes;
            existingForType.UpdatedAtUtc = DateTime.UtcNow;
            if (files.Count == 1)
            {
                existingForType.FileUrl = await _supplierService.SaveDocumentFileAsync(files[0], existingForType.FileUrl, _environment.WebRootPath);
            }
            else
            {
                existingForType.FileUrl = currentFileUrl;
            }

            existingForType.DocumentStatus = ResolveDocumentStatus(existingForType.FileUrl, existingForType.ExpiryDateUtc);

            AddAudit(nameof(SupplierComplianceDocument), existingForType.Id, "Edit", $"Updated document {existingForType.DocumentType}.");
            await _db.SaveChangesAsync();
            await _supplierService.UpdateSupplierRenewalDueUtcAsync(existingForType.SupplierId);
            
            // Re-evaluate risk status after document update
            await _performance.UpdateSupplierRiskStatusAsync(existingForType.SupplierId);
            
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Document '{existingForType.DocumentType}' updated.";
            return RedirectToAction(nameof(Details), new { id = existingForType.SupplierId });
        }

        document.DocumentType = normalizedType;

        if (files.Count == 1)
        {
            document.FileUrl = await _supplierService.SaveDocumentFileAsync(files[0], null, _environment.WebRootPath);
        }
        document.DocumentStatus = ResolveDocumentStatus(document.FileUrl, document.ExpiryDateUtc);

        _db.SupplierComplianceDocuments.Add(document);
        AddAudit(nameof(SupplierComplianceDocument), document.Id, "Create", $"Added document {document.DocumentType}.");
        await _db.SaveChangesAsync();
        await _supplierService.UpdateSupplierRenewalDueUtcAsync(document.SupplierId);
        
        // Re-evaluate risk status after document creation
        await _performance.UpdateSupplierRiskStatusAsync(document.SupplierId);
        
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = document.SupplierId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> UploadRequiredDocuments(
        Guid supplierId,
        IFormFile? businessPermitUpload,
        DateTime? businessPermitExpiryUtc,
        IFormFile? environmentalCertificateUpload,
        DateTime? environmentalCertificateExpiryUtc,
        IFormFile? sustainabilityDeclarationUpload,
        DateTime? sustainabilityDeclarationExpiryUtc)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var supplier = await _db.Suppliers.FindAsync(supplierId);
        if (supplier == null)
        {
            return NotFound();
        }

        var touchedCount = 0;
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplierId, "Business Permit", businessPermitUpload, businessPermitExpiryUtc, _environment.WebRootPath, User.Identity?.Name);
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplierId, "Environmental Certificate", environmentalCertificateUpload, environmentalCertificateExpiryUtc, _environment.WebRootPath, User.Identity?.Name);
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplierId, "Sustainability Declaration", sustainabilityDeclarationUpload, sustainabilityDeclarationExpiryUtc, _environment.WebRootPath, User.Identity?.Name);

        if (touchedCount == 0)
        {
            TempData["WarningMessage"] = "No document changes were submitted.";
            return RedirectToAction(nameof(Details), new { id = supplierId });
        }

        AddAudit(nameof(Supplier), supplierId, "BulkDocumentUpload", $"Uploaded/updated {touchedCount} compliance document(s) for supplier {supplier.Name}.");
        await _db.SaveChangesAsync();
        await _supplierService.UpdateSupplierRenewalDueUtcAsync(supplierId);
        
        // Re-evaluate risk status after bulk document upload
        await _performance.UpdateSupplierRiskStatusAsync(supplierId);
        
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Documents saved.";
        return RedirectToAction(nameof(Details), new { id = supplierId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentEdit(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var document = await _db.SupplierComplianceDocuments.FindAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        PopulateDocumentTypes(document.DocumentType);
        return View(document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentEdit(Guid id, SupplierComplianceDocument document, List<IFormFile>? uploads)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        if (id != document.Id)
        {
            return BadRequest();
        }

        document.Notes ??= string.Empty;
        ModelState.Remove(nameof(SupplierComplianceDocument.FileUrl));
        ModelState.Remove(nameof(SupplierComplianceDocument.Notes));

        if (!ModelState.IsValid)
        {
            PopulateDocumentTypes(document.DocumentType);
            return View(document);
        }

        var existing = await _db.SupplierComplianceDocuments
            .FirstOrDefaultAsync(d => d.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        var duplicateTypeExists = await _db.SupplierComplianceDocuments.AnyAsync(d =>
            d.SupplierId == document.SupplierId &&
            d.Id != document.Id &&
            d.DocumentType == document.DocumentType);
        if (duplicateTypeExists)
        {
            ModelState.AddModelError(nameof(SupplierComplianceDocument.DocumentType),
                "A document of this type already exists for this supplier. Edit that record instead.");
            PopulateDocumentTypes(document.DocumentType);
            return View(document);
        }

        var files = (uploads ?? new List<IFormFile>())
            .Where(f => f != null && f.Length > 0)
            .Take(1)
            .ToList();

        if ((uploads?.Count ?? 0) > 1)
        {
            ModelState.AddModelError(string.Empty, "Upload one file per document type to avoid duplicates.");
            PopulateDocumentTypes(document.DocumentType);
            return View(document);
        }

        if (files.Count > 0)
        {
            // Replace the current document file with the first uploaded file.
            existing.FileUrl = await _supplierService.SaveDocumentFileAsync(files[0], existing.FileUrl, _environment.WebRootPath);
        }

        existing.DocumentType = document.DocumentType;
        existing.ExpiryDateUtc = document.ExpiryDateUtc;
        existing.Notes = document.Notes;
        existing.DocumentStatus = ResolveDocumentStatus(existing.FileUrl, existing.ExpiryDateUtc);
        existing.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(SupplierComplianceDocument), existing.Id, "Edit", $"Updated document {existing.DocumentType}.");

        await _db.SaveChangesAsync();
        await _supplierService.UpdateSupplierRenewalDueUtcAsync(existing.SupplierId);
        
        // Re-evaluate risk status after document edit
        await _performance.UpdateSupplierRiskStatusAsync(existing.SupplierId);
        
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = existing.SupplierId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentDelete(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var document = await _db.SupplierComplianceDocuments
            .Include(d => d.Supplier)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null)
        {
            return NotFound();
        }

        return View(document);
    }

    [HttpPost, ActionName("DocumentDelete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentDeleteConfirmed(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var document = await _db.SupplierComplianceDocuments.FindAsync(id);
        if (document != null)
        {
            var supplierId = document.SupplierId;
            document.IsDeleted = true;
            document.DeletedAtUtc = DateTime.UtcNow;
            document.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(SupplierComplianceDocument), document.Id, "Archive", $"Archived document {document.DocumentType}.");
            await _db.SaveChangesAsync();
            
            // Re-evaluate risk status after document archival
            await _performance.UpdateSupplierRiskStatusAsync(supplierId);
            
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Document '{document.DocumentType}' archived.";
            return RedirectToAction(nameof(Details), new { id = supplierId });
        }

        TempData["ErrorMessage"] = "Archive failed: document not found.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> DocumentRestore(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var document = await _db.SupplierComplianceDocuments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null)
        {
            return NotFound();
        }

        document.IsDeleted = false;
        document.DeletedAtUtc = null;
        document.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(SupplierComplianceDocument), document.Id, "Restore", $"Restored document {document.DocumentType}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Document '{document.DocumentType}' restored.";

        return RedirectToAction(nameof(Details), new { id = document.SupplierId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public IActionResult Create()
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        PopulateCategories();
        PopulateCertificationOptions(Array.Empty<string>());
        return View(new Supplier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Create(
        Supplier supplier,
        string[] certificationSelections,
        bool submitAsPending = false,
        IFormFile? businessPermitUpload = null,
        DateTime? businessPermitExpiryUtc = null,
        IFormFile? environmentalCertificateUpload = null,
        DateTime? environmentalCertificateExpiryUtc = null,
        IFormFile? sustainabilityDeclarationUpload = null,
        DateTime? sustainabilityDeclarationExpiryUtc = null)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        supplier.Certifications = NormalizeCertifications(supplier.Certifications, certificationSelections);
        supplier.Status = SupplierStatus.Active;
        supplier.Rating = 0;
        supplier.PerformanceScore = 0;
        supplier.RenewalDueUtc = null;
        supplier.LastReviewUtc = null;
        var isApprover =
            !User.IsInRole(RoleNames.SuperAdmin) &&
            User.IsInRole(RoleNames.Admin);
        var shouldBePending = !isApprover || submitAsPending;
        supplier.ApprovalStatus = shouldBePending ? SupplierApprovalStatus.Pending : SupplierApprovalStatus.Approved;
        supplier.ApprovedBy = shouldBePending ? null : (User.Identity?.Name ?? RoleNames.Admin);
        supplier.ApprovedOnUtc = shouldBePending ? null : DateTime.UtcNow;

        if (!ModelState.IsValid)
        {
            PopulateCategories();
            PopulateCertificationOptions(certificationSelections);
            TempData["ErrorMessage"] = "Please review highlighted fields and submit again.";
            return View(supplier);
        }

        _db.Suppliers.Add(supplier);

        var defaultDocuments = new List<SupplierComplianceDocument>
        {
            new()
            {
                SupplierId = supplier.Id,
                DocumentType = "Business Permit",
                DocumentStatus = SupplierDocumentStatus.Missing,
                Notes = "Upload required."
            },
            new()
            {
                SupplierId = supplier.Id,
                DocumentType = "Environmental Certificate",
                DocumentStatus = SupplierDocumentStatus.Missing,
                Notes = "Upload required."
            },
            new()
            {
                SupplierId = supplier.Id,
                DocumentType = "Sustainability Declaration",
                DocumentStatus = SupplierDocumentStatus.Missing,
                Notes = "Upload required."
            }
        };

        _db.SupplierComplianceDocuments.AddRange(defaultDocuments);
        AddAudit(nameof(Supplier), supplier.Id, "Create", $"Created supplier {supplier.Name}.");
        await _db.SaveChangesAsync();

        var touchedCount = 0;
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplier.Id, "Business Permit", businessPermitUpload, businessPermitExpiryUtc, _environment.WebRootPath, User.Identity?.Name);
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplier.Id, "Environmental Certificate", environmentalCertificateUpload, environmentalCertificateExpiryUtc, _environment.WebRootPath, User.Identity?.Name);
        touchedCount += await _supplierService.UpsertRequiredDocumentAsync(supplier.Id, "Sustainability Declaration", sustainabilityDeclarationUpload, sustainabilityDeclarationExpiryUtc, _environment.WebRootPath, User.Identity?.Name);
        if (touchedCount > 0)
        {
            await _db.SaveChangesAsync();
            await _supplierService.UpdateSupplierRenewalDueUtcAsync(supplier.Id);
            await _performance.UpdateSupplierRiskStatusAsync(supplier.Id);
            await _db.SaveChangesAsync();
        }

        TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' created." + (touchedCount > 0 ? $" {touchedCount} document(s) uploaded." : "");
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        PopulateCategories();
        PopulateCertificationOptions(ParseCertifications(supplier.Certifications));
        return View(supplier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Edit(Guid id, Supplier supplier, string[] certificationSelections)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        if (id != supplier.Id)
        {
            return BadRequest();
        }

        supplier.Certifications = NormalizeCertifications(supplier.Certifications, certificationSelections);

        if (!ModelState.IsValid)
        {
            PopulateCategories();
            PopulateCertificationOptions(certificationSelections);
            TempData["ErrorMessage"] = "Please review highlighted fields and submit again.";
            return View(supplier);
        }

        var existing = await _db.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        if (existing.ApprovalStatus == SupplierApprovalStatus.Pending && supplier.Status == SupplierStatus.OnHold)
        {
            ModelState.AddModelError(nameof(Supplier.Status), "Cannot put a supplier On Hold while approval is still pending.");
            PopulateCategories();
            PopulateCertificationOptions(certificationSelections);
            return View(supplier);
        }

        existing.Name = supplier.Name;
        existing.SupplierCategoryId = supplier.SupplierCategoryId;
        existing.ContactEmail = supplier.ContactEmail;
        existing.Address = supplier.Address;
        existing.Status = supplier.Status;
        existing.Certifications = supplier.Certifications;
        existing.RenewalDueUtc = supplier.RenewalDueUtc;
        existing.LastReviewUtc = supplier.LastReviewUtc;
        var canSetApprovalStatus =
            !User.IsInRole(RoleNames.SuperAdmin) &&
            User.IsInRole(RoleNames.Admin);
        if (canSetApprovalStatus)
        {
            var previousStatus = existing.ApprovalStatus;
            existing.ApprovalStatus = supplier.ApprovalStatus;

            if (existing.ApprovalStatus == SupplierApprovalStatus.Pending)
            {
                existing.ApprovedBy = null;
                existing.ApprovedOnUtc = null;
            }
            else if (existing.ApprovalStatus != previousStatus || !existing.ApprovedOnUtc.HasValue)
            {
                existing.ApprovedBy = User.Identity?.Name ?? RoleNames.Admin;
                existing.ApprovedOnUtc = DateTime.UtcNow;
                existing.LastReviewUtc ??= DateTime.UtcNow;
            }
        }
        existing.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(Supplier), existing.Id, "Edit", $"Updated supplier {existing.Name}.");
        
        // Re-evaluate risk status after manual edit
        await _performance.UpdateSupplierRiskStatusAsync(existing.Id);
        
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Suppliers.Approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        var blocker = await _supplierService.GetSupplierApprovalBlockerAsync(supplier);
        if (blocker != null)
        {
            TempData["ErrorMessage"] = blocker;
            return RedirectToAction(nameof(Details), new { id = supplier.Id });
        }

        supplier.ApprovalStatus = SupplierApprovalStatus.Approved;
        supplier.ApprovedBy = User.Identity?.Name ?? RoleNames.Admin;
        supplier.ApprovedOnUtc = DateTime.UtcNow;
        supplier.LastReviewUtc = DateTime.UtcNow;
        supplier.UpdatedAtUtc = DateTime.UtcNow;

        AddAudit(nameof(Supplier), supplier.Id, "Approve", $"Approved supplier {supplier.Name}.");
        
        // Re-evaluate risk status after approval
        await _performance.UpdateSupplierRiskStatusAsync(supplier.Id);
        
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' approved.";
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Suppliers.Approve")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        supplier.ApprovalStatus = SupplierApprovalStatus.Rejected;
        supplier.ApprovedBy = User.Identity?.Name ?? RoleNames.Admin;
        supplier.ApprovedOnUtc = DateTime.UtcNow;
        supplier.UpdatedAtUtc = DateTime.UtcNow;

        AddAudit(nameof(Supplier), supplier.Id, "Reject", $"Rejected supplier {supplier.Name}.");
        
        // Re-evaluate risk status after rejection
        await _performance.UpdateSupplierRiskStatusAsync(supplier.Id);
        
        var auditRows = await _db.SaveChangesAsync();
        TempData["ErrorMessage"] = $"Supplier '{supplier.Name}' rejected.";
        if (auditRows == 0)
        {
            TempData["WarningMessage"] = "Rejected, but audit log was not recorded.";
        }
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    public async Task<IActionResult> DocumentPreview(Guid id)
    {
        var document = await _db.SupplierComplianceDocuments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null || !IsLocalUploadUrl(document.FileUrl))
        {
            return NotFound();
        }

        return Redirect(document.FileUrl!);
    }

    public async Task<IActionResult> DocumentDownload(Guid id)
    {
        var document = await _db.SupplierComplianceDocuments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null || !IsLocalUploadUrl(document.FileUrl))
        {
            return NotFound();
        }

        var relativePath = document.FileUrl!.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);
        if (!System.IO.File.Exists(fullPath))
        {
            return NotFound();
        }

        var fileName = Path.GetFileName(fullPath);
        return PhysicalFile(fullPath, "application/octet-stream", fileName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Verify")]
    public async Task<IActionResult> ReviewDocument(Guid id)
    {
        var document = await _db.SupplierComplianceDocuments
            .Include(d => d.Supplier)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null)
        {
            return NotFound();
        }

        if (!HasStoredUpload(document))
        {
            TempData["ErrorMessage"] = $"Cannot verify '{document.DocumentType}': no file uploaded.";
            return RedirectToAction(nameof(Documents), new { supplierId = document.SupplierId });
        }

        document.DocumentStatus = SupplierDocumentStatus.Verified;
        document.UpdatedAtUtc = DateTime.UtcNow;
        if (document.Supplier != null)
        {
            document.Supplier.LastReviewUtc = DateTime.UtcNow;
        }
        AddAudit(nameof(SupplierComplianceDocument), document.Id, "Review", $"Reviewed supplier document {document.DocumentType}.");
        await _db.SaveChangesAsync();
        await _supplierService.UpdateSupplierRenewalDueUtcAsync(document.SupplierId);
        
        // Re-evaluate risk status after document review
        await _performance.UpdateSupplierRiskStatusAsync(document.SupplierId);
        
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Document '{document.DocumentType}' reviewed.";
        return RedirectToAction(nameof(Documents), new { supplierId = document.SupplierId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Compliance.Verify")]
    public async Task<IActionResult> FlagDocument(Guid id)
    {
        var document = await _db.SupplierComplianceDocuments
            .Include(d => d.Supplier)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (document == null)
        {
            return NotFound();
        }

        document.DocumentStatus = SupplierDocumentStatus.PendingReview;
        document.UpdatedAtUtc = DateTime.UtcNow;
        document.Notes = string.IsNullOrWhiteSpace(document.Notes)
            ? "Flagged by Compliance Officer for follow-up."
            : $"{document.Notes.Trim()} | Flagged by Compliance Officer for follow-up.";
        if (document.Supplier != null)
        {
            document.Supplier.LastReviewUtc = DateTime.UtcNow;
        }
        AddAudit(nameof(SupplierComplianceDocument), document.Id, "Flag", $"Flagged supplier document {document.DocumentType} for follow-up.");
        
        // Re-evaluate risk status after document flag
        await _performance.UpdateSupplierRiskStatusAsync(document.SupplierId);
        
        await _db.SaveChangesAsync();

        TempData["ErrorMessage"] = $"Document '{document.DocumentType}' flagged.";
        return RedirectToAction(nameof(Documents), new { supplierId = document.SupplierId });
    }

    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!IsProcurementOperator())
        {
            return Forbid();
        }

        var supplier = await _db.Suppliers
            .Include(s => s.SupplierCategory)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
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

        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            supplier.IsDeleted = true;
            supplier.DeletedAtUtc = DateTime.UtcNow;
            supplier.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(Supplier), supplier.Id, "Archive", $"Archived supplier {supplier.Name}.");
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' archived.";
        }
        else
        {
            TempData["ErrorMessage"] = "Archive failed: supplier not found.";
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

        var supplier = await _db.Suppliers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == id);
        if (supplier == null)
        {
            return NotFound();
        }

        supplier.IsDeleted = false;
        supplier.DeletedAtUtc = null;
        supplier.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(Supplier), supplier.Id, "Restore", $"Restored supplier {supplier.Name}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Supplier '{supplier.Name}' restored.";

        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Procurement.Write")]
    public async Task<IActionResult> RequestOverride(Guid id, string reason)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        if (string.IsNullOrWhiteSpace(reason))
        {
            TempData["ErrorMessage"] = "Override reason is required.";
            return RedirectToAction(nameof(Details), new { id = supplier.Id });
        }

        supplier.OverrideRequestActive = true;
        supplier.OverrideRequestReason = reason.Trim();
        supplier.RequestedBy = User.Identity?.Name ?? "system";
        supplier.RequestedAtUtc = DateTime.UtcNow;

        AddAudit(nameof(Supplier), supplier.Id, "OverrideRequested", $"Override requested by {supplier.RequestedBy}: {supplier.OverrideRequestReason}");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Override request submitted.";
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Suppliers.Override")]
    public async Task<IActionResult> ToggleOverride(Guid id, string? reason)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        supplier.IsOverrideActive = !supplier.IsOverrideActive;
        if (supplier.IsOverrideActive)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                // Fallback if toggled from a list without reason
                reason = supplier.OverrideRequestReason ?? "No reason provided.";
            }

            supplier.OverrideReason = reason.Trim();
            supplier.OverriddenBy = User.Identity?.Name ?? "SuperAdmin";
            
            // Clear pending request if it was active
            supplier.OverrideRequestActive = false;
            supplier.OverrideRequestReason = null;
            supplier.RequestedBy = null;
            supplier.RequestedAtUtc = null;

            AddAudit(nameof(Supplier), supplier.Id, "OverrideEnabled", $"Super Admin override enabled: {supplier.OverrideReason}");
        }
        else
        {
            supplier.OverrideReason = null;
            supplier.OverriddenBy = null;
            AddAudit(nameof(Supplier), supplier.Id, "OverrideDisabled", "Super Admin override disabled.");
        }

        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = supplier.IsOverrideActive ? "Override enabled." : "Override disabled.";
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Suppliers.Override")]
    public async Task<IActionResult> RejectOverrideRequest(Guid id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();

        var originalRequester = supplier.RequestedBy;
        supplier.OverrideRequestActive = false;
        supplier.OverrideRequestReason = null;
        supplier.RequestedBy = null;
        supplier.RequestedAtUtc = null;

        AddAudit(nameof(Supplier), supplier.Id, "OverrideRequestRejected", $"Super Admin rejected override request from {originalRequester}.");
        await _db.SaveChangesAsync();

        TempData["InfoMessage"] = "Override request rejected.";
        return RedirectToAction(nameof(Details), new { id = supplier.Id });
    }

    private void PopulateCategories()
    {
        var categories = _db.SupplierCategories
            .OrderBy(c => c.SortOrder)
            .Select(c => new { c.Id, c.Name })
            .ToList();
        ViewBag.SupplierCategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Id", "Name");
    }

    private void PopulateCertificationOptions(IEnumerable<string> selected)
    {
        var options = new[]
        {
            "FSC",
            "ISO 9001",
            "ISO 14001",
            "PEFC",
            "SVLK"
        };

        ViewBag.CertificationOptions = options;
        ViewBag.SelectedCertifications = new HashSet<string>(selected, StringComparer.OrdinalIgnoreCase);
    }

    private void PopulateDocumentTypes(string? selectedDocumentType)
    {
        var options = new[]
        {
            "Business Permit",
            "Environmental Certificate",
            "Sustainability Declaration"
        };

        ViewBag.DocumentTypeOptions = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(options, selectedDocumentType);
    }

    private static bool IsLocalUploadUrl(string? fileUrl) =>
        !string.IsNullOrWhiteSpace(fileUrl) &&
        fileUrl.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase);

    private bool HasStoredUpload(SupplierComplianceDocument document)
    {
        if (!IsLocalUploadUrl(document.FileUrl))
        {
            return false;
        }

        var relativePath = document.FileUrl!.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);
        return System.IO.File.Exists(fullPath);
    }

    private static string[] ParseCertifications(string certifications) =>
        (certifications ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private static string NormalizeCertifications(string? manualCertifications, string[] selectedCertifications)
    {
        var selected = selectedCertifications ?? Array.Empty<string>();
        var parsedManual = ParseCertifications(manualCertifications ?? string.Empty);
        var all = selected.Concat(parsedManual).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase);
        return string.Join(", ", all);
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

    private SupplierDocumentStatus ResolveDocumentStatus(string? fileUrl, DateTime? expiryDateUtc)
    {
        if (!IsLocalUploadUrl(fileUrl))
        {
            return SupplierDocumentStatus.Missing;
        }

        if (expiryDateUtc.HasValue && expiryDateUtc.Value.Date < DateTime.UtcNow.Date)
        {
            return SupplierDocumentStatus.Expired;
        }

        if (expiryDateUtc.HasValue && expiryDateUtc.Value.Date >= DateTime.UtcNow.Date)
        {
            return SupplierDocumentStatus.Verified;
        }

        return SupplierDocumentStatus.PendingReview;
    }
}
