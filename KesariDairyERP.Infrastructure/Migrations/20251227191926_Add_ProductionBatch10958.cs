using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductionBatch10958 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "production_batches",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_production_batches_ProductId",
                table: "production_batches",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_production_batches_ProductType_ProductId",
                table: "production_batches",
                column: "ProductId",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_production_batches_ProductType_ProductId",
                table: "production_batches");

            migrationBuilder.DropIndex(
                name: "IX_production_batches_ProductId",
                table: "production_batches");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "production_batches",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
