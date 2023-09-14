using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acacia_Back_End.Migrations
{
    /// <inheritdoc />
    public partial class fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftBoxes_GiftBoxPrices_GiftBoxPriceId",
                table: "GiftBoxes");

            migrationBuilder.DropIndex(
                name: "IX_GiftBoxes_GiftBoxPriceId",
                table: "GiftBoxes");

            migrationBuilder.DropColumn(
                name: "GiftBoxPriceId",
                table: "GiftBoxes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "GiftBoxPrices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "GiftBoxId",
                table: "GiftBoxPrices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftBoxPrices_GiftBoxId",
                table: "GiftBoxPrices",
                column: "GiftBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices",
                column: "GiftBoxId",
                principalTable: "GiftBoxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftBoxPrices_GiftBoxes_GiftBoxId",
                table: "GiftBoxPrices");

            migrationBuilder.DropIndex(
                name: "IX_GiftBoxPrices_GiftBoxId",
                table: "GiftBoxPrices");

            migrationBuilder.DropColumn(
                name: "GiftBoxId",
                table: "GiftBoxPrices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "GiftBoxPrices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiftBoxPriceId",
                table: "GiftBoxes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GiftBoxes_GiftBoxPriceId",
                table: "GiftBoxes",
                column: "GiftBoxPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftBoxes_GiftBoxPrices_GiftBoxPriceId",
                table: "GiftBoxes",
                column: "GiftBoxPriceId",
                principalTable: "GiftBoxPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
