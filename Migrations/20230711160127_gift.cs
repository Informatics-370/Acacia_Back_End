using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acacia_Back_End.Migrations
{
    /// <inheritdoc />
    public partial class gift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftBoxId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GiftBoxPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    PackagingCosts = table.Column<double>(type: "REAL", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftBoxPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    GiftBoxImage = table.Column<string>(type: "TEXT", nullable: true),
                    GiftBoxPriceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftBoxes_GiftBoxPrices_GiftBoxPriceId",
                        column: x => x.GiftBoxPriceId,
                        principalTable: "GiftBoxPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_GiftBoxId",
                table: "Products",
                column: "GiftBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftBoxes_GiftBoxPriceId",
                table: "GiftBoxes",
                column: "GiftBoxPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GiftBoxes_GiftBoxId",
                table: "Products",
                column: "GiftBoxId",
                principalTable: "GiftBoxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GiftBoxes_GiftBoxId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "GiftBoxes");

            migrationBuilder.DropTable(
                name: "GiftBoxPrices");

            migrationBuilder.DropIndex(
                name: "IX_Products_GiftBoxId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GiftBoxId",
                table: "Products");
        }
    }
}
