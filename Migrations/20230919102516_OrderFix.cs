using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acacia_Back_End.Migrations
{
    /// <inheritdoc />
    public partial class OrderFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Savings",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VATId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VATId",
                table: "Orders",
                column: "VATId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vats_VATId",
                table: "Orders",
                column: "VATId",
                principalTable: "Vats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vats_VATId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VATId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Savings",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VATId",
                table: "Orders");
        }
    }
}
