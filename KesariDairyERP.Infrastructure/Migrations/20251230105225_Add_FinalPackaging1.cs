using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KesariDairyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_FinalPackaging1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualPackets",
                table: "batch_packaging",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CalculatedPackets",
                table: "batch_packaging",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DamagedPackets",
                table: "batch_packaging",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "batch_packaging",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualPackets",
                table: "batch_packaging");

            migrationBuilder.DropColumn(
                name: "CalculatedPackets",
                table: "batch_packaging");

            migrationBuilder.DropColumn(
                name: "DamagedPackets",
                table: "batch_packaging");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "batch_packaging");
        }
    }
}
