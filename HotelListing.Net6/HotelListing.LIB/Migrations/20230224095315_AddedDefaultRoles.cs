using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5eda1c45-03cd-4394-a03c-1ccdcd3ef1cd", "e9a1bf69-d300-4ef4-b95d-d7e5c5de3359", "Administrator", "ADMINISTRATOR" },
                    { "90aa18ed-b140-46e2-87a5-47ec24cbc728", "9de3a7e0-bbda-49f6-8ed9-29638c1ceaa5", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eda1c45-03cd-4394-a03c-1ccdcd3ef1cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90aa18ed-b140-46e2-87a5-47ec24cbc728");
        }
    }
}
