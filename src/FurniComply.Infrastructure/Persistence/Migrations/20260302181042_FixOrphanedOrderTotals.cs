using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixOrphanedOrderTotals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rule 6: Clean up legacy data
            // Reset totals for orders with 0 items
            migrationBuilder.Sql(@"
                UPDATE ProcurementOrders 
                SET TotalAmount = 0, 
                    ExchangeRateUsed = NULL, 
                    ExchangeRateTimestampUtc = NULL
                WHERE Id NOT IN (SELECT DISTINCT ProcurementOrderId FROM ProcurementItems WHERE IsDeleted = 0)
            ");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2523));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2538));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2541));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2485));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2501));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2512));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3135));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3149));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3151));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3153));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3155));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3171));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3233));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3248));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3258));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3270));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3278));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3291));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3295));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2409));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2287));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2345));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2975));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3014));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3024));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3051));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1781));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1806));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1811));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1826));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1834));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1843));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1846));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1850));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3613));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3626));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3628));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3637));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2777));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2750));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2758));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2760));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2762));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2764));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2771));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2149));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2172));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2209));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2215));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2622));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2630));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2633));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2638));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2641));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2644));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2590));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2614));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2617));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3361));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3371));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3375));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3508));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3513));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3517));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3519));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(3523));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(1986));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2035));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2039));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2053));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2056));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2059));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 18, 10, 41, 395, DateTimeKind.Utc).AddTicks(2063));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(83));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(86));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(88));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(91));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(421));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(432));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(435));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(439));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(441));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(444));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(446));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(448));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(451));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(567));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(576));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(588));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(592));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(595));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(601));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(605));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(608));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(613));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(7));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(9));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9977));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9989));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9991));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9994));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(358));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(363));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(373));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(376));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9639));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9657));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(736));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(740));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(743));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(746));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(749));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(753));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(755));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(758));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(760));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(762));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(295));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(275));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(279));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(281));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(284));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(286));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9850));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9907));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9910));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9913));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9919));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9922));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(182));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(224));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(158));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(173));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(176));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(662));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(667));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(670));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(672));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(674));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(677));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(679));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(685));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(688));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 813, DateTimeKind.Utc).AddTicks(690));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9769));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 17, 10, 22, 812, DateTimeKind.Utc).AddTicks(9799));
        }
    }
}
