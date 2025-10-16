using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9", "a2b3c4d5-e6f7-489a-b123-4567c89def01" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a2b3c4d5-e6f7-489a-b123-4567c89def01");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_LocationId",
                table: "Companies",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Locations_LocationId",
                table: "Companies",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Locations_LocationId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Companies_LocationId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Companies",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountType", "CompanyId", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a2b3c4d5-e6f7-489a-b123-4567c89def01", 0, 1, null, "95ae7380-f7c8-433f-9db6-d1d02c65e738", new DateTime(2025, 9, 12, 23, 28, 57, 827, DateTimeKind.Utc).AddTicks(9606), "admin@tradehub.com", true, "System", "Administrator", false, null, "ADMIN@TRADEHUB.COM", "ADMIN@TRADEHUB.COM", "AQAAAAIAAYagAAAAECDCbmMrh4hJfrm0S2MOJ3YxAt2pNjs23oHyG6nsnjSjJRafld9IMCaxQ96tZ0Zwyg==", null, false, 3, "790ebd1f-3584-4786-a38e-642842be9871", false, "admin@tradehub.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9", "a2b3c4d5-e6f7-489a-b123-4567c89def01" });
        }
    }
}
