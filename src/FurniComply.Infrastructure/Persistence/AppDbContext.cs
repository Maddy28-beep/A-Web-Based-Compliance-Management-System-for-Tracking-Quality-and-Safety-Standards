using FurniComply.Domain.Entities;
using FurniComply.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurniComply.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<ComplianceCheck> ComplianceChecks => Set<ComplianceCheck>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RegulatoryReport> RegulatoryReports => Set<RegulatoryReport>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ProcurementOrder> ProcurementOrders => Set<ProcurementOrder>();
    public DbSet<ProcurementItem> ProcurementItems => Set<ProcurementItem>();
    public DbSet<ComplianceStatusMaster> ComplianceStatuses => Set<ComplianceStatusMaster>();
    public DbSet<ReportStatusMaster> ReportStatuses => Set<ReportStatusMaster>();
    public DbSet<ProcurementStatusMaster> ProcurementStatuses => Set<ProcurementStatusMaster>();
    public DbSet<SupplierCategory> SupplierCategories => Set<SupplierCategory>();
    public DbSet<PolicyCategory> PolicyCategories => Set<PolicyCategory>();
    public DbSet<ComplianceCategory> ComplianceCategories => Set<ComplianceCategory>();
    public DbSet<RiskLevel> RiskLevels => Set<RiskLevel>();
    public DbSet<PolicyVersion> PolicyVersions => Set<PolicyVersion>();
    public DbSet<SupplierComplianceDocument> SupplierComplianceDocuments => Set<SupplierComplianceDocument>();
    public DbSet<ComplianceAlertRule> ComplianceAlertRules => Set<ComplianceAlertRule>();
    public DbSet<ComplianceAlert> ComplianceAlerts => Set<ComplianceAlert>();
    public DbSet<CorrectiveAction> CorrectiveActions => Set<CorrectiveAction>();
    public DbSet<SupplierPerformance> SupplierPerformances => Set<SupplierPerformance>();
    public DbSet<SupplierEvaluation> SupplierEvaluations => Set<SupplierEvaluation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var complianceStatusPendingId = new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var complianceStatusCompliantId = new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var complianceStatusNonCompliantId = new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var reportStatusDraftId = new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var reportStatusSubmittedId = new Guid("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var reportStatusAcceptedId = new Guid("66666666-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var reportStatusRejectedId = new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var procurementStatusDraftId = new Guid("88888888-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var procurementStatusSubmittedId = new Guid("88888888-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var procurementStatusApprovedId = new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var procurementStatusRejectedId = new Guid("99999999-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var procurementStatusOrderedId = new Guid("aaaaaaaa-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var procurementStatusReceivedId = new Guid("bbbbbbbb-cccc-cccc-cccc-cccccccccccc");
        var procurementStatusClosedId = new Guid("cccccccc-dddd-dddd-dddd-dddddddddddd");
        var procurementStatusCancelledId = new Guid("dddddddd-eeee-eeee-eeee-eeeeeeeeeeee");
        var supplierCategoryRawId = new Guid("eeeeeeee-1111-1111-1111-111111111111");
        var supplierCategoryHardwareId = new Guid("eeeeeeee-2222-2222-2222-222222222222");
        var supplierCategoryFinishingId = new Guid("eeeeeeee-3333-3333-3333-333333333333");
        var supplierCategoryPackagingId = new Guid("eeeeeeee-4444-4444-4444-444444444444");
        var policyCategorySafetyId = new Guid("ffffffff-1111-1111-1111-111111111111");
        var policyCategoryQualityId = new Guid("ffffffff-2222-2222-2222-222222222222");
        var policyCategoryProcurementId = new Guid("ffffffff-3333-3333-3333-333333333333");
        var policyCategoryEnvironmentalId = new Guid("ffffffff-4444-4444-4444-444444444444");
        var complianceCategorySafetyId = new Guid("abababab-1111-1111-1111-111111111111");
        var complianceCategoryQualityId = new Guid("abababab-2222-2222-2222-222222222222");
        var complianceCategorySupplierId = new Guid("abababab-3333-3333-3333-333333333333");
        var complianceCategoryRegulatoryId = new Guid("abababab-4444-4444-4444-444444444444");
        var complianceCategoryEnvironmentalId = new Guid("abababab-5555-5555-5555-555555555555");
        var riskLowId = new Guid("cdcdcdcd-1111-1111-1111-111111111111");
        var riskMediumId = new Guid("cdcdcdcd-2222-2222-2222-222222222222");
        var riskHighId = new Guid("cdcdcdcd-3333-3333-3333-333333333333");
        var riskCriticalId = new Guid("cdcdcdcd-4444-4444-4444-444444444444");

        modelBuilder.Entity<ComplianceCheck>(entity =>
        {
            entity.HasOne(c => c.Policy)
                .WithMany()
                .HasForeignKey(c => c.PolicyId);

            entity.HasOne(c => c.ComplianceStatus)
                .WithMany()
                .HasForeignKey(c => c.ComplianceStatusId);

            entity.HasOne(c => c.ComplianceCategory)
                .WithMany()
                .HasForeignKey(c => c.ComplianceCategoryId);

            entity.HasOne(c => c.RiskLevel)
                .WithMany()
                .HasForeignKey(c => c.RiskLevelId);

            entity.HasQueryFilter(c => !c.IsDeleted && !c.Policy!.IsDeleted);
        });

        modelBuilder.Entity<ProcurementOrder>(entity =>
        {
            entity.HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierId);

            entity.HasOne(p => p.ProcurementStatus)
                .WithMany()
                .HasForeignKey(p => p.ProcurementStatusId);

            entity.Property(p => p.TotalAmount).HasPrecision(18, 2);
            entity.Property(p => p.ExchangeRateUsed).HasPrecision(18, 4);

            entity.HasMany(p => p.Items)
                .WithOne(i => i.ProcurementOrder)
                .HasForeignKey(i => i.ProcurementOrderId);

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(s => s.Name).HasMaxLength(200);
            entity.Property(s => s.ContactEmail).HasMaxLength(200);
            entity.Property(s => s.Rating).HasPrecision(4, 2);
            entity.Property(s => s.PerformanceScore).HasPrecision(4, 2);
            entity.Property(s => s.ApprovedBy).HasMaxLength(120);

            entity.HasOne(s => s.SupplierCategory)
                .WithMany()
                .HasForeignKey(s => s.SupplierCategoryId);

            entity.HasMany(s => s.ComplianceDocuments)
                .WithOne(d => d.Supplier)
                .HasForeignKey(d => d.SupplierId);

            entity.HasMany(s => s.Evaluations)
                .WithOne(e => e.Supplier)
                .HasForeignKey(e => e.SupplierId);

            entity.HasQueryFilter(s => !s.IsDeleted);
        });

        modelBuilder.Entity<ProcurementItem>(entity =>
        {
            entity.Property(i => i.UnitCost).HasPrecision(18, 2);
            entity.HasQueryFilter(i => !i.IsDeleted && !i.ProcurementOrder!.IsDeleted);
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.Property(p => p.Code).HasMaxLength(50);
            entity.Property(p => p.Title).HasMaxLength(200);
            entity.Property(p => p.Owner).HasMaxLength(100);
            entity.HasIndex(p => p.Code).IsUnique();

            entity.HasOne(p => p.PolicyCategory)
                .WithMany()
                .HasForeignKey(p => p.PolicyCategoryId);

            entity.HasMany(p => p.PolicyVersions)
                .WithOne(v => v.Policy)
                .HasForeignKey(v => v.PolicyId);

            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<PolicyVersion>(entity =>
        {
            entity.Property(v => v.VersionNumber).HasMaxLength(20);
            entity.Property(v => v.Owner).HasMaxLength(120);
            entity.Property(v => v.Notes).HasMaxLength(1000);
            entity.HasQueryFilter(v => !v.Policy!.IsDeleted);
        });

        modelBuilder.Entity<SupplierComplianceDocument>(entity =>
        {
            entity.Property(d => d.DocumentType).HasMaxLength(120);
            entity.Property(d => d.FileUrl).HasMaxLength(400);
            entity.Property(d => d.Notes).HasMaxLength(1000);
            entity.HasQueryFilter(d => !d.IsDeleted && !d.Supplier!.IsDeleted);
        });

        modelBuilder.Entity<ComplianceAlertRule>(entity =>
        {
            entity.Property(r => r.RuleKey).HasMaxLength(100);
            entity.Property(r => r.Name).HasMaxLength(150);
            entity.Property(r => r.Description).HasMaxLength(500);
            entity.HasIndex(r => r.RuleKey).IsUnique();
            entity.HasQueryFilter(r => !r.IsDeleted);
        });

        modelBuilder.Entity<ComplianceAlert>(entity =>
        {
            entity.Property(a => a.Title).HasMaxLength(200);
            entity.Property(a => a.Message).HasMaxLength(1000);
            entity.Property(a => a.EntityType).HasMaxLength(80);

            entity.HasOne(a => a.ComplianceAlertRule)
                .WithMany()
                .HasForeignKey(a => a.ComplianceAlertRuleId);

            entity.HasQueryFilter(a => !a.IsDeleted && !a.ComplianceAlertRule!.IsDeleted);
        });

        modelBuilder.Entity<CorrectiveAction>(entity =>
        {
            entity.Property(c => c.Title).HasMaxLength(220);
            entity.Property(c => c.Description).HasMaxLength(2000);
            entity.Property(c => c.AssignedTo).HasMaxLength(180);
            entity.Property(c => c.EvidenceNotes).HasMaxLength(2000);
            entity.Property(c => c.ClosedBy).HasMaxLength(180);

            entity.HasOne(c => c.ComplianceCheck)
                .WithMany()
                .HasForeignKey(c => c.ComplianceCheckId);

            entity.HasOne(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId);

            entity.HasIndex(c => new { c.ComplianceCheckId, c.Status });
            entity.HasIndex(c => new { c.Status, c.DueAtUtc });
            entity.HasQueryFilter(c => !c.IsDeleted && (c.ComplianceCheckId == null || !c.ComplianceCheck!.IsDeleted));
        });

        modelBuilder.Entity<SupplierPerformance>(entity =>
        {
            entity.Property(p => p.PerformanceScore).HasPrecision(4, 2);
            entity.Property(p => p.DefectRate).HasPrecision(5, 2);
            entity.Property(p => p.Remarks).HasMaxLength(2000);
            entity.Property(p => p.EvaluatedBy).HasMaxLength(180);

            entity.HasOne(p => p.Supplier)
                .WithMany(s => s.Performances)
                .HasForeignKey(p => p.SupplierId);

            entity.HasQueryFilter(p => !p.IsDeleted && !p.Supplier!.IsDeleted);
        });

        modelBuilder.Entity<SupplierEvaluation>(entity =>
        {
            entity.HasKey(e => e.SupplierEvaluationId);
            entity.Property(e => e.SupplierEvaluationId).ValueGeneratedOnAdd();
            entity.Property(e => e.EvaluatedByUserId).HasMaxLength(450);
            entity.Property(e => e.EvaluatedByEmail).HasMaxLength(256);
            entity.Property(e => e.BandSnapshot).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ReasonsSnapshot).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => new { e.SupplierId, e.EvaluatedAtUtc });
            entity.HasQueryFilter(e => !e.Supplier!.IsDeleted);
        });

        modelBuilder.Entity<RegulatoryReport>(entity =>
        {
            entity.HasOne(r => r.ReportStatus)
                .WithMany()
                .HasForeignKey(r => r.ReportStatusId);

            entity.HasOne(r => r.Supplier)
                .WithMany()
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(r => !r.IsDeleted);
        });

        modelBuilder.Entity<ComplianceStatusMaster>().HasData(
            new ComplianceStatusMaster { Id = complianceStatusPendingId, Name = "Pending", SortOrder = 1 },
            new ComplianceStatusMaster { Id = complianceStatusCompliantId, Name = "Compliant", SortOrder = 2 },
            new ComplianceStatusMaster { Id = complianceStatusNonCompliantId, Name = "Non-Compliant", SortOrder = 3 }
        );

        modelBuilder.Entity<ReportStatusMaster>().HasData(
            new ReportStatusMaster { Id = reportStatusDraftId, Name = "Draft", SortOrder = 1 },
            new ReportStatusMaster { Id = reportStatusSubmittedId, Name = "Submitted", SortOrder = 2 },
            new ReportStatusMaster { Id = reportStatusAcceptedId, Name = "Accepted", SortOrder = 3 },
            new ReportStatusMaster { Id = reportStatusRejectedId, Name = "Rejected", SortOrder = 4 }
        );

        modelBuilder.Entity<ProcurementStatusMaster>().HasData(
            new ProcurementStatusMaster { Id = procurementStatusDraftId, Name = "Draft", SortOrder = 1 },
            new ProcurementStatusMaster { Id = procurementStatusSubmittedId, Name = "Submitted", SortOrder = 2 },
            new ProcurementStatusMaster { Id = procurementStatusApprovedId, Name = "Approved", SortOrder = 3 },
            new ProcurementStatusMaster { Id = procurementStatusRejectedId, Name = "Rejected", SortOrder = 4 },
            new ProcurementStatusMaster { Id = procurementStatusOrderedId, Name = "Ordered", SortOrder = 5 },
            new ProcurementStatusMaster { Id = procurementStatusReceivedId, Name = "Received", SortOrder = 6 },
            new ProcurementStatusMaster { Id = procurementStatusClosedId, Name = "Closed", SortOrder = 7 },
            new ProcurementStatusMaster { Id = procurementStatusCancelledId, Name = "Cancelled", SortOrder = 8 }
        );

        modelBuilder.Entity<SupplierCategory>().HasData(
            new SupplierCategory { Id = supplierCategoryRawId, Name = "Raw Materials", SortOrder = 1 },
            new SupplierCategory { Id = supplierCategoryHardwareId, Name = "Hardware", SortOrder = 2 },
            new SupplierCategory { Id = supplierCategoryFinishingId, Name = "Finishing", SortOrder = 3 },
            new SupplierCategory { Id = supplierCategoryPackagingId, Name = "Packaging", SortOrder = 4 }
        );

        modelBuilder.Entity<PolicyCategory>().HasData(
            new PolicyCategory { Id = policyCategorySafetyId, Name = "Safety", SortOrder = 1 },
            new PolicyCategory { Id = policyCategoryQualityId, Name = "Quality", SortOrder = 2 },
            new PolicyCategory { Id = policyCategoryProcurementId, Name = "Procurement", SortOrder = 3 },
            new PolicyCategory { Id = policyCategoryEnvironmentalId, Name = "Environmental", SortOrder = 4 }
        );

        modelBuilder.Entity<ComplianceCategory>().HasData(
            new ComplianceCategory { Id = complianceCategorySafetyId, Name = "Safety", SortOrder = 1 },
            new ComplianceCategory { Id = complianceCategoryQualityId, Name = "Quality", SortOrder = 2 },
            new ComplianceCategory { Id = complianceCategorySupplierId, Name = "Supplier", SortOrder = 3 },
            new ComplianceCategory { Id = complianceCategoryRegulatoryId, Name = "Regulatory", SortOrder = 4 }
        );

        modelBuilder.Entity<RiskLevel>().HasData(
            new RiskLevel { Id = riskLowId, Name = "Low", SortOrder = 1 },
            new RiskLevel { Id = riskMediumId, Name = "Medium", SortOrder = 2 },
            new RiskLevel { Id = riskHighId, Name = "High", SortOrder = 3 },
            new RiskLevel { Id = riskCriticalId, Name = "Critical", SortOrder = 4 }
        );

        modelBuilder.Entity<ComplianceAlertRule>().HasData(
            new ComplianceAlertRule { Id = new Guid("1111eeee-1111-1111-1111-111111111111"), RuleKey = "NON_COMPLIANT_CHECKS", Name = "Non-Compliant Checks", Description = "Alert when a check is non-compliant.", Severity = Domain.Enums.ComplianceAlertSeverity.High, IsEnabled = true },
            new ComplianceAlertRule { Id = new Guid("2222eeee-2222-2222-2222-222222222222"), RuleKey = "SUPPLIERS_ON_HOLD", Name = "Suppliers on Hold", Description = "Alert when a supplier is on hold.", Severity = Domain.Enums.ComplianceAlertSeverity.Warning, IsEnabled = true },
            new ComplianceAlertRule { Id = new Guid("3333eeee-3333-3333-3333-333333333333"), RuleKey = "SUPPLIER_RENEWAL_DUE_SOON", Name = "Supplier Renewal Due Soon", Description = "Alert when a supplier renewal is due soon.", Severity = Domain.Enums.ComplianceAlertSeverity.Info, IsEnabled = true },
            new ComplianceAlertRule { Id = new Guid("4444eeee-4444-4444-4444-444444444444"), RuleKey = "SUPPLIER_RENEWAL_OVERDUE", Name = "Supplier Renewal Overdue", Description = "Alert when a supplier renewal is overdue.", Severity = Domain.Enums.ComplianceAlertSeverity.High, IsEnabled = true }
        );
    }
}
