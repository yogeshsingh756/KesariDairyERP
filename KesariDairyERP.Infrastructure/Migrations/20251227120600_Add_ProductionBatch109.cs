using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductionBatch109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualCostPerUnit",
                table: "production_batches",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePricePerUnit",
                table: "production_batches",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPricePerUnit",
                table: "production_batches",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCostPerUnit",
                table: "production_batches");

            migrationBuilder.DropColumn(
                name: "BasePricePerUnit",
                table: "production_batches");

            migrationBuilder.DropColumn(
                name: "SellingPricePerUnit",
                table: "production_batches");
        }
    }
}
