using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using FurniComply.Domain.Enums;
using FurniComply.Infrastructure.Identity;
using FurniComply.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FurniComply.Web.Controllers;

[Authorize(Policy = "Reports.Read")]
public class RegulatoryReportsController : Controller
{
    private readonly AppDbContext _db;

    public RegulatoryReportsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var reports = await _db.RegulatoryReports
            .Include(r => r.ReportStatus)
            .Include(r => r.Supplier)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync();

        var reportIds = reports.Select(r => r.Id).ToList();
        var overrideReportIds = reportIds.Count == 0
            ? new HashSet<Guid>()
            : (await _db.AuditLogs
                .Where(a =>
                    a.EntityName == nameof(FurniComply.Domain.Entities.RegulatoryReport) &&
                    a.Action == "ApproveOverride" &&
                    reportIds.Contains(a.EntityId))
                .Select(a => a.EntityId)
                .Distinct()
                .ToListAsync())
            .ToHashSet();
        ViewBag.OverrideReportIds = overrideReportIds;

        return View(reports);
    }

    public async Task<IActionResult> Archived()
    {
        var reports = await _db.RegulatoryReports
            .IgnoreQueryFilters()
            .Where(r => r.IsDeleted)
            .Include(r => r.ReportStatus)
            .Include(r => r.Supplier)
            .OrderByDescending(r => r.DeletedAtUtc)
            .ThenByDescending(r => r.PeriodEndUtc)
            .ToListAsync();

        var reportIds = reports.Select(r => r.Id).ToList();
        var overrideReportIds = reportIds.Count == 0
            ? new HashSet<Guid>()
            : (await _db.AuditLogs
                .Where(a =>
                    a.EntityName == nameof(FurniComply.Domain.Entities.RegulatoryReport) &&
                    a.Action == "ApproveOverride" &&
                    reportIds.Contains(a.EntityId))
                .Select(a => a.EntityId)
                .Distinct()
                .ToListAsync())
            .ToHashSet();
        ViewBag.OverrideReportIds = overrideReportIds;

        return View(reports);
    }

    [Authorize(Policy = "Reports.Write")]
    public IActionResult Create(Guid? supplierId)
    {
        PopulateSuppliers(supplierId);
        PopulateReportTypes(null);
        var today = DateTime.UtcNow.Date;
        var start = today.AddDays(-30);
        return View(new FurniComply.Domain.Entities.RegulatoryReport
        {
            SupplierId = supplierId,
            PeriodStartUtc = start,
            PeriodEndUtc = today
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> Create(FurniComply.Domain.Entities.RegulatoryReport report)
    {
        ModelState.Remove(nameof(FurniComply.Domain.Entities.RegulatoryReport.ReportStatusId));
        ModelState.Remove(nameof(FurniComply.Domain.Entities.RegulatoryReport.SubmittedAtUtc));
        if (RequiresSupplier(report.ReportType) && (report.SupplierId == null || report.SupplierId == Guid.Empty))
        {
            ModelState.AddModelError(nameof(FurniComply.Domain.Entities.RegulatoryReport.SupplierId), "Supplier is required.");
        }

        if (!ModelState.IsValid)
        {
            PopulateSuppliers(report.SupplierId);
            PopulateReportTypes(report.ReportType);
            return View(report);
        }

        var draftId = await GetReportStatusIdAsync("Draft");
        if (draftId.HasValue)
        {
            report.ReportStatusId = draftId.Value;
        }
        report.SubmittedAtUtc = null;

        _db.RegulatoryReports.Add(report);
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Create", $"Created report {report.ReportType}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Draft saved.";
        return RedirectToAction(nameof(Details), new { id = report.Id });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var report = await _db.RegulatoryReports
            .IgnoreQueryFilters()
            .Include(r => r.ReportStatus)
            .Include(r => r.Supplier)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (report == null)
        {
            return NotFound();
        }

        ViewBag.OverrideAudit = await _db.AuditLogs
            .Where(a =>
                a.EntityName == nameof(FurniComply.Domain.Entities.RegulatoryReport) &&
                a.EntityId == id &&
                a.Action == "ApproveOverride")
            .OrderByDescending(a => a.TimestampUtc)
            .FirstOrDefaultAsync();
        ViewBag.AdminApprovalBlocker = await GetReportSubmissionBlockerAsync(report.SupplierId, report.ReportType);

        var supplierDocuments = new List<FurniComply.Domain.Entities.SupplierComplianceDocument>();
        var recentSupplierChecks = new List<FurniComply.Domain.Entities.ComplianceCheck>();
        var supplierOpenCapaCount = 0;
        var supplierHighAlertCount = 0;

        if (report.SupplierId.HasValue && report.SupplierId.Value != Guid.Empty)
        {
            supplierDocuments = await _db.SupplierComplianceDocuments
                .Where(d => d.SupplierId == report.SupplierId.Value && !d.IsDeleted)
                .OrderBy(d => d.DocumentType)
                .ThenByDescending(d => d.UpdatedAtUtc ?? d.CreatedAtUtc)
                .ToListAsync();

            var supplierName = report.Supplier?.Name?.Trim();
            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                recentSupplierChecks = await _db.ComplianceChecks
                    .Where(c => c.Notes.Contains(supplierName))
                    .Include(c => c.Policy)
                    .Include(c => c.ComplianceStatus)
                    .Include(c => c.RiskLevel)
                    .OrderByDescending(c => c.CheckedAtUtc)
                    .Take(5)
                    .ToListAsync();

                var relatedCheckIds = recentSupplierChecks.Select(c => c.Id).ToList();
                if (relatedCheckIds.Count > 0)
                {
                    supplierOpenCapaCount = await _db.CorrectiveActions.CountAsync(c =>
                        c.ComplianceCheckId.HasValue && 
                        relatedCheckIds.Contains(c.ComplianceCheckId.Value) &&
                        c.Status != CorrectiveActionStatus.Closed);
                }
            }

            supplierHighAlertCount = await _db.ComplianceAlerts.CountAsync(a =>
                a.IsActive &&
                a.EntityType == "Supplier" &&
                a.EntityId == report.SupplierId.Value &&
                (a.Severity == ComplianceAlertSeverity.High || a.Severity == ComplianceAlertSeverity.Critical));
        }

        ViewBag.SupplierDocuments = supplierDocuments;
        ViewBag.RecentSupplierChecks = recentSupplierChecks;
        ViewBag.SupplierOpenCapaCount = supplierOpenCapaCount;
        ViewBag.SupplierHighAlertCount = supplierHighAlertCount;
        ViewBag.SupplierApprovalStatus = report.SupplierId.HasValue && report.Supplier != null
            ? report.Supplier.ApprovalStatus
            : (Domain.Enums.SupplierApprovalStatus?)null;

        return View(report);
    }

    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }

        PopulateReportStatuses();
        PopulateSuppliers(report.SupplierId);
        PopulateReportTypes(report.ReportType);
        return View(report);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> Edit(Guid id, FurniComply.Domain.Entities.RegulatoryReport report)
    {
        if (id != report.Id)
        {
            return BadRequest();
        }

        ModelState.Remove(nameof(FurniComply.Domain.Entities.RegulatoryReport.ReportStatusId));
        ModelState.Remove(nameof(FurniComply.Domain.Entities.RegulatoryReport.SubmittedAtUtc));
        if (RequiresSupplier(report.ReportType) && (report.SupplierId == null || report.SupplierId == Guid.Empty))
        {
            ModelState.AddModelError(nameof(FurniComply.Domain.Entities.RegulatoryReport.SupplierId), "Supplier is required.");
        }

        if (!ModelState.IsValid)
        {
            PopulateReportStatuses();
            PopulateSuppliers(report.SupplierId);
            PopulateReportTypes(report.ReportType);
            return View(report);
        }

        var draftId = await GetReportStatusIdAsync("Draft");
        if (draftId.HasValue)
        {
            report.ReportStatusId = draftId.Value;
        }
        report.SubmittedAtUtc = null;

        _db.Entry(report).State = EntityState.Modified;
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Edit", $"Updated report {report.ReportType}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = "Draft saved.";
        return RedirectToAction(nameof(Details), new { id = report.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }

        if (RequiresSupplier(report.ReportType) && (!report.SupplierId.HasValue || report.SupplierId.Value == Guid.Empty))
        {
            TempData["ErrorMessage"] = "Submission blocked: supplier required.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var submittedId = await GetReportStatusIdAsync("Submitted");
        if (!submittedId.HasValue)
        {
            TempData["ErrorMessage"] = "Submission failed: status not configured.";
            return RedirectToAction(nameof(Index));
        }

        var blocker = await GetReportSubmissionBlockerAsync(report.SupplierId, report.ReportType);
        if (!string.IsNullOrWhiteSpace(blocker))
        {
            TempData["ErrorMessage"] = blocker;
            return RedirectToAction(nameof(Details), new { id });
        }

        report.ReportStatusId = submittedId.Value;
        report.SubmittedAtUtc = DateTime.UtcNow;
        report.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Submit", $"Submitted report {report.ReportType}.");
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Report '{report.ReportType}' submitted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var report = await _db.RegulatoryReports.FirstOrDefaultAsync(r => r.Id == id);
        if (report == null)
        {
            return NotFound();
        }

        return View(report);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report != null)
        {
            report.IsDeleted = true;
            report.DeletedAtUtc = DateTime.UtcNow;
            report.UpdatedAtUtc = DateTime.UtcNow;
            AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Archive", $"Archived report {report.ReportType}.");
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Report '{report.ReportType}' archived.";
        }
        else
        {
            TempData["ErrorMessage"] = "Archive failed: report not found.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Write")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var report = await _db.RegulatoryReports
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == id);
        if (report == null)
        {
            return NotFound();
        }

        report.IsDeleted = false;
        report.DeletedAtUtc = null;
        report.UpdatedAtUtc = DateTime.UtcNow;
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Restore", $"Restored report {report.ReportType}.");
        await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Report '{report.ReportType}' restored.";

        return RedirectToAction(nameof(Details), new { id = report.Id });
    }

    [Authorize(Policy = "Reports.Export")]
    public async Task<IActionResult> ExportExcel()
    {
        try
        {
            var reports = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.PeriodEndUtc)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Regulatory Reports");

        ws.Cell(1, 1).Value = "Report Type";
        ws.Cell(1, 2).Value = "Supplier";
        ws.Cell(1, 3).Value = "Period Start";
        ws.Cell(1, 4).Value = "Period End";
        ws.Cell(1, 5).Value = "Status";
        ws.Cell(1, 6).Value = "Submitted At";
        ws.Cell(1, 7).Value = "Summary";

        for (var i = 0; i < reports.Count; i++)
        {
            var row = i + 2;
            var r = reports[i];
            ws.Cell(row, 1).Value = r.ReportType;
            ws.Cell(row, 2).Value = r.Supplier?.Name ?? "All suppliers";
            ws.Cell(row, 3).Value = r.PeriodStartUtc.ToString("yyyy-MM-dd");
            ws.Cell(row, 4).Value = r.PeriodEndUtc.ToString("yyyy-MM-dd");
            ws.Cell(row, 5).Value = r.ReportStatus?.Name ?? "-";
            ws.Cell(row, 6).Value = r.SubmittedAtUtc?.ToString("yyyy-MM-dd") ?? "-";
            ws.Cell(row, 7).Value = r.Summary;
        }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"FurniComply_Reports_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch
        {
            TempData["ErrorMessage"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Policy = "Reports.Export")]
    public async Task<IActionResult> ExportExcelSingle(Guid id)
    {
        try
        {
            var report = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Regulatory Report");

        ws.Cell(1, 1).Value = "Report Type";
        ws.Cell(1, 2).Value = "Supplier";
        ws.Cell(1, 3).Value = "Period Start";
        ws.Cell(1, 4).Value = "Period End";
        ws.Cell(1, 5).Value = "Status";
        ws.Cell(1, 6).Value = "Submitted At";
        ws.Cell(1, 7).Value = "Summary";

        ws.Cell(2, 1).Value = report.ReportType;
        ws.Cell(2, 2).Value = report.Supplier?.Name ?? "All suppliers";
        ws.Cell(2, 3).Value = report.PeriodStartUtc.ToString("yyyy-MM-dd");
        ws.Cell(2, 4).Value = report.PeriodEndUtc.ToString("yyyy-MM-dd");
        ws.Cell(2, 5).Value = report.ReportStatus?.Name ?? "-";
        ws.Cell(2, 6).Value = report.SubmittedAtUtc?.ToString("yyyy-MM-dd") ?? "-";
        ws.Cell(2, 7).Value = report.Summary;

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Regulatory_Report_{report.Id:N}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch
        {
            TempData["ErrorMessage"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Policy = "Reports.Export")]
    [HttpGet]
    public IActionResult ExportSingle(Guid id, string format)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            TempData["ErrorMessage"] = "Please select an export format.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var key = format.Trim().ToLowerInvariant();
        if (key == "pdf")
        {
            return RedirectToAction(nameof(ExportPdfSingle), new { id });
        }

        if (key == "letter")
        {
            return RedirectToAction(nameof(ExportLetterSingle), new { id });
        }

        if (key == "excel")
        {
            return RedirectToAction(nameof(ExportExcelSingle), new { id });
        }

        TempData["ErrorMessage"] = "Unknown export format.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Policy = "Reports.Export")]
    public async Task<IActionResult> ExportPdf()
    {
        try
        {
            var reports = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.PeriodEndUtc)
                .ToListAsync();

        var totalReports = reports.Count;
        var submittedReports = reports.Count(r => r.ReportStatus?.Name == "Submitted");
        var acceptedReports = reports.Count(r => r.ReportStatus?.Name == "Accepted");
        var rejectedReports = reports.Count(r => r.ReportStatus?.Name == "Rejected");
        var draftReports = reports.Count(r => r.ReportStatus?.Name == "Draft");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Column(header =>
                {
                    header.Item().Text("FurniComply")
                        .FontSize(10)
                        .SemiBold()
                        .FontColor("#0f3b2f");
                    header.Item().Text("Regulatory Compliance Report Pack")
                        .FontSize(20)
                        .Bold()
                        .FontColor("#3f2f22");
                    header.Item().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                });

                page.Content().Column(content =>
                {
                    content.Spacing(12);

                    content.Item().Row(row =>
                    {
                        row.RelativeItem().Background("#f2f6f3").Padding(8).Column(col =>
                        {
                            col.Item().Text("Total Reports").FontSize(9).FontColor(Colors.Grey.Darken2);
                            col.Item().Text(totalReports.ToString()).FontSize(18).Bold().FontColor("#0f3b2f");
                        });
                        row.RelativeItem().Background("#f9f3ea").Padding(8).Column(col =>
                        {
                            col.Item().Text("Submitted").FontSize(9).FontColor(Colors.Grey.Darken2);
                            col.Item().Text(submittedReports.ToString()).FontSize(18).Bold().FontColor("#9a6b3f");
                        });
                        row.RelativeItem().Background("#eef5ef").Padding(8).Column(col =>
                        {
                            col.Item().Text("Accepted").FontSize(9).FontColor(Colors.Grey.Darken2);
                            col.Item().Text(acceptedReports.ToString()).FontSize(18).Bold().FontColor("#1e6d49");
                        });
                        row.RelativeItem().Background("#f8eeee").Padding(8).Column(col =>
                        {
                            col.Item().Text("Rejected").FontSize(9).FontColor(Colors.Grey.Darken2);
                            col.Item().Text(rejectedReports.ToString()).FontSize(18).Bold().FontColor("#9a2f2f");
                        });
                        row.RelativeItem().Background("#f4f4f4").Padding(8).Column(col =>
                        {
                            col.Item().Text("Draft").FontSize(9).FontColor(Colors.Grey.Darken2);
                            col.Item().Text(draftReports.ToString()).FontSize(18).Bold().FontColor("#4f4f4f");
                        });
                    });

                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Report Type");
                            header.Cell().Element(HeaderCell).Text("Supplier");
                            header.Cell().Element(HeaderCell).Text("Period");
                            header.Cell().Element(HeaderCell).Text("Status");
                            header.Cell().Element(HeaderCell).Text("Submitted");
                            header.Cell().Element(HeaderCell).Text("Summary");
                        });

                        foreach (var report in reports)
                        {
                            table.Cell().Element(BodyCell).Text(report.ReportType);
                            table.Cell().Element(BodyCell).Text(report.Supplier?.Name ?? "All suppliers");
                            table.Cell().Element(BodyCell).Text($"{report.PeriodStartUtc:yyyy-MM-dd} to {report.PeriodEndUtc:yyyy-MM-dd}");
                            table.Cell().Element(BodyCell).Text(report.ReportStatus?.Name ?? "-");
                            table.Cell().Element(BodyCell).Text(report.SubmittedAtUtc?.ToString("yyyy-MM-dd") ?? "-");
                            table.Cell().Element(BodyCell).Text(report.Summary);
                        }
                    });
                });

                page.Footer().AlignRight().Text(text =>
                {
                    text.Span("FurniComply Compliance Office | Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

            var pdfBytes = document.GeneratePdf();
            var fileName = $"FurniComply_Reports_{DateTime.UtcNow:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            var reports = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.PeriodEndUtc)
                .ToListAsync();

            ViewBag.GeneratedAtUtc = DateTime.UtcNow;
            ViewBag.PdfFallbackReason = ex.Message;
            TempData["WarningMessage"] = "PDF engine unavailable. Opening print view.";
            return View("ExportPdfFallback", reports);
        }
    }

    [Authorize(Policy = "Reports.Export")]
    public async Task<IActionResult> ExportPdfSingle(Guid id)
    {
        try
        {
            var report = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (report == null)
            {
                return NotFound();
            }

        var supplierName = report.Supplier?.Name ?? "All Suppliers";
        var statusLabel = report.ReportStatus?.Name ?? "-";
        var periodStart = report.PeriodStartUtc.ToLocalTime().ToString("MMMM dd, yyyy");
        var periodEnd = report.PeriodEndUtc.ToLocalTime().ToString("MMMM dd, yyyy");
        var submittedDate = report.SubmittedAtUtc.HasValue
            ? report.SubmittedAtUtc.Value.ToLocalTime().ToString("MMMM dd, yyyy")
            : "Not yet submitted";
        var generatedDate = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Header().Column(header =>
                {
                    header.Item().Row(row =>
                    {
                        row.RelativeItem().Column(left =>
                        {
                            left.Item().Text("FurniComply")
                                .FontSize(18).Bold().FontColor("#12372a");
                            left.Item().Text("Compliance Management System")
                                .FontSize(8).FontColor("#607168");
                        });
                        row.ConstantItem(140).AlignRight().Column(right =>
                        {
                            right.Item().AlignRight().Text("REGULATORY REPORT")
                                .FontSize(9).Bold().FontColor("#607168").LetterSpacing(0.08f);
                            right.Item().AlignRight().Text($"Generated: {generatedDate}")
                                .FontSize(7).FontColor("#8a9690");
                        });
                    });

                    header.Item().PaddingTop(8).LineHorizontal(1.5f).LineColor("#12372a");
                });

                page.Content().PaddingTop(16).Column(column =>
                {
                    column.Spacing(14);

                    column.Item().Text(report.ReportType)
                        .FontSize(16).Bold().FontColor("#1a2f26");

                    column.Item().Background("#f4f7f5").Padding(14).Column(card =>
                    {
                        card.Spacing(6);
                        card.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Supplier").FontSize(7).Bold().FontColor("#607168");
                                c.Item().Text(supplierName).FontSize(10).FontColor("#1a2f26");
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Status").FontSize(7).Bold().FontColor("#607168");
                                c.Item().Text(statusLabel).FontSize(10).FontColor("#1a2f26");
                            });
                        });
                        card.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Report Period").FontSize(7).Bold().FontColor("#607168");
                                c.Item().Text($"{periodStart}  to  {periodEnd}").FontSize(10).FontColor("#1a2f26");
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Date Submitted").FontSize(7).Bold().FontColor("#607168");
                                c.Item().Text(submittedDate).FontSize(10).FontColor("#1a2f26");
                            });
                        });
                    });

                    column.Item().PaddingTop(4).Column(sec =>
                    {
                        sec.Item().Text("Executive Summary")
                            .FontSize(12).Bold().FontColor("#12372a");
                        sec.Item().PaddingTop(4).Text(string.IsNullOrWhiteSpace(report.Summary) ? "No summary provided." : report.Summary)
                            .FontSize(10).FontColor("#2a3d34").LineHeight(1.5f);
                    });

                    column.Item().PaddingTop(16).LineHorizontal(0.5f).LineColor("#d0d8d4");

                    column.Item().PaddingTop(6).Text("COMPLIANCE NOTICE")
                        .FontSize(7).Bold().FontColor("#607168").LetterSpacing(0.08f);
                    column.Item().Text("This report was generated by FurniComply Compliance Management System. " +
                        "All data is sourced from verified operational modules including supplier management, " +
                        "procurement, and compliance check workflows. This document is intended for internal " +
                        "compliance review and regulatory submission purposes.")
                        .FontSize(8).FontColor("#8a9690").LineHeight(1.4f);
                });

                page.Footer().Column(footer =>
                {
                    footer.Item().LineHorizontal(0.5f).LineColor("#d0d8d4");
                    footer.Item().PaddingTop(6).Row(row =>
                    {
                        row.RelativeItem().Text("FurniComply · Compliance Management System")
                            .FontSize(7).FontColor("#8a9690");
                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("Page ").FontSize(7).FontColor("#8a9690");
                            text.CurrentPageNumber().FontSize(7).FontColor("#607168");
                            text.Span(" of ").FontSize(7).FontColor("#8a9690");
                            text.TotalPages().FontSize(7).FontColor("#607168");
                        });
                    });
                });
            });
        });

            var pdfBytes = document.GeneratePdf();
            var fileName = $"Regulatory_Report_{report.Id:N}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            var report = await _db.RegulatoryReports
                .Include(r => r.ReportStatus)
                .Include(r => r.Supplier)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            ViewBag.GeneratedAtUtc = DateTime.UtcNow;
            ViewBag.PdfFallbackReason = ex.Message;
            TempData["WarningMessage"] = "PDF engine unavailable. Opening print view.";
            return View("ExportPdfSingleFallback", report);
        }
    }

    [Authorize(Policy = "Reports.Export")]
    public async Task<IActionResult> ExportLetterSingle(Guid id)
    {
        var report = await _db.RegulatoryReports
            .Include(r => r.ReportStatus)
            .Include(r => r.Supplier)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (report == null)
        {
            return NotFound();
        }

        ViewBag.GeneratedAtUtc = DateTime.UtcNow;
        return View("ExportLetterSingle", report);
    }
    private async Task<Guid?> GetReportStatusIdAsync(string statusName)
    {
        return await _db.ReportStatuses
            .Where(s => s.Name == statusName)
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync();
    }
    private static IContainer HeaderCell(IContainer container)
    {
        return container
            .DefaultTextStyle(x => x.SemiBold())
            .PaddingVertical(6)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2);
    }

    private static IContainer BodyCell(IContainer container)
    {
        return container
            .PaddingVertical(4)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten4);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }

        var currentStatusName = await _db.ReportStatuses
            .Where(s => s.Id == report.ReportStatusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();
        if (!string.Equals(currentStatusName, "Submitted", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Approval blocked: report must be submitted first.";
            return RedirectToAction(nameof(Index));
        }

        var blocker = await GetReportSubmissionBlockerAsync(report.SupplierId, report.ReportType);
        if (!string.IsNullOrWhiteSpace(blocker))
        {
            TempData["ErrorMessage"] = blocker;
            return RedirectToAction(nameof(Index));
        }

        var acceptedId = await GetReportStatusIdAsync("Accepted");
        if (!acceptedId.HasValue)
        {
            TempData["ErrorMessage"] = "Approval failed: status not configured.";
            return RedirectToAction(nameof(Index));
        }

        report.ReportStatusId = acceptedId.Value;
        report.UpdatedAtUtc = DateTime.UtcNow;
        if (!report.SubmittedAtUtc.HasValue)
        {
            report.SubmittedAtUtc = DateTime.UtcNow;
        }
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Approve", $"Approved report {report.ReportType}.");
        var auditRows = await _db.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Report '{report.ReportType}' approved.";
        if (auditRows == 0)
        {
            TempData["WarningMessage"] = "Approved, but audit log was not recorded.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.ApproveOverride")]
    public async Task<IActionResult> ApproveOverride(Guid id, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            TempData["ErrorMessage"] = "Override reason is required.";
            return RedirectToAction(nameof(Index));
        }

        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }

        var currentStatusName = await _db.ReportStatuses
            .Where(s => s.Id == report.ReportStatusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();
        if (!string.Equals(currentStatusName, "Submitted", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Override allowed only for submitted reports.";
            return RedirectToAction(nameof(Index));
        }

        var blocker = await GetReportSubmissionBlockerAsync(report.SupplierId, report.ReportType);
        var acceptedId = await GetReportStatusIdAsync("Accepted");
        if (!acceptedId.HasValue)
        {
            TempData["ErrorMessage"] = "Override failed: status not configured.";
            return RedirectToAction(nameof(Index));
        }

        report.ReportStatusId = acceptedId.Value;
        report.UpdatedAtUtc = DateTime.UtcNow;
        if (!report.SubmittedAtUtc.HasValue)
        {
            report.SubmittedAtUtc = DateTime.UtcNow;
        }

        var blockerSnapshot = string.IsNullOrWhiteSpace(blocker) ? "none" : blocker;
        var cleanReason = reason.Trim();
        AddAudit(
            nameof(FurniComply.Domain.Entities.RegulatoryReport),
            report.Id,
            "ApproveOverride",
            $"Approved report {report.ReportType} with override: {cleanReason} | Blocker at approval: {blockerSnapshot}");

        await _db.SaveChangesAsync();
        TempData["WarningMessage"] = $"Report '{report.ReportType}' approved with override.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Reports.Approve")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var report = await _db.RegulatoryReports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }

        var currentStatusName = await _db.ReportStatuses
            .Where(s => s.Id == report.ReportStatusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();
        if (!string.Equals(currentStatusName, "Submitted", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Rejection blocked: report must be submitted first.";
            return RedirectToAction(nameof(Index));
        }

        var rejectedId = await GetReportStatusIdAsync("Rejected");
        if (!rejectedId.HasValue)
        {
            TempData["ErrorMessage"] = "Rejection failed: status not configured.";
            return RedirectToAction(nameof(Index));
        }

        report.ReportStatusId = rejectedId.Value;
        report.UpdatedAtUtc = DateTime.UtcNow;
        if (!report.SubmittedAtUtc.HasValue)
        {
            report.SubmittedAtUtc = DateTime.UtcNow;
        }
        AddAudit(nameof(FurniComply.Domain.Entities.RegulatoryReport), report.Id, "Reject", $"Rejected report {report.ReportType}.");
        var auditRows = await _db.SaveChangesAsync();
        TempData["ErrorMessage"] = $"Report '{report.ReportType}' rejected.";
        if (auditRows == 0)
        {
            TempData["WarningMessage"] = "Rejected, but audit log was not recorded.";
        }
        return RedirectToAction(nameof(Index));
    }
    private void PopulateReportStatuses()
    {
        var statuses = _db.ReportStatuses
            .OrderBy(s => s.SortOrder)
            .Select(s => new { s.Id, s.Name })
            .ToList();
        ViewBag.ReportStatusId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(statuses, "Id", "Name");
    }

    private void PopulateSuppliers(Guid? selectedSupplierId)
    {
        var suppliers = _db.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => new { s.Id, s.Name })
            .ToList();
        ViewBag.SupplierId = new SelectList(suppliers, "Id", "Name", selectedSupplierId);
    }

    private void PopulateReportTypes(string? selectedReportType)
    {
        var reportTypes = new[]
        {
            "Safety Compliance",
            "Environmental Compliance",
            "Workplace Health",
            "Supplier Requirements",
            "Internal Report"
        };

        ViewBag.ReportTypeOptions = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(reportTypes, selectedReportType);
    }

    private async Task<string?> GetReportSubmissionBlockerAsync(Guid? supplierId, string? reportType)
    {
        if (!RequiresSupplier(reportType) && (!supplierId.HasValue || supplierId.Value == Guid.Empty))
        {
            return null;
        }

        if (!supplierId.HasValue || supplierId.Value == Guid.Empty)
        {
            return "Submission blocked: supplier is required.";
        }

        var supplier = await _db.Suppliers
            .AsNoTracking()
            .Where(s => s.Id == supplierId.Value)
            .Select(s => new { s.Id, s.Name, s.ApprovalStatus, s.IsOverrideActive })
            .FirstOrDefaultAsync();

        if (supplier == null)
        {
            return "Submission blocked: selected supplier no longer exists.";
        }

        if (supplier.ApprovalStatus != SupplierApprovalStatus.Approved)
        {
            return $"Submission blocked: supplier '{supplier.Name}' is not approved.";
        }

        var supplierName = supplier.Name?.Trim();
        var supplierCheckIds = await _db.ComplianceChecks
            .Where(c => !string.IsNullOrEmpty(supplierName) && c.Notes.Contains(supplierName))
            .Select(c => c.Id)
            .ToListAsync();

        if (supplierCheckIds.Count == 0)
        {
            return $"Submission blocked: supplier '{supplier.Name}' has no compliance checks on record. Complete compliance checks first before requesting a regulatory report.";
        }

        var now = DateTime.UtcNow;
        if (!supplier.IsOverrideActive)
        {
            var hasInvalidSupplierDocuments = await _db.SupplierComplianceDocuments.AnyAsync(d =>
                d.SupplierId == supplier.Id &&
                !d.IsDeleted &&
                (d.DocumentStatus == SupplierDocumentStatus.Missing ||
                 d.DocumentStatus == SupplierDocumentStatus.PendingReview ||
                 d.DocumentStatus == SupplierDocumentStatus.Expired ||
                 (d.ExpiryDateUtc.HasValue && d.ExpiryDateUtc.Value < now)));
            if (hasInvalidSupplierDocuments)
            {
                return $"Submission blocked: supplier '{supplier.Name}' has missing/pending/expired compliance documents.";
            }
        }

        if (string.IsNullOrWhiteSpace(supplierName))
        {
            return null;
        }

        var hasOpenCorrectiveActions = await _db.CorrectiveActions.AnyAsync(c =>
            c.Status != CorrectiveActionStatus.Closed &&
            c.ComplianceCheckId.HasValue &&
            supplierCheckIds.Contains(c.ComplianceCheckId.Value));
        if (hasOpenCorrectiveActions)
        {
            return "Submission blocked: unresolved CAPA items must be closed first.";
        }

        var hasCriticalAlerts = await _db.ComplianceAlerts.AnyAsync(a =>
            a.IsActive &&
            (a.Severity == ComplianceAlertSeverity.Critical || a.Severity == ComplianceAlertSeverity.High) &&
            (
                (a.EntityType == "Supplier" && a.EntityId == supplier.Id) ||
                (a.EntityType == "ComplianceCheck" && a.EntityId.HasValue && supplierCheckIds.Contains(a.EntityId.Value))
            ));
        if (hasCriticalAlerts)
        {
            return "Submission blocked: active high/critical compliance alerts must be resolved first.";
        }

        var supplierChecks = await _db.ComplianceChecks
            .Where(c => c.Notes.Contains(supplierName))
            .Select(c => new
            {
                c.Id,
                c.PolicyId,
                c.CheckedAtUtc,
                StatusName = c.ComplianceStatus != null ? c.ComplianceStatus.Name : string.Empty
            })
            .ToListAsync();

        var latestChecksByPolicy = supplierChecks
            .GroupBy(c => c.PolicyId)
            .Select(g => g
                .OrderByDescending(x => x.CheckedAtUtc)
                .First())
            .ToList();

        var latestNonCompliantCheckIds = latestChecksByPolicy
            .Where(x => string.Equals(x.StatusName, "Non-Compliant", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Id)
            .Distinct()
            .ToList();

        var closedCapaCheckIds = latestNonCompliantCheckIds.Count == 0
            ? new List<Guid>()
            : await _db.CorrectiveActions
                .Where(ca =>
                    ca.ComplianceCheckId.HasValue &&
                    latestNonCompliantCheckIds.Contains(ca.ComplianceCheckId.Value) &&
                    ca.Status == CorrectiveActionStatus.Closed)
                .Select(ca => ca.ComplianceCheckId.Value)
                .Distinct()
                .ToListAsync();

        var hasCurrentNonCompliantChecks = latestNonCompliantCheckIds
            .Except(closedCapaCheckIds)
            .Any();

        if (hasCurrentNonCompliantChecks)
        {
            return "Submission blocked: unresolved non-compliant checks still exist.";
        }

        return null;
    }

    private static bool RequiresSupplier(string? reportType)
    {
        if (string.IsNullOrWhiteSpace(reportType))
        {
            return true;
        }

        return reportType.IndexOf("Internal", StringComparison.OrdinalIgnoreCase) < 0;
    }

    private void AddAudit(string entityName, Guid entityId, string action, string details)
    {
        _db.AuditLogs.Add(new FurniComply.Domain.Entities.AuditLog
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
