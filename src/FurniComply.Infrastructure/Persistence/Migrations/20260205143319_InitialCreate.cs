using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegulatoryReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriodStartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEndUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulatoryReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Certifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplianceChecks_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcurementOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QualityStandard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementItems_ProcurementOrders_ProcurementOrderId",
                        column: x => x.ProcurementOrderId,
                        principalTable: "ProcurementOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuditLogs",
                columns: new[] { "Id", "Action", "Actor", "CreatedAtUtc", "Details", "EntityId", "EntityName", "TimestampUtc", "UpdatedAtUtc" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Marked Non-Compliant", "Safety Officer", new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(518), "Issue logged and corrective action requested.", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "ComplianceCheck", new DateTime(2026, 2, 1, 14, 30, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "Id", "Code", "Content", "CreatedAtUtc", "EffectiveDateUtc", "Owner", "Status", "Title", "UpdatedAtUtc", "Version" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "QMS-001", "Defines minimum quality criteria for raw materials.", new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(241), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality Manager", 1, "Material Quality Standard", null, "1.2" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "SAFE-010", "Defines safety practices for production areas.", new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(244), new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Safety Officer", 1, "Workshop Safety Standard", null, "2.0" }
                });

            migrationBuilder.InsertData(
                table: "RegulatoryReports",
                columns: new[] { "Id", "CreatedAtUtc", "PeriodEndUtc", "PeriodStartUtc", "ReportType", "Status", "SubmittedAtUtc", "Summary", "UpdatedAtUtc" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(537), new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monthly Safety Summary", 1, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "January safety report submitted to regulator.", null });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Certifications", "ContactEmail", "CreatedAtUtc", "Name", "Rating", "Status", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "FSC, ISO 9001", "qa@oakline.example", new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(412), "Oakline Timber Co.", 4.6m, 0, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "ISO 14001", "quality@precision.example", new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(417), "Precision Hardware Ltd.", 3.2m, 1, null }
                });

            migrationBuilder.InsertData(
                table: "ComplianceChecks",
                columns: new[] { "Id", "CheckedAtUtc", "CreatedAtUtc", "Notes", "PolicyId", "Status", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(497), "Supplier material certification verified.", new Guid("11111111-1111-1111-1111-111111111111"), 1, null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(499), "Safety signage missing in finishing bay.", new Guid("22222222-2222-2222-2222-222222222222"), 2, null }
                });

            migrationBuilder.InsertData(
                table: "ProcurementOrders",
                columns: new[] { "Id", "CreatedAtUtc", "OrderDateUtc", "OrderNumber", "Status", "SupplierId", "TotalAmount", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(438), new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "PO-2026-0001", 2, new Guid("33333333-3333-3333-3333-333333333333"), 18500m, null },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(442), new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "PO-2026-0002", 1, new Guid("44444444-4444-4444-4444-444444444444"), 9400m, null }
                });

            migrationBuilder.InsertData(
                table: "ProcurementItems",
                columns: new[] { "Id", "CreatedAtUtc", "ItemName", "Notes", "ProcurementOrderId", "QualityStandard", "Quantity", "UnitCost", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(460), "Premium Oak Planks", "Moisture content under 8%.", new Guid("55555555-5555-5555-5555-555555555555"), "QMS-001", 120, 95m, null },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(477), "Protective Finish", "VOC compliant finish.", new Guid("55555555-5555-5555-5555-555555555555"), "SAFE-010", 60, 35m, null },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 2, 5, 14, 33, 19, 412, DateTimeKind.Utc).AddTicks(479), "Steel Brackets", "Batch to be inspected on arrival.", new Guid("66666666-6666-6666-6666-666666666666"), "QMS-001", 450, 12m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceChecks_PolicyId",
                table: "ComplianceChecks",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementItems_ProcurementOrderId",
                table: "ProcurementItems",
                column: "ProcurementOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementOrders_SupplierId",
                table: "ProcurementOrders",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ComplianceChecks");

            migrationBuilder.DropTable(
                name: "ProcurementItems");

            migrationBuilder.DropTable(
                name: "RegulatoryReports");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "ProcurementOrders");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
