using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierIdToRegulatoryReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "RegulatoryReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(614));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(590));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(312));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(562));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(376));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "CreatedAtUtc", "SupplierId" },
                values: new object[] { new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(631), null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 22, 19, 41, 46, 651, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_SupplierId",
                table: "RegulatoryReports",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegulatoryReports_Suppliers_SupplierId",
                table: "RegulatoryReports",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegulatoryReports_Suppliers_SupplierId",
                table: "RegulatoryReports");

            migrationBuilder.DropIndex(
                name: "IX_RegulatoryReports_SupplierId",
                table: "RegulatoryReports");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "RegulatoryReports");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5101));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5107));

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
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5079));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5083));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5085));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5055));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5062));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5166));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 20, 7, 39, 31, 190, DateTimeKind.Utc).AddTicks(5034));
        }
    }
}
