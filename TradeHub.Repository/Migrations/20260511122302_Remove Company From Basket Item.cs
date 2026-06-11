using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyFromBasketItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "BasketItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "BasketItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
