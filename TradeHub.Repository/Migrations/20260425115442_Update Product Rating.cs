using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRaiting");

            migrationBuilder.CreateTable(
                name: "productRatings",
                columns: table => new
                {
                    RaitingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RaitngValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productRatings", x => x.RaitingId);
                    table.ForeignKey(
                        name: "FK_productRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productRatings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productRatings_ProductId_UserId",
                table: "productRatings",
                columns: new[] { "ProductId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_productRatings_UserId",
                table: "productRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productRatings");

            migrationBuilder.CreateTable(
                name: "ProductRaiting",
                columns: table => new
                {
                    RaitingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RaitingValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRaiting", x => x.RaitingId);
                    table.ForeignKey(
                        name: "FK_ProductRaiting_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRaiting_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRaiting_ProductId",
                table: "ProductRaiting",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRaiting_UserId",
                table: "ProductRaiting",
                column: "UserId");
        }
    }
}
