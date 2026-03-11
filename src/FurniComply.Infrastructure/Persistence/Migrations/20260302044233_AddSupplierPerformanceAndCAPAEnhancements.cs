using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierPerformanceAndCAPAEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_ComplianceChecks_ComplianceCheckId",
                table: "CorrectiveActions");

            migrationBuilder.AddColumn<decimal>(
                name: "PerformanceScore",
                table: "Suppliers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "RiskLevelId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplianceCheckId",
                table: "CorrectiveActions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "CorrectiveActionPlan",
                table: "CorrectiveActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IssueType",
                table: "CorrectiveActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PreventiveActionPlan",
                table: "CorrectiveActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "CorrectiveActions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupplierPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QualityRating = table.Column<int>(type: "int", nullable: false),
                    DeliveryRating = table.Column<int>(type: "int", nullable: false),
                    DefectRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerformanceScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvaluatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierPerformances_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9708), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9723), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9726), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9733), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9736), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9741), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9744), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9755), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9761), 0, null, null });

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                columns: new[] { "CorrectiveActionPlan", "CreatedAtUtc", "IssueType", "PreventiveActionPlan", "SupplierId" },
                values: new object[] { null, new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(9763), 0, null, null });

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
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8780), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8800), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8804), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8808), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8814), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8817), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8823), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8826), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8829), 0m, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "PerformanceScore", "RiskLevelId" },
                values: new object[] { new DateTime(2026, 3, 2, 4, 42, 30, 212, DateTimeKind.Utc).AddTicks(8836), 0m, null });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_RiskLevelId",
                table: "Suppliers",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrectiveActions_SupplierId",
                table: "CorrectiveActions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPerformances_SupplierId",
                table: "SupplierPerformances",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_ComplianceChecks_ComplianceCheckId",
                table: "CorrectiveActions",
                column: "ComplianceCheckId",
                principalTable: "ComplianceChecks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_Suppliers_SupplierId",
                table: "CorrectiveActions",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_RiskLevels_RiskLevelId",
                table: "Suppliers",
                column: "RiskLevelId",
                principalTable: "RiskLevels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_ComplianceChecks_ComplianceCheckId",
                table: "CorrectiveActions");

            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_Suppliers_SupplierId",
                table: "CorrectiveActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_RiskLevels_RiskLevelId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "SupplierPerformances");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_RiskLevelId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_CorrectiveActions_SupplierId",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "PerformanceScore",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RiskLevelId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CorrectiveActionPlan",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "IssueType",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "PreventiveActionPlan",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "CorrectiveActions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplianceCheckId",
                table: "CorrectiveActions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("00000000-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("11111111-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4860));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("22222222-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4862));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4867));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("55555555-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4869));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4848));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4856));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("1111eeee-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5191));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("2222eeee-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5194));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("3333eeee-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5196));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("4444eeee-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5198));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5200));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("6666eeee-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5201));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("7777eeee-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5202));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("8888eeee-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5204));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("9999eeee-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5207));

            migrationBuilder.UpdateData(
                table: "ComplianceAlertRules",
                keyColumn: "Id",
                keyValue: new Guid("aaaaeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5208));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("1111ffff-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("2222ffff-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5242));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("3333ffff-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5244));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("4444ffff-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("5555ffff-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5286));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("7777ffff-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("8888ffff-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5292));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("9999ffff-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5295));

            migrationBuilder.UpdateData(
                table: "ComplianceAlerts",
                keyColumn: "Id",
                keyValue: new Guid("aaaaffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5297));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("00000000-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4812));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4819));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4810));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("1111aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5135));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("4444dddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5138));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("5555eeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("6666ffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5141));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("77770000-0000-0000-0000-000000000000"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("88881111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5145));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("99992222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5147));

            migrationBuilder.UpdateData(
                table: "CorrectiveActions",
                keyColumn: "Id",
                keyValue: new Guid("aaaa3333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5149));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4543));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4552));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4556));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4558));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4560));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4563));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4574));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("1111bbbb-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5407));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("2222bbbb-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5412));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("3333bbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5416));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("4444bbbb-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("5555bbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5419));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("6666bbbb-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5421));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("7777bbbb-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5423));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("8888bbbb-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("9999bbbb-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5426));

            migrationBuilder.UpdateData(
                table: "PolicyVersions",
                keyColumn: "Id",
                keyValue: new Guid("aaaabbbb-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5428));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("00007777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5091));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5069));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5076));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5078));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5080));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5083));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5085));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("eeee5555-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("ffff6666-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4672));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("77777777-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4684));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("88888888-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4743));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("99999999-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4747));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4751));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4753));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("00000000-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4909));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("11111111-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4912));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("22222222-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("33333333-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("44444444-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("55555555-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4921));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4905));

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-dddd-dddd-dddd-dddddddddddd"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4907));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("1111cccc-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5351));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("2222cccc-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5357));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("3333cccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5360));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("4444cccc-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5362));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("5555cccc-5555-5555-5555-555555555555"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5366));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("6666cccc-6666-6666-6666-666666666666"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5367));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("7777cccc-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("8888cccc-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("9999cccc-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "SupplierComplianceDocuments",
                keyColumn: "Id",
                keyValue: new Guid("aaaacccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(5376));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4618));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("66666666-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4620));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("88888888-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4624));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("99999999-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 3, 1, 17, 50, 29, 549, DateTimeKind.Utc).AddTicks(4633));

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_ComplianceChecks_ComplianceCheckId",
                table: "CorrectiveActions",
                column: "ComplianceCheckId",
                principalTable: "ComplianceChecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
