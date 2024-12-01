using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class editUserName2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "272fdb9c-df9f-4b78-bae7-9d3e784f767d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a075a32-2b50-40c3-8691-52719811ca50");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a4aa1f6-c997-4ad7-a8d4-d0a80e3b9e78", null, "Admin", "ADMIN" },
                    { "c0458f1c-1cd6-4a91-a001-5d094bd28d67", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "272fdb9c-df9f-4b78-bae7-9d3e784f767d", null, "Admin", "ADMIN" },
                    { "9a075a32-2b50-40c3-8691-52719811ca50", null, "User", "USER" }
                });
        }
    }
}
