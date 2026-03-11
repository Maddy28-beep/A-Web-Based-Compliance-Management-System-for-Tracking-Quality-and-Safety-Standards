using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMasterDataLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RegulatoryReports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProcurementOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ComplianceChecks");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierCategoryId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("eeeeeeee-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReportStatusId",
                table: "RegulatoryReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementStatusId",
                table: "ProcurementOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("88888888-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyCategoryId",
                table: "Policies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("ffffffff-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<Guid>(
                name: "ComplianceCategoryId",
                table: "ComplianceChecks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("abababab-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<Guid>(
                name: "ComplianceStatusId",
                table: "ComplianceChecks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.AddColumn<Guid>(
                name: "RiskLevelId",
                table: "ComplianceChecks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("cdcdcdcd-1111-1111-1111-111111111111"));

            migrationBuilder.CreateTable(
                name: "ComplianceCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierCategories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1341));

            migrationBuilder.InsertData(
                table: "ComplianceCategories",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("abababab-1111-1111-1111-111111111111"), null, true, "Safety", 1 },
                    { new Guid("abababab-2222-2222-2222-222222222222"), null, true, "Quality", 2 },
                    { new Guid("abababab-3333-3333-3333-333333333333"), null, true, "Supplier", 3 },
                    { new Guid("abababab-4444-4444-4444-444444444444"), null, true, "Regulatory", 4 }
                });

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ComplianceCategoryId", "ComplianceStatusId", "CreatedAtUtc", "RiskLevelId" },
                values: new object[] { new Guid("abababab-2222-2222-2222-222222222222"), new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1320), new Guid("cdcdcdcd-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ComplianceCategoryId", "ComplianceStatusId", "CreatedAtUtc", "RiskLevelId" },
                values: new object[] { new Guid("abababab-1111-1111-1111-111111111111"), new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1322), new Guid("cdcdcdcd-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                table: "ComplianceStatuses",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Pending", 1 },
                    { new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Compliant", 2 },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Non-Compliant", 3 }
                });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAtUtc", "PolicyCategoryId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1226), new Guid("ffffffff-2222-2222-2222-222222222222") });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAtUtc", "PolicyCategoryId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1230), new Guid("ffffffff-1111-1111-1111-111111111111") });

            migrationBuilder.InsertData(
                table: "PolicyCategories",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("ffffffff-1111-1111-1111-111111111111"), null, true, "Safety", 1 },
                    { new Guid("ffffffff-2222-2222-2222-222222222222"), null, true, "Quality", 2 },
                    { new Guid("ffffffff-3333-3333-3333-333333333333"), null, true, "Procurement", 3 },
                    { new Guid("ffffffff-4444-4444-4444-444444444444"), null, true, "Environmental", 4 }
                });

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1295));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1297));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAtUtc", "ProcurementStatusId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1272), new Guid("aaaaaaaa-bbbb-bbbb-bbbb-bbbbbbbbbbbb") });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CreatedAtUtc", "ProcurementStatusId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1275), new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.InsertData(
                table: "ProcurementStatuses",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("88888888-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Draft", 1 },
                    { new Guid("99999999-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Approved", 2 },
                    { new Guid("aaaaaaaa-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, true, "Ordered", 3 },
                    { new Guid("bbbbbbbb-cccc-cccc-cccc-cccccccccccc"), null, true, "Received", 4 },
                    { new Guid("cccccccc-dddd-dddd-dddd-dddddddddddd"), null, true, "Closed", 5 },
                    { new Guid("dddddddd-eeee-eeee-eeee-eeeeeeeeeeee"), null, true, "Cancelled", 6 }
                });

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "CreatedAtUtc", "ReportStatusId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1359), new Guid("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.InsertData(
                table: "ReportStatuses",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Draft", 1 },
                    { new Guid("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Submitted", 2 },
                    { new Guid("66666666-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Accepted", 3 },
                    { new Guid("77777777-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, true, "Rejected", 4 }
                });

            migrationBuilder.InsertData(
                table: "RiskLevels",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("cdcdcdcd-1111-1111-1111-111111111111"), null, true, "Low", 1 },
                    { new Guid("cdcdcdcd-2222-2222-2222-222222222222"), null, true, "Medium", 2 },
                    { new Guid("cdcdcdcd-3333-3333-3333-333333333333"), null, true, "High", 3 },
                    { new Guid("cdcdcdcd-4444-4444-4444-444444444444"), null, true, "Critical", 4 }
                });

            migrationBuilder.InsertData(
                table: "SupplierCategories",
                columns: new[] { "Id", "Description", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("eeeeeeee-1111-1111-1111-111111111111"), null, true, "Raw Materials", 1 },
                    { new Guid("eeeeeeee-2222-2222-2222-222222222222"), null, true, "Hardware", 2 },
                    { new Guid("eeeeeeee-3333-3333-3333-333333333333"), null, true, "Finishing", 3 },
                    { new Guid("eeeeeeee-4444-4444-4444-444444444444"), null, true, "Packaging", 4 }
                });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAtUtc", "SupplierCategoryId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1250), new Guid("eeeeeeee-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAtUtc", "SupplierCategoryId" },
                values: new object[] { new DateTime(2026, 2, 6, 18, 33, 14, 402, DateTimeKind.Utc).AddTicks(1253), new Guid("eeeeeeee-2222-2222-2222-222222222222") });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCategoryId",
                table: "Suppliers",
                column: "SupplierCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_ReportStatusId",
                table: "RegulatoryReports",
                column: "ReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementOrders_ProcurementStatusId",
                table: "ProcurementOrders",
                column: "ProcurementStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_PolicyCategoryId",
                table: "Policies",
                column: "PolicyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceChecks_ComplianceCategoryId",
                table: "ComplianceChecks",
                column: "ComplianceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceChecks_ComplianceStatusId",
                table: "ComplianceChecks",
                column: "ComplianceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceChecks_RiskLevelId",
                table: "ComplianceChecks",
                column: "RiskLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplianceChecks_ComplianceCategories_ComplianceCategoryId",
                table: "ComplianceChecks",
                column: "ComplianceCategoryId",
                principalTable: "ComplianceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplianceChecks_ComplianceStatuses_ComplianceStatusId",
                table: "ComplianceChecks",
                column: "ComplianceStatusId",
                principalTable: "ComplianceStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplianceChecks_RiskLevels_RiskLevelId",
                table: "ComplianceChecks",
                column: "RiskLevelId",
                principalTable: "RiskLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_PolicyCategories_PolicyCategoryId",
                table: "Policies",
                column: "PolicyCategoryId",
                principalTable: "PolicyCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementOrders_ProcurementStatuses_ProcurementStatusId",
                table: "ProcurementOrders",
                column: "ProcurementStatusId",
                principalTable: "ProcurementStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegulatoryReports_ReportStatuses_ReportStatusId",
                table: "RegulatoryReports",
                column: "ReportStatusId",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_SupplierCategories_SupplierCategoryId",
                table: "Suppliers",
                column: "SupplierCategoryId",
                principalTable: "SupplierCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplianceChecks_ComplianceCategories_ComplianceCategoryId",
                table: "ComplianceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplianceChecks_ComplianceStatuses_ComplianceStatusId",
                table: "ComplianceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplianceChecks_RiskLevels_RiskLevelId",
                table: "ComplianceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_PolicyCategories_PolicyCategoryId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementOrders_ProcurementStatuses_ProcurementStatusId",
                table: "ProcurementOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_RegulatoryReports_ReportStatuses_ReportStatusId",
                table: "RegulatoryReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_SupplierCategories_SupplierCategoryId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "ComplianceCategories");

            migrationBuilder.DropTable(
                name: "ComplianceStatuses");

            migrationBuilder.DropTable(
                name: "PolicyCategories");

            migrationBuilder.DropTable(
                name: "ProcurementStatuses");

            migrationBuilder.DropTable(
                name: "ReportStatuses");

            migrationBuilder.DropTable(
                name: "RiskLevels");

            migrationBuilder.DropTable(
                name: "SupplierCategories");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierCategoryId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_RegulatoryReports_ReportStatusId",
                table: "RegulatoryReports");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementOrders_ProcurementStatusId",
                table: "ProcurementOrders");

            migrationBuilder.DropIndex(
                name: "IX_Policies_PolicyCategoryId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_ComplianceChecks_ComplianceCategoryId",
                table: "ComplianceChecks");

            migrationBuilder.DropIndex(
                name: "IX_ComplianceChecks_ComplianceStatusId",
                table: "ComplianceChecks");

            migrationBuilder.DropIndex(
                name: "IX_ComplianceChecks_RiskLevelId",
                table: "ComplianceChecks");

            migrationBuilder.DropColumn(
                name: "SupplierCategoryId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ReportStatusId",
                table: "RegulatoryReports");

            migrationBuilder.DropColumn(
                name: "ProcurementStatusId",
                table: "ProcurementOrders");

            migrationBuilder.DropColumn(
                name: "PolicyCategoryId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "ComplianceCategoryId",
                table: "ComplianceChecks");

            migrationBuilder.DropColumn(
                name: "ComplianceStatusId",
                table: "ComplianceChecks");

            migrationBuilder.DropColumn(
                name: "RiskLevelId",
                table: "ComplianceChecks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RegulatoryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProcurementOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ComplianceChecks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9866), 1 });

            migrationBuilder.UpdateData(
                table: "ComplianceChecks",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9868), 2 });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "ProcurementItems",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9845));

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9823), 2 });

            migrationBuilder.UpdateData(
                table: "ProcurementOrders",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9826), 1 });

            migrationBuilder.UpdateData(
                table: "RegulatoryReports",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                columns: new[] { "CreatedAtUtc", "Status" },
                values: new object[] { new DateTime(2026, 2, 5, 15, 19, 13, 194, DateTimeKind.Utc).AddTicks(45), 1 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAtUtc",
                value: new DateTime(2026, 2, 5, 15, 19, 13, 193, DateTimeKind.Utc).AddTicks(9797));
        }
    }
}
