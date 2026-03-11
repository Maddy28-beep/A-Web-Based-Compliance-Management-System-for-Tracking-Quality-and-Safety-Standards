using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePolicyCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Policies
                SET Code = CONCAT('AUTO-', RIGHT(CONVERT(varchar(8), NEWID()), 4))
                WHERE Code IS NULL OR LTRIM(RTRIM(Code)) = '';
            ");
            migrationBuilder.Sql(@"
                WITH cte AS (
                    SELECT Id, Code, ROW_NUMBER() OVER (PARTITION BY Code ORDER BY Id) rn
                    FROM Policies
                    WHERE Code IS NOT NULL AND LTRIM(RTRIM(Code)) <> ''
                )
                UPDATE p
                SET Code = CONCAT(p.Code, '-', RIGHT(CONVERT(varchar(8), NEWID()), 4))
                FROM Policies p
                JOIN cte ON p.Id = cte.Id
                WHERE cte.rn > 1;
            ");
            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3373));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3328));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3181));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3192));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3261));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3393));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3227));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 11, 19, 54, 28, 922, DateTimeKind.Utc).AddTicks(3235));

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Code",
                table: "Policies",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Policies_Code",
                table: "Policies");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3250));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3033));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3196));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3200));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3202));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3173));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3268));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3068));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 10, 16, 56, 21, 573, DateTimeKind.Utc).AddTicks(3074));
        }
    }
}
