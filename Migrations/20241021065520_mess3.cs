using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class mess3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73273901-7ae5-4db1-a9cf-a49fbec0719b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74afaa44-d66a-48b9-b8a2-73648730cee0");

            migrationBuilder.AlterColumn<int>(
                name: "ReplyToId",
                table: "Messenges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b5f7ff6-0e09-432e-80dd-5834d4c588be", null, "User", "USER" },
                    { "81e9d498-e663-43b9-b526-19355b9eb0d7", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b5f7ff6-0e09-432e-80dd-5834d4c588be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81e9d498-e663-43b9-b526-19355b9eb0d7");

            migrationBuilder.AlterColumn<int>(
                name: "ReplyToId",
                table: "Messenges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "73273901-7ae5-4db1-a9cf-a49fbec0719b", null, "User", "USER" },
                    { "74afaa44-d66a-48b9-b8a2-73648730cee0", null, "Admin", "ADMIN" }
                });
        }
    }
}
