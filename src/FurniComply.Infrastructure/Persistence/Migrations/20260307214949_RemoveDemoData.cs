using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use Raw SQL to purge all operational data while preserving lookup tables
            // Order is critical to respect foreign key constraints

            migrationBuilder.Sql("DELETE FROM [AuditLogs]");
            migrationBuilder.Sql("DELETE FROM [CorrectiveActions]");
            migrationBuilder.Sql("DELETE FROM [ComplianceAlerts]");
            migrationBuilder.Sql("DELETE FROM [ComplianceChecks]");
            migrationBuilder.Sql("DELETE FROM [PolicyVersions]");
            migrationBuilder.Sql("DELETE FROM [Policies]");
            migrationBuilder.Sql("DELETE FROM [ProcurementItems]");
            migrationBuilder.Sql("DELETE FROM [ProcurementOrders]");
            migrationBuilder.Sql("DELETE FROM [SupplierComplianceDocuments]");
            migrationBuilder.Sql("DELETE FROM [SupplierPerformances]");
            migrationBuilder.Sql("DELETE FROM [SupplierEvaluations]");
            migrationBuilder.Sql("DELETE FROM [RegulatoryReports]");
            migrationBuilder.Sql("DELETE FROM [Suppliers]");

            // We keep lookup tables:
            // - ComplianceStatusMaster
            // - ReportStatusMaster
            // - ProcurementStatusMaster
            // - SupplierCategory
            // - PolicyCategory
            // - ComplianceCategory
            // - RiskLevel
            // - ComplianceAlertRules (User wants to keep these as they are system rules)
            
            // Update the existing rules to ensure they are in a clean state if needed
            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAtUtc", "Description", "Name", "RuleKey", "Severity" },
                values: new object[] { new DateTime(2026, 3, 7, 21, 49, 48, 969, DateTimeKind.Utc).AddTicks(7129), "Alert when a check is non-compliant.", "Non-Compliant Checks", "NON_COMPLIANT_CHECKS", 2 });

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAtUtc", "Description", "Name", "RuleKey", "Severity" },
                values: new object[] { new DateTime(2026, 3, 7, 21, 49, 48, 969, DateTimeKind.Utc).AddTicks(7137), "Alert when a supplier is on hold.", "Suppliers on Hold", "SUPPLIERS_ON_HOLD", 1 });

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "Description", "Name", "RuleKey", "Severity" },
                values: new object[] { new DateTime(2026, 3, 7, 21, 49, 48, 969, DateTimeKind.Utc).AddTicks(7140), "Alert when a supplier renewal is due soon.", "Supplier Renewal Due Soon", "SUPPLIER_RENEWAL_DUE_SOON", 0 });

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "Description", "Name", "RuleKey" },
                values: new object[] { new DateTime(2026, 3, 7, 21, 49, 48, 969, DateTimeKind.Utc).AddTicks(7142), "Alert when a supplier renewal is overdue.", "Supplier Renewal Overdue", "SUPPLIER_RENEWAL_OVERDUE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down method doesn't restore data in this case as it's a purge operation
        }
    }
}
