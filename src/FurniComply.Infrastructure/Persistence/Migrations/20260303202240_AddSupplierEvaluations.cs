using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurniComply.Infrastructure.Persistence.Migrations
{
    public partial class AddSupplierEvaluations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierEvaluations",
                columns: table => new
                {
                    SupplierEvaluationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    EvaluatedByEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EvaluatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScoreSnapshot = table.Column<int>(type: "int", nullable: false),
                    BandSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReasonsSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierEvaluations", x => x.SupplierEvaluationId);
                    table.ForeignKey(
                        name: "FK_SupplierEvaluations_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierEvaluations_SupplierId_EvaluatedAtUtc",
                table: "SupplierEvaluations",
                columns: new[] { "SupplierId", "EvaluatedAtUtc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierEvaluations");
        }
    }
}
