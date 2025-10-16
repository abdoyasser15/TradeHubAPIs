using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeHub.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a2b3c4d5-e6f7-489a-b123-4567c89def01",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "Role", "SecurityStamp" },
                values: new object[] { "95ae7380-f7c8-433f-9db6-d1d02c65e738", new DateTime(2025, 9, 12, 23, 28, 57, 827, DateTimeKind.Utc).AddTicks(9606), "AQAAAAIAAYagAAAAECDCbmMrh4hJfrm0S2MOJ3YxAt2pNjs23oHyG6nsnjSjJRafld9IMCaxQ96tZ0Zwyg==", 3, "790ebd1f-3584-4786-a38e-642842be9871" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a2b3c4d5-e6f7-489a-b123-4567c89def01",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "Role", "SecurityStamp" },
                values: new object[] { "adb54bc1-64a4-47c9-a9bc-8da5d0761e06", new DateTime(2025, 9, 12, 23, 26, 31, 313, DateTimeKind.Utc).AddTicks(1118), "AQAAAAIAAYagAAAAEMJNv5tReUcERi8ReF7C+lpELEM9sSzNvCxtZUiUJ6e8AYNIYiWd+6msXNm/SfOJ5w==", 0, "d1acb903-59e0-46b3-8a85-c306dee6440a" });
        }
    }
}
