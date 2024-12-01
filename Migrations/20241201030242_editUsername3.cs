using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class editUsername3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a4aa1f6-c997-4ad7-a8d4-d0a80e3b9e78");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0458f1c-1cd6-4a91-a001-5d094bd28d67");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f98ae56-b403-4365-8752-d3ca06e189a9", null, "User", "USER" },
                    { "548ceb56-7dc7-49f2-83ea-6bea43ffae14", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f98ae56-b403-4365-8752-d3ca06e189a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "548ceb56-7dc7-49f2-83ea-6bea43ffae14");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a4aa1f6-c997-4ad7-a8d4-d0a80e3b9e78", null, "Admin", "ADMIN" },
                    { "c0458f1c-1cd6-4a91-a001-5d094bd28d67", null, "User", "USER" }
                });
        }
    }
}
