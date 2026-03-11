using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurniComply.Domain.Entities;
using FurniComply.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurniComply.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, int targetPerModule = 5, bool includeLiveScenarios = false)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // 1. Ensure database is ready
        if (db.Database.IsSqlite())
        {
            await db.Database.EnsureCreatedAsync();
        }
        else
        {
            try { await db.Database.MigrateAsync(); } catch { /* Migrations might already be applied */ }
        }

        // 2. Clear existing data to ensure a clean slate of 5 records as requested
        // (Wait, user just purged, so we should just seed if count < 5)
        if (await db.Suppliers.CountAsync() >= 5) return;

        // Fetch or Create Master Data
        var qualityCat = await GetPolicyCategory(db, "Quality Management");
        var safetyCat = await GetPolicyCategory(db, "Health & Safety");
        var envCat = await GetPolicyCategory(db, "Environmental");
        var procCat = await GetPolicyCategory(db, "Procurement");

        var rawSuppCat = await GetSupplierCategory(db, "Raw Materials");
        var hardSuppCat = await GetSupplierCategory(db, "Hardware");
        var finishSuppCat = await GetSupplierCategory(db, "Finishing & Coatings");
        var packSuppCat = await GetSupplierCategory(db, "Packaging");

        var qualityCompCat = await GetComplianceCategory(db, "Quality Compliance");
        var safetyCompCat = await GetComplianceCategory(db, "Safety Compliance");
        var envCompCat = await GetComplianceCategory(db, "Environmental Compliance");
        var suppCompCat = await GetComplianceCategory(db, "Supplier Governance");

        var compliantStatus = await db.ComplianceStatuses.FirstOrDefaultAsync(s => s.Name == "Compliant");
        var nonCompliantStatus = await db.ComplianceStatuses.FirstOrDefaultAsync(s => s.Name == "Non-Compliant");
        
        var orderedStatus = await db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Ordered");
        var approvedStatus = await db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Approved");
        var draftStatus = await db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
        var cancelledStatus = await db.ProcurementStatuses.FirstOrDefaultAsync(s => s.Name == "Cancelled");

        var riskLow = await db.RiskLevels.FirstOrDefaultAsync(r => r.Name == "Low");
        var riskHigh = await db.RiskLevels.FirstOrDefaultAsync(r => r.Name == "High");
        var riskCritical = await db.RiskLevels.FirstOrDefaultAsync(r => r.Name == "Critical");

        var reportDraft = await db.ReportStatuses.FirstOrDefaultAsync(s => s.Name == "Draft");
        var reportSubmitted = await db.ReportStatuses.FirstOrDefaultAsync(s => s.Name == "Submitted");

        var now = DateTime.UtcNow;

        // 3. Filipino People (Actors)
        string adminName = "Juan Dela Cruz";
        string complianceName = "Maria Clara Santos";
        string procurementName = "Ricardo Dalisay";
        string safetyName = "Elena Adarna";
        string sustainabilityName = "Antonio Luna";

        // 4. Seed exactly 5 Filipino Suppliers
        var suppliers = new List<Supplier>
        {
            new Supplier { Id = Guid.NewGuid(), SupplierCategoryId = rawSuppCat.Id, Name = "Bayanihan Timber Co.", Code = "SUP-PH-001", ContactEmail = "bayanihan@timber.ph", Status = SupplierStatus.Active, Rating = 4.8m, Certifications = "FSC, ISO 9001", ApprovalStatus = SupplierApprovalStatus.Approved, RenewalDueUtc = now.AddMonths(6) },
            new Supplier { Id = Guid.NewGuid(), SupplierCategoryId = hardSuppCat.Id, Name = "Sikatuna Hardware Solutions", Code = "SUP-PH-002", ContactEmail = "sikatuna@hardware.ph", Status = SupplierStatus.OnHold, Rating = 3.2m, Certifications = "ISO 14001", ApprovalStatus = SupplierApprovalStatus.Approved, RenewalDueUtc = now.AddDays(-5) },
            new Supplier { Id = Guid.NewGuid(), SupplierCategoryId = finishSuppCat.Id, Name = "Maharlika Coatings & Finishes", Code = "SUP-PH-003", ContactEmail = "maharlika@coatings.ph", Status = SupplierStatus.Active, Rating = 4.7m, Certifications = "REACH, RoHS", ApprovalStatus = SupplierApprovalStatus.Approved, RenewalDueUtc = now.AddYears(1) },
            new Supplier { Id = Guid.NewGuid(), SupplierCategoryId = packSuppCat.Id, Name = "Malasakit Packaging Group", Code = "SUP-PH-004", ContactEmail = "malasakit@packaging.ph", Status = SupplierStatus.Active, Rating = 4.0m, Certifications = "DTI Certified", ApprovalStatus = SupplierApprovalStatus.Approved, RenewalDueUtc = now.AddMonths(8) },
            new Supplier { Id = Guid.NewGuid(), SupplierCategoryId = packSuppCat.Id, Name = "Agila Logistics Services", Code = "SUP-PH-005", ContactEmail = "agila@logistics.ph", Status = SupplierStatus.Suspended, Rating = 1.8m, Certifications = "None", ApprovalStatus = SupplierApprovalStatus.Rejected }
        };
        await db.Suppliers.AddRangeAsync(suppliers);
        await db.SaveChangesAsync();

        // 4b. Seed Supplier Compliance Documents with expiry dates (for early warning / risk mitigation demo)
        var docTypes = new[] { "Business Permit", "Environmental Certificate", "Sustainability Declaration" };
        var expiriesBySupplier = new Dictionary<int, DateTime> // supplier index -> earliest expiry for RenewalDueUtc
        {
            { 0, now.AddDays(10) },   // Bayanihan: expiring in 10 days (early warning)
            { 1, now.AddDays(-5) },  // Sikatuna: already overdue
            { 2, now.AddDays(20) },  // Maharlika: expiring in 20 days (early warning)
            { 3, now.AddDays(5) },   // Malasakit: expiring in 5 days (early warning)
            { 4, now.AddMonths(6) }  // Agila: far future
        };
        foreach (var kv in expiriesBySupplier)
        {
            var supplierIdx = kv.Key;
            var expiry = kv.Value;
            var supplier = suppliers[supplierIdx];
            foreach (var docType in docTypes)
            {
                var docExpiry = supplierIdx == 0 && docType == "Business Permit" ? now.AddDays(10)
                    : supplierIdx == 0 ? now.AddMonths(6)
                    : supplierIdx == 1 ? (docType == "Business Permit" ? now.AddDays(-5) : now.AddMonths(3))
                    : supplierIdx == 2 && docType == "Environmental Certificate" ? now.AddDays(20)
                    : supplierIdx == 2 ? now.AddYears(1)
                    : supplierIdx == 3 && docType == "Sustainability Declaration" ? now.AddDays(5)
                    : supplierIdx == 3 ? now.AddMonths(4)
                    : now.AddMonths(6);
                db.SupplierComplianceDocuments.Add(new SupplierComplianceDocument
                {
                    Id = Guid.NewGuid(),
                    SupplierId = supplier.Id,
                    DocumentType = docType,
                    DocumentStatus = docExpiry < now ? SupplierDocumentStatus.Expired
                        : docExpiry <= now.AddDays(30) ? SupplierDocumentStatus.Verified
                        : SupplierDocumentStatus.Verified,
                    ExpiryDateUtc = docExpiry,
                    FileUrl = null,
                    Notes = "Seeded for demo.",
                    CreatedAtUtc = now
                });
            }
            supplier.RenewalDueUtc = expiry;
        }
        // Ensure "Supplier Renewal Due Soon" rule warns 30 days ahead (for early warning demo)
        var dueSoonRule = await db.ComplianceAlertRules.FirstOrDefaultAsync(r => r.RuleKey == "SUPPLIER_RENEWAL_DUE_SOON");
        if (dueSoonRule != null)
        {
            dueSoonRule.ThresholdValue = 30;
        }
        await db.SaveChangesAsync();

        // 5. Seed exactly 5 Policies
        var policies = new List<Policy>
        {
            new Policy { Id = Guid.NewGuid(), PolicyCategoryId = qualityCat.Id, Code = "PH-QMS-001", Title = "Pamantayang Kalidad ng Materyales", Version = "1.0", EffectiveDateUtc = now.AddMonths(-5), Status = PolicyStatus.Active, Owner = adminName, Content = "Criteria for raw materials quality." },
            new Policy { Id = Guid.NewGuid(), PolicyCategoryId = safetyCat.Id, Code = "PH-SAFE-010", Title = "Gabay sa Kaligtasan sa Trabaho", Version = "2.0", EffectiveDateUtc = now.AddMonths(-4), Status = PolicyStatus.Active, Owner = safetyName, Content = "Safety practices for workshop." },
            new Policy { Id = Guid.NewGuid(), PolicyCategoryId = envCat.Id, Code = "PH-ENV-005", Title = "Patakaran sa Likas-Kayang Pagkuha", Version = "1.1", EffectiveDateUtc = now.AddMonths(-2), Status = PolicyStatus.Active, Owner = sustainabilityName, Content = "Sustainable timber sourcing." },
            new Policy { Id = Guid.NewGuid(), PolicyCategoryId = procCat.Id, Code = "PH-PROC-022", Title = "Kodigo ng Gawi ng Supplier", Version = "3.0", EffectiveDateUtc = now.AddMonths(-7), Status = PolicyStatus.Active, Owner = procurementName, Content = "Ethical conduct for vendors." },
            new Policy { Id = Guid.NewGuid(), PolicyCategoryId = safetyCat.Id, Code = "PH-SAFE-015", Title = "Protokol sa Paghawak ng Kemikal", Version = "1.0", EffectiveDateUtc = now.AddMonths(-1), Status = PolicyStatus.Active, Owner = safetyName, Content = "Chemical storage guidelines." }
        };
        await db.Policies.AddRangeAsync(policies);
        await db.SaveChangesAsync();

        // 6. Seed exactly 5 Procurement Orders
        var orders = new List<ProcurementOrder>
        {
            new ProcurementOrder { Id = Guid.NewGuid(), SupplierId = suppliers[0].Id, ProcurementStatusId = orderedStatus?.Id ?? Guid.Empty, OrderNumber = "PO-PH-2026-001", OrderDateUtc = now.AddDays(-10), TotalAmount = 150000m },
            new ProcurementOrder { Id = Guid.NewGuid(), SupplierId = suppliers[1].Id, ProcurementStatusId = draftStatus?.Id ?? Guid.Empty, OrderNumber = "PO-PH-2026-002", OrderDateUtc = now.AddDays(-5), TotalAmount = 45000m },
            new ProcurementOrder { Id = Guid.NewGuid(), SupplierId = suppliers[2].Id, ProcurementStatusId = approvedStatus?.Id ?? Guid.Empty, OrderNumber = "PO-PH-2026-003", OrderDateUtc = now.AddDays(-2), TotalAmount = 85000m },
            new ProcurementOrder { Id = Guid.NewGuid(), SupplierId = suppliers[3].Id, ProcurementStatusId = orderedStatus?.Id ?? Guid.Empty, OrderNumber = "PO-PH-2026-004", OrderDateUtc = now.AddDays(-1), TotalAmount = 30000m },
            new ProcurementOrder { Id = Guid.NewGuid(), SupplierId = suppliers[4].Id, ProcurementStatusId = cancelledStatus?.Id ?? Guid.Empty, OrderNumber = "PO-PH-2026-005", OrderDateUtc = now.AddDays(-20), TotalAmount = 120000m }
        };
        await db.ProcurementOrders.AddRangeAsync(orders);

        // Add 1 item per order
        foreach (var order in orders)
        {
            db.ProcurementItems.Add(new ProcurementItem
            {
                Id = Guid.NewGuid(),
                ProcurementOrderId = order.Id,
                ItemName = "Supply Materials for " + order.OrderNumber,
                Quantity = 100,
                UnitCost = order.TotalAmount / 100,
                QualityStandard = "PH-STD-2026",
                Notes = "Order for Filipino production line."
            });
        }
        await db.SaveChangesAsync();

        // 7. Seed exactly 5 Compliance Checks
        var checks = new List<ComplianceCheck>
        {
            new ComplianceCheck { Id = Guid.NewGuid(), PolicyId = policies[0].Id, ComplianceStatusId = compliantStatus?.Id ?? Guid.Empty, ComplianceCategoryId = qualityCompCat.Id, RiskLevelId = riskLow?.Id ?? Guid.Empty, CheckedAtUtc = now.AddDays(-15), Notes = "Material audit passed by " + complianceName },
            new ComplianceCheck { Id = Guid.NewGuid(), PolicyId = policies[1].Id, ComplianceStatusId = nonCompliantStatus?.Id ?? Guid.Empty, ComplianceCategoryId = safetyCompCat.Id, RiskLevelId = riskHigh?.Id ?? Guid.Empty, CheckedAtUtc = now.AddDays(-10), Notes = "Missing safety gear detected by " + safetyName },
            new ComplianceCheck { Id = Guid.NewGuid(), PolicyId = policies[2].Id, ComplianceStatusId = compliantStatus?.Id ?? Guid.Empty, ComplianceCategoryId = envCompCat.Id, RiskLevelId = riskLow?.Id ?? Guid.Empty, CheckedAtUtc = now.AddDays(-5), Notes = "FSC logs verified for Bayanihan." },
            new ComplianceCheck { Id = Guid.NewGuid(), PolicyId = policies[3].Id, ComplianceStatusId = compliantStatus?.Id ?? Guid.Empty, ComplianceCategoryId = suppCompCat.Id, RiskLevelId = riskLow?.Id ?? Guid.Empty, CheckedAtUtc = now.AddDays(-2), Notes = "Signed code of conduct received." },
            new ComplianceCheck { Id = Guid.NewGuid(), PolicyId = policies[4].Id, ComplianceStatusId = nonCompliantStatus?.Id ?? Guid.Empty, ComplianceCategoryId = safetyCompCat.Id, RiskLevelId = riskCritical?.Id ?? Guid.Empty, CheckedAtUtc = now.AddDays(-1), Notes = "Spillage risk found in Maharlika facility." }
        };
        await db.ComplianceChecks.AddRangeAsync(checks);
        await db.SaveChangesAsync();

        // 8. Seed exactly 5 Corrective Actions
        var capas = new List<CorrectiveAction>
        {
            new CorrectiveAction { Id = Guid.NewGuid(), ComplianceCheckId = checks[1].Id, Title = "Mag-install ng Safety Signs", Description = "Install signage in the production area.", AssignedTo = safetyName, Status = CorrectiveActionStatus.InProgress, DueAtUtc = now.AddDays(7), CreatedAtUtc = now },
            new CorrectiveAction { Id = Guid.NewGuid(), ComplianceCheckId = checks[4].Id, Title = "Ayusin ang Chemical Leak", Description = "Fix the containment leak in Maharlika.", AssignedTo = adminName, Status = CorrectiveActionStatus.Open, DueAtUtc = now.AddDays(3), CreatedAtUtc = now },
            new CorrectiveAction { Id = Guid.NewGuid(), ComplianceCheckId = checks[1].Id, Title = "Safety Training ng mga Tauhan", Description = "Conduct safety seminar for all workers.", AssignedTo = safetyName, Status = CorrectiveActionStatus.InProgress, DueAtUtc = now.AddDays(14), CreatedAtUtc = now },
            new CorrectiveAction { Id = Guid.NewGuid(), ComplianceCheckId = checks[4].Id, Title = "Upgrade ng Spill Kit", Description = "Purchase new spill kits for the facility.", AssignedTo = procurementName, Status = CorrectiveActionStatus.Open, DueAtUtc = now.AddDays(5), CreatedAtUtc = now },
            new CorrectiveAction { Id = Guid.NewGuid(), ComplianceCheckId = checks[1].Id, Title = "Review ng Safety Gear", Description = "Check all PPE for wear and tear.", AssignedTo = safetyName, Status = CorrectiveActionStatus.Closed, DueAtUtc = now.AddDays(-2), ClosedAtUtc = now.AddDays(-1), CreatedAtUtc = now.AddMonths(-1) }
        };
        await db.CorrectiveActions.AddRangeAsync(capas);
        await db.SaveChangesAsync();

        // 9. Seed exactly 5 Audit Logs
        var auditLogs = new List<AuditLog>
        {
            new AuditLog { Id = Guid.NewGuid(), Action = "Status Changed to Suspended", Actor = adminName, TimestampUtc = now.AddDays(-20), Details = "Agila Logistics suspended due to repeated violations.", EntityName = "Supplier", EntityId = suppliers[4].Id },
            new AuditLog { Id = Guid.NewGuid(), Action = "New Policy Created", Actor = adminName, TimestampUtc = now.AddDays(-15), Details = "Created Pamantayang Kalidad ng Materyales.", EntityName = "Policy", EntityId = policies[0].Id },
            new AuditLog { Id = Guid.NewGuid(), Action = "Check Marked Non-Compliant", Actor = safetyName, TimestampUtc = now.AddDays(-10), Details = "Safety breach found in workshop.", EntityName = "ComplianceCheck", EntityId = checks[1].Id },
            new AuditLog { Id = Guid.NewGuid(), Action = "Order Approved", Actor = adminName, TimestampUtc = now.AddDays(-2), Details = "Approved order for Maharlika Coatings.", EntityName = "ProcurementOrder", EntityId = orders[2].Id },
            new AuditLog { Id = Guid.NewGuid(), Action = "Report Submitted", Actor = complianceName, TimestampUtc = now.AddDays(-1), Details = "Submitted monthly compliance summary.", EntityName = "RegulatoryReport", EntityId = Guid.NewGuid() }
        };
        await db.AuditLogs.AddRangeAsync(auditLogs);
        await db.SaveChangesAsync();

        // 10. Seed exactly 5 Regulatory Reports
        var reports = new List<RegulatoryReport>
        {
            new RegulatoryReport { Id = Guid.NewGuid(), SupplierId = suppliers[0].Id, ReportStatusId = reportSubmitted?.Id ?? Guid.Empty, ReportType = "Environmental Compliance", PeriodStartUtc = now.AddYears(-1), PeriodEndUtc = now, SubmittedAtUtc = now.AddDays(-3), Summary = "Annual environmental report for Bayanihan." },
            new RegulatoryReport { Id = Guid.NewGuid(), SupplierId = suppliers[1].Id, ReportStatusId = reportDraft?.Id ?? Guid.Empty, ReportType = "Safety Compliance", PeriodStartUtc = now.AddMonths(-6), PeriodEndUtc = now, Summary = "Draft report for permit renewal." },
            new RegulatoryReport { Id = Guid.NewGuid(), SupplierId = suppliers[2].Id, ReportStatusId = reportSubmitted?.Id ?? Guid.Empty, ReportType = "Workplace Health", PeriodStartUtc = now.AddMonths(-3), PeriodEndUtc = now, SubmittedAtUtc = now.AddDays(-1), Summary = "Quarterly audit for Maharlika." },
            new RegulatoryReport { Id = Guid.NewGuid(), SupplierId = suppliers[3].Id, ReportStatusId = reportDraft?.Id ?? Guid.Empty, ReportType = "Supplier Requirements", PeriodStartUtc = now.AddMonths(-1), PeriodEndUtc = now, Summary = "Initial review for Malasakit Group." },
            new RegulatoryReport { Id = Guid.NewGuid(), SupplierId = suppliers[4].Id, ReportStatusId = reportSubmitted?.Id ?? Guid.Empty, ReportType = "Internal Report", PeriodStartUtc = now.AddMonths(-2), PeriodEndUtc = now, SubmittedAtUtc = now.AddDays(-15), Summary = "Investigation into Agila Logistics breach." }
        };
        await db.RegulatoryReports.AddRangeAsync(reports);
        await db.SaveChangesAsync();

        // 11. Seed 5 Compliance Alerts
        var alertRules = await db.ComplianceAlertRules.ToListAsync();
        if (alertRules.Any())
        {
            var r1 = alertRules.FirstOrDefault(r => r.RuleKey == "NON_COMPLIANT_CHECKS") ?? alertRules[0];
            var r2 = alertRules.FirstOrDefault(r => r.RuleKey == "SUPPLIERS_ON_HOLD") ?? alertRules[0];
            var r3 = alertRules.FirstOrDefault(r => r.RuleKey == "SUPPLIER_RENEWAL_OVERDUE") ?? alertRules[0];

            var alerts = new List<ComplianceAlert>
            {
                new ComplianceAlert { Id = Guid.NewGuid(), ComplianceAlertRuleId = r1.Id, Title = "BABALA: Safety Breach", Message = "Missing safety gear detected in production.", EntityType = "ComplianceCheck", EntityId = checks[1].Id, Severity = ComplianceAlertSeverity.High, TriggeredAtUtc = now.AddDays(-10) },
                new ComplianceAlert { Id = Guid.NewGuid(), ComplianceAlertRuleId = r2.Id, Title = "Supplier Naka-Hold: Sikatuna", Message = "Sikatuna Hardware Solutions is on hold due to expired permit.", EntityType = "Supplier", EntityId = suppliers[1].Id, Severity = ComplianceAlertSeverity.Warning, TriggeredAtUtc = now.AddDays(-5) },
                new ComplianceAlert { Id = Guid.NewGuid(), ComplianceAlertRuleId = r3.Id, Title = "Expired na Permit: Sikatuna", Message = "The permit for Sikatuna has expired 5 days ago.", EntityType = "Supplier", EntityId = suppliers[1].Id, Severity = ComplianceAlertSeverity.Critical, TriggeredAtUtc = now.AddDays(-5) },
                new ComplianceAlert { Id = Guid.NewGuid(), ComplianceAlertRuleId = r1.Id, Title = "KRITIKAL: Chemical Spillage Risk", Message = "High risk of chemical spill found in Maharlika.", EntityType = "ComplianceCheck", EntityId = checks[4].Id, Severity = ComplianceAlertSeverity.Critical, TriggeredAtUtc = now.AddDays(-1) },
                new ComplianceAlert { Id = Guid.NewGuid(), ComplianceAlertRuleId = r1.Id, Title = "Overdue na Corrective Action", Message = "Corrective action for safety signs is overdue.", EntityType = "CorrectiveAction", EntityId = capas[0].Id, Severity = ComplianceAlertSeverity.High, TriggeredAtUtc = now.AddHours(-12) }
            };
            await db.ComplianceAlerts.AddRangeAsync(alerts);
            await db.SaveChangesAsync();
        }

        // 12. Seed 5 Performance Records
        for (var i = 0; i < 5; i++)
        {
            db.SupplierPerformances.Add(new SupplierPerformance
            {
                Id = Guid.NewGuid(),
                SupplierId = suppliers[i].Id,
                EvaluationDateUtc = now.AddDays(-2),
                QualityRating = 5 - (i % 2),
                DeliveryRating = 4 - (i % 2),
                DefectRate = 0.5m + (i * 0.1m),
                PerformanceScore = 4.5m - (i * 0.2m),
                Remarks = "Likas-kayang pagsusuri para sa " + suppliers[i].Name,
                EvaluatedBy = adminName
            });
        }
        await db.SaveChangesAsync();
    }

    private static async Task<PolicyCategory> GetPolicyCategory(AppDbContext db, string name)
    {
        return await db.PolicyCategories.FirstOrDefaultAsync(c => c.Name.Contains(name))
            ?? (await db.PolicyCategories.AddAsync(new PolicyCategory { Id = Guid.NewGuid(), Name = name })).Entity;
    }

    private static async Task<SupplierCategory> GetSupplierCategory(AppDbContext db, string name)
    {
        return await db.SupplierCategories.FirstOrDefaultAsync(c => c.Name.Contains(name))
            ?? (await db.SupplierCategories.AddAsync(new SupplierCategory { Id = Guid.NewGuid(), Name = name })).Entity;
    }

    private static async Task<ComplianceCategory> GetComplianceCategory(AppDbContext db, string name)
    {
        return await db.ComplianceCategories.FirstOrDefaultAsync(c => c.Name.Contains(name))
            ?? (await db.ComplianceCategories.AddAsync(new ComplianceCategory { Id = Guid.NewGuid(), Name = name })).Entity;
    }
}
