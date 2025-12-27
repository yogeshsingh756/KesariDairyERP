using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductionBatch1095 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerUnit",
                table: "production_batches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPerUnit",
                table: "production_batches",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
