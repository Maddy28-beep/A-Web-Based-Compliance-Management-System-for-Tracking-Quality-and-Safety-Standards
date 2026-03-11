using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteAcrossTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "SupplierComplianceDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SupplierComplianceDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RegulatoryReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RegulatoryReports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ProcurementOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProcurementOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ProcurementItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProcurementItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "PolicyVersions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PolicyVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CorrectiveActions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CorrectiveActions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ComplianceChecks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComplianceChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ComplianceAlerts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComplianceAlerts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ComplianceAlertRules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComplianceAlertRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AuditLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5148), null, false });

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5101), null, false });

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5107), null, false });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5004));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5079), null, false });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5083), null, false });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5085), null, false });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5055), null, false });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5062), null, false });

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5166), null, false });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5026), null, false });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "DeletedAtUtc", "IsDeleted" },
                values: new object[] { new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5034), null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "SupplierComplianceDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SupplierComplianceDocuments");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RegulatoryReports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RegulatoryReports");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ProcurementOrders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProcurementOrders");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ProcurementItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProcurementItems");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "PolicyVersions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PolicyVersions");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ComplianceChecks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComplianceChecks");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ComplianceAlerts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComplianceAlerts");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ComplianceAlertRules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComplianceAlertRules");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6283));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6251));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6255));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6130));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6139));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6227));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6195));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6202));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6300));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6162));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 16, 20, 7, 53, 474, DateTimeKind.Utc).AddTicks(6169));
        }
    }
}
