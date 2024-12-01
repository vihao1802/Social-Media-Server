using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class editUserModel : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "TINYINT");

            migrationBuilder.AddColumn<byte>(
                name: "Is_external_user",
                table: "AspNetUsers",
                type: "TINYINT",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "643e6404-cd63-482b-9a51-aff22e7976fd", null, "User", "USER" },
                    { "a80ff027-80ac-4dad-aff9-2455719087a8", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "643e6404-cd63-482b-9a51-aff22e7976fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a80ff027-80ac-4dad-aff9-2455719087a8");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Is_external_user",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte>(
                name: "Gender",
                table: "AspNetUsers",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

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
