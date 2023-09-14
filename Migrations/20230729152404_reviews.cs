using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acacia_Back_End.Migrations
{
    /// <inheritdoc />
    public partial class reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_ProductReviewStatuses_ProductReviewStatusId",
                table: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ProductReviewStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductReviewStatusId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "ProductReviewStatusId",
                table: "ProductReviews",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "ProductReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ProductReviews",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ProductReviews",
                newName: "ProductReviewStatusId");

            migrationBuilder.CreateTable(
                name: "ProductReviewStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviewStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductReviewStatusId",
                table: "ProductReviews",
                column: "ProductReviewStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_ProductReviewStatuses_ProductReviewStatusId",
                table: "ProductReviews",
                column: "ProductReviewStatusId",
                principalTable: "ProductReviewStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
