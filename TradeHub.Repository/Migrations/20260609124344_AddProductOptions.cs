using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    ProductOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultiple = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.ProductOptionId);
                    table.ForeignKey(
                        name: "FK_ProductOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValues",
                columns: table => new
                {
                    ProductOptionValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValues", x => x.ProductOptionValueId);
                    table.ForeignKey(
                        name: "FK_ProductOptionValues_ProductOptions_ProductOptionId",
                        column: x => x.ProductOptionId,
                        principalTable: "ProductOptions",
                        principalColumn: "ProductOptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionValues_ProductOptionId",
                table: "ProductOptionValues",
                column: "ProductOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOptionValues");

            migrationBuilder.DropTable(
                name: "ProductOptions");
        }
    }
}
