using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acacia_Back_End.Migrations
{
    /// <inheritdoc />
    public partial class feature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_GiftBoxes_GiftBoxId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "GiftBoxId",
                table: "Products",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_GiftBoxId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TresholdValue",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GiftBoxId",
                table: "GiftBoxPrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GiftBoxProduct",
                columns: table => new
                {
                    GiftBoxesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftBoxProduct", x => new { x.GiftBoxesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_GiftBoxProduct_GiftBoxes_GiftBoxesId",
                        column: x => x.GiftBoxesId,
                        principalTable: "GiftBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftBoxProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftBoxProduct_ProductsId",
                table: "GiftBoxProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices",
                column: "GiftBoxId",
                principalTable: "GiftBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "GiftBoxProduct");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TresholdValue",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Products",
                newName: "GiftBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                newName: "IX_Products_GiftBoxId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "GiftBoxId",
                table: "GiftBoxPrices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices",
                column: "GiftBoxId",
                principalTable: "GiftBoxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GiftBoxes_GiftBoxId",
                table: "Products",
                column: "GiftBoxId",
                principalTable: "GiftBoxes",
                principalColumn: "Id");
        }
    }
}
