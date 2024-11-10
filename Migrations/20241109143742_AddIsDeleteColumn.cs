using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "284d9ca2-bf84-42aa-92b5-eaecb4f68036");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f81e4dc9-6a73-40e8-93b3-2e059da29072");

            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "GroupMessenges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeft",
                table: "GroupMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Left_at",
                table: "GroupMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b7887952-1ba1-4f93-be89-77c9ffe9d17d", null, "User", "USER" },
                    { "c7388fde-4697-465d-b6f0-939cbbb4ce46", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7887952-1ba1-4f93-be89-77c9ffe9d17d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7388fde-4697-465d-b6f0-939cbbb4ce46");

            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "GroupMessenges");

            migrationBuilder.DropColumn(
                name: "IsLeft",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "Left_at",
                table: "GroupMembers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "284d9ca2-bf84-42aa-92b5-eaecb4f68036", null, "User", "USER" },
                    { "f81e4dc9-6a73-40e8-93b3-2e059da29072", null, "Admin", "ADMIN" }
                });
        }
    }
}
