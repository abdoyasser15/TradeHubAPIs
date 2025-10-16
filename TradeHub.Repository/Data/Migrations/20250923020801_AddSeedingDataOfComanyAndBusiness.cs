using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TradeHub.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedingDataOfComanyAndBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "BusinessTypeId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Technology" },
                    { 2, true, "Retail" },
                    { 3, true, "Manufacturing" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Egypt" },
                    { 2, "USA" },
                    { 3, "KSU" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "BusinessName", "BusinessTypeId", "CreatedById", "LocationId", "LogoUrl", "TaxNumber" },
                values: new object[,]
                {
                    { new Guid("48b80bbf-75e2-434d-a940-8ee73e3d04ab"), "Mega Retail Co.", 2, "4a2dce06-ddb1-4297-8529-566ffa18b1cf", 2, "https://example.com/logos/megaretail.png", "TX67890" },
                    { new Guid("c4f45bd0-744e-4a54-b87c-208dbaee8a7c"), "Tech Hub Ltd.", 1, "4a2dce06-ddb1-4297-8529-566ffa18b1cf", 1, "https://example.com/logos/techhub.png", "TX12345" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("48b80bbf-75e2-434d-a940-8ee73e3d04ab"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("c4f45bd0-744e-4a54-b87c-208dbaee8a7c"));

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
