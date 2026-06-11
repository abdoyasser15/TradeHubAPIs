using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExtraPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ExtraPrice",
                table: "BasketItemOptions",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraPrice",
                table: "BasketItemOptions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
