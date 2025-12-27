using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductionBatch1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "production_batches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BatchQuantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BatchUnit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fat = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SNF = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ProcessingFeePerUnit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalIngredientCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalProcessingCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BatchDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_batches", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "production_batch_ingredients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductionBatchId = table.Column<long>(type: "bigint", nullable: false),
                    IngredientTypeId = table.Column<long>(type: "bigint", nullable: false),
                    QuantityUsed = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Unit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CostPerUnit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_production_batch_ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_production_batch_ingredients_IngredientType_IngredientTypeId",
                        column: x => x.IngredientTypeId,
                        principalTable: "IngredientType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_production_batch_ingredients_production_batches_ProductionBa~",
                        column: x => x.ProductionBatchId,
                        principalTable: "production_batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_production_batch_ingredients_IngredientTypeId",
                table: "production_batch_ingredients",
                column: "IngredientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_production_batch_ingredients_ProductionBatchId",
                table: "production_batch_ingredients",
                column: "ProductionBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "production_batch_ingredients");

            migrationBuilder.DropTable(
                name: "production_batches");
        }
    }
}
