using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminIdToGroupChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3217fd1-643d-40c1-a144-747b0a7aa0f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb29aad4-ba8c-42ff-bb3f-0dcbd5e52485");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupMembers");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "284d9ca2-bf84-42aa-92b5-eaecb4f68036", null, "User", "USER" },
                    { "f81e4dc9-6a73-40e8-93b3-2e059da29072", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "284d9ca2-bf84-42aa-92b5-eaecb4f68036");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f81e4dc9-6a73-40e8-93b3-2e059da29072");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f3217fd1-643d-40c1-a144-747b0a7aa0f9", null, "Admin", "ADMIN" },
                    { "fb29aad4-ba8c-42ff-bb3f-0dcbd5e52485", null, "User", "USER" }
                });
        }
    }
}
