using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyRatings",
                columns: table => new
                {
                    RaitingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRatings", x => x.RaitingId);
                    table.ForeignKey(
                        name: "FK_CompanyRatings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRatings_CompanyId_UserId",
                table: "CompanyRatings",
                columns: new[] { "CompanyId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyRatings");
        }
    }
}
