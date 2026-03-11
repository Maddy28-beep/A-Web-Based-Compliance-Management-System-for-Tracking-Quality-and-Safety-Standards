using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupplierPerformanceSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EvaluationDate",
                table: "SupplierPerformances",
                newName: "EvaluationDateUtc");

            migrationBuilder.AlterColumn<decimal>(
                name: "PerformanceScore",
                table: "Suppliers",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "SupplierPerformances",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PerformanceScore",
                table: "SupplierPerformances",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "EvaluatedBy",
                table: "SupplierPerformances",
                type: "nvarchar(180)",
                maxLength: 180,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DefectRate",
                table: "SupplierPerformances",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(382));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(384));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(387));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(389));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(391));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(373));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(376));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(378));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(665));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(670));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(672));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(674));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(677));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(679));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(681));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(682));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(684));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(686));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(721));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(734));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(739));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(742));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(745));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(748));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(750));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(753));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(756));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(759));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(324));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(326));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(328));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(299));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(307));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(310));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(313));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(315));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(318));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(593));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(605));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(609));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(613));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(618));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(620));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(623));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(625));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9973));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9984));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9988));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9991));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9995));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 913, DateTimeKind.Utc).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(55));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(61));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(885));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(890));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(897));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(903));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(556));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(489));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(503));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(508));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(510));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(512));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(514));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(186));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(224));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(227));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(444));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(447));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(449));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(451));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(458));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(438));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(442));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(829));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(834));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(844));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(847));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(849));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-001", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(100), 4.6m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-002", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(121), 3.2m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-003", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(126), 4.8m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-004", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(129), 4.2m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-005", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(132), 4.5m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-006", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(134), 1.5m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc" },
                values: new object[] { "SUP-007", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(137) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-008", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(140), 3.9m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-009", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(142), 2.8m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                columns: new[] { "Code", "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { "SUP-010", new DateTime(2026, 3, 2, 5, 14, 3, 914, DateTimeKind.Utc).AddTicks(145), 4.7m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "EvaluationDateUtc",
                table: "SupplierPerformances",
                newName: "EvaluationDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "PerformanceScore",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "SupplierPerformances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<decimal>(
                name: "PerformanceScore",
                table: "SupplierPerformances",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "EvaluatedBy",
                table: "SupplierPerformances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(180)",
                oldMaxLength: 180);

            migrationBuilder.AlterColumn<decimal>(
                name: "DefectRate",
                table: "SupplierPerformances",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9248));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9268));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9834));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9927));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9964));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9967));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9995));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(7));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9154));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9156));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9129));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9140));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8576));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8601));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8606));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8626));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8631));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8699));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8703));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(232));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(235));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(237));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(252));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(254));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(256));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9492));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9502));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9511));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9514));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9525));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9528));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9538));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8927));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8934));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8942));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8945));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8957));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8961));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9434));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9368));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(123));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(141));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(143));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(148));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(150));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(155));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(164));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(166));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 213, DateTimeKind.Utc).AddTicks(170));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8780), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8800), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8804), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8808), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8814), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8817), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8826), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8829), 0m });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8836), 0m });
        }
    }
}
