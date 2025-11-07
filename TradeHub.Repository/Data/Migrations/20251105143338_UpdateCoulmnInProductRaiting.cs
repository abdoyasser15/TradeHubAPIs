using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCoulmnInProductRaiting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRaiting_AspNetUsers_UserId1",
                table: "ProductRaiting");

            migrationBuilder.DropIndex(
                name: "IX_ProductRaiting_UserId1",
                table: "ProductRaiting");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProductRaiting");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductRaiting",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRaiting_UserId",
                table: "ProductRaiting",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRaiting_AspNetUsers_UserId",
                table: "ProductRaiting",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRaiting_AspNetUsers_UserId",
                table: "ProductRaiting");

            migrationBuilder.DropIndex(
                name: "IX_ProductRaiting_UserId",
                table: "ProductRaiting");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ProductRaiting",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ProductRaiting",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRaiting_UserId1",
                table: "ProductRaiting",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRaiting_AspNetUsers_UserId1",
                table: "ProductRaiting",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
