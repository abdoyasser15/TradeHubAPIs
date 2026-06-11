using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddBasketItemOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasketItemOptions",
                columns: table => new
                {
                    BaskItemOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasketItemId = table.Column<int>(type: "int", nullable: false),
                    ProductOptionValueId = table.Column<int>(type: "int", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValueName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItemOptions", x => x.BaskItemOptionId);
                    table.ForeignKey(
                        name: "FK_BasketItemOptions_BasketItems_BasketItemId",
                        column: x => x.BasketItemId,
                        principalTable: "BasketItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItemOptions_ProductOptionValues_ProductOptionValueId",
                        column: x => x.ProductOptionValueId,
                        principalTable: "ProductOptionValues",
                        principalColumn: "ProductOptionValueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemOptions_BasketItemId",
                table: "BasketItemOptions",
                column: "BasketItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItemOptions_ProductOptionValueId",
                table: "BasketItemOptions",
                column: "ProductOptionValueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItemOptions");
        }
    }
}
