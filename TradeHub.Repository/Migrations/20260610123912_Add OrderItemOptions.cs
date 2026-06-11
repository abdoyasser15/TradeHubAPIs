using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItemOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    ProductOptionValueId = table.Column<int>(type: "int", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValueName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExtraPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemOption_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOption_OrderItemId",
                table: "OrderItemOption",
                column: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemOption");
        }
    }
}
