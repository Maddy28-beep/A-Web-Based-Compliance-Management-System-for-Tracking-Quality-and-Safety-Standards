using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierOverrideRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OverrideRequestActive",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OverrideRequestReason",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedAtUtc",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7713));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7715));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7719));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7721));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7725));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7710));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7932));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7937));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7938));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7939));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7942));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7943));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7945));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7946));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7949));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7975));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7979));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7982));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7984));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7986));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7988));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7990));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7994));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7996));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7998));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7643));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7645));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7647));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7672));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7634));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7637));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7641));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7846));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7887));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7891));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7893));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7895));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7897));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7902));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7904));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7906));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7414));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7434));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7436));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7439));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7441));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7443));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8098));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8102));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8104));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8107));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8108));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8110));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8113));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8115));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7814), "CertiPUR-US" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7795), "ISO 9001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7799), "ASTM D4236" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7801), "ANSI Grade 1" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7803), "FSC Certified" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7804), "ISO 14001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7807), "ISO 9001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7809));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7811), "BIFMA X5.1" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7812), "OEKO-TEX" });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7586));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7590));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7594));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7596));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7598));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7600));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7761));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7765));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7770));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7757));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8022));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8024));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8025));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8027));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8030));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8031));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8034));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(8074));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7479), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7488), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7490), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7492), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7540), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy", "Status" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7543), false, null, null, null, 4 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7545), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7548), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7550), false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "OverrideRequestActive", "OverrideRequestReason", "RequestedAtUtc", "RequestedBy" },
                values: new object[] { new DateTime(2026, 3, 6, 12, 40, 39, 393, DateTimeKind.Utc).AddTicks(7553), false, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverrideRequestActive",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "OverrideRequestReason",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RequestedAtUtc",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RequestedBy",
                table: "Suppliers");

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5084));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5093));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5095));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5073));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5077));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5080));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5082));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5314));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5317));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5319));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5321));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5324));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5325));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5329));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5331));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5384));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5403));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5406));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5409));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5412));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5037));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5033));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5259));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5264));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5268));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5273));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5275));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5279));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5282));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4808));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4813));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4820));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4823));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4826));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4829));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5482));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5486));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5489));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5491));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5495));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5496));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5498));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5500));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5234), "SAFE-010" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5214), "QMS-001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5218), "SAFE-010" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5220), "QMS-001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5222), "ENV-005" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5226), "SAFE-015" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5227), "QMS-001" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5229));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5231), "QMS-015" });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                columns: new[] { "CreatedAtUtc", "QualityStandard" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5232), "PROC-022" });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4970));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4982));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4987));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5129));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5137));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5185));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5187));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5118));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5438));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5441));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5443));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5445));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5448));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5450));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5451));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5454));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5456));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(5457));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4861));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4871));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4877));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4879), 2 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4885));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4888));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 5, 15, 31, 21, 268, DateTimeKind.Utc).AddTicks(4890));
        }
    }
}
