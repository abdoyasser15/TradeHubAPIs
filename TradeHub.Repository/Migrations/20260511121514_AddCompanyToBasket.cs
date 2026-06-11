using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyToBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRatings_UserId",
                table: "CompanyRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CompanyId",
                table: "Baskets",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Companies_CompanyId",
                table: "Baskets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyRatings_AspNetUsers_UserId",
                table: "CompanyRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Companies_CompanyId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyRatings_AspNetUsers_UserId",
                table: "CompanyRatings");

            migrationBuilder.DropIndex(
                name: "IX_CompanyRatings_UserId",
                table: "CompanyRatings");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_CompanyId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Baskets");
        }
    }
}
