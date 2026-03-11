using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceProductFieldsAndSeedDataFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "SupplierPerformances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "SupplierPerformances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9352));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9354));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9356));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9358));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9359));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9342));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9346));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9348));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9311));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9313));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9315));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9294));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9303));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9306));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9308));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9309));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9526));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9532));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9535));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9537));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9539));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9582));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9588));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9592));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9128));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9134));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9138));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9140));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9143));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9786));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9788));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9471));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555551"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9473));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555552"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555553"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555554"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9485));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555556"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555557"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9492));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555558"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555559"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9497));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555560"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9499));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9459));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9467));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9470));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9251));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9258));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9260));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9262));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9267));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9418));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9422));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9423));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9427));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9407));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9414));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9416));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9175));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9187));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9189));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9191));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9194));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9196));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9200));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9202));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 45, 21, 284, DateTimeKind.Utc).AddTicks(9229));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SupplierPerformances");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "SupplierPerformances");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(308));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(310));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(312));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(314));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(315));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(252));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(254));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(538));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(541));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(543));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(546));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(547));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(548));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(550));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(551));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(552));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(589));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(591));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(593));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(595));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(600));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(201));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(458));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(489));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(492));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(495));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(497));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(500));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(502));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(511));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(27));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(34));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(38));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(40));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(42));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(44));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(46));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(700));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(703));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(705));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(707));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(708));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(711));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(712));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(714));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(715));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(717));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(404));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555551"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555552"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555553"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555554"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555556"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(421));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555557"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(423));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555558"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(426));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555559"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(428));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555560"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(384));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(389));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(392));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(395));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(152));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(161));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(163));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(165));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(167));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(169));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(171));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(172));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(177));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(349));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(351));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(355));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(357));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(359));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(361));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(337));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(344));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(633));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(643));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(670));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(673));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(675));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(677));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(85));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(89));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(91));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(93));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(123));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(125));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 14, 41, 33, 446, DateTimeKind.Utc).AddTicks(128));
        }
    }
}
