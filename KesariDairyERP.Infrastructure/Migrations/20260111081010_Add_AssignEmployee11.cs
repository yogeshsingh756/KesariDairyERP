using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_AssignEmployee11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_product_assignments_production_batches_ProductionBa~",
                table: "employee_product_assignments");

            migrationBuilder.DropIndex(
                name: "IX_employee_product_assignments_ProductionBatchId",
                table: "employee_product_assignments");

            migrationBuilder.DropColumn(
                name: "ProductionBatchId",
                table: "employee_product_assignments");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPricePerUnit",
                table: "employee_product_assignments",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "employee_product_assignments",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPricePerUnit",
                table: "employee_product_assignments");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "employee_product_assignments");

            migrationBuilder.AddColumn<long>(
                name: "ProductionBatchId",
                table: "employee_product_assignments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_product_assignments_ProductionBatchId",
                table: "employee_product_assignments",
                column: "ProductionBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_product_assignments_production_batches_ProductionBa~",
                table: "employee_product_assignments",
                column: "ProductionBatchId",
                principalTable: "production_batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
