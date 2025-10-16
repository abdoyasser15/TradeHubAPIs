using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountType", "CompanyId", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a2b3c4d5-e6f7-489a-b123-4567c89def01", 0, 1, null, "adb54bc1-64a4-47c9-a9bc-8da5d0761e06", new DateTime(2025, 9, 12, 23, 26, 31, 313, DateTimeKind.Utc).AddTicks(1118), "admin@tradehub.com", true, "System", "Administrator", false, null, "ADMIN@TRADEHUB.COM", "ADMIN@TRADEHUB.COM", "AQAAAAIAAYagAAAAEMJNv5tReUcERi8ReF7C+lpELEM9sSzNvCxtZUiUJ6e8AYNIYiWd+6msXNm/SfOJ5w==", null, false, 0, "d1acb903-59e0-46b3-8a85-c306dee6440a", false, "admin@tradehub.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b9f5c49e-75f3-4b93-a12e-91d4c2c1b0f9", "a2b3c4d5-e6f7-489a-b123-4567c89def01" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
