using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCategories_SubCategories_CategoryId",
                table: "CompanyCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCategories_Categories_CategoryId",
                table: "CompanyCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCategories_Categories_CategoryId",
                table: "CompanyCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCategories_SubCategories_CategoryId",
                table: "CompanyCategories",
                column: "CategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
