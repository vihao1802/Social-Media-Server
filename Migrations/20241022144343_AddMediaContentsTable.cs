using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaContentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaContent_Posts_PostId",
                table: "MediaContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaContent",
                table: "MediaContent");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce91c250-c5a9-48f9-a194-784752768626");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5b52c54-7eb3-482e-a4fa-2194576257e5");

            migrationBuilder.RenameTable(
                name: "MediaContent",
                newName: "MediaContents");

            migrationBuilder.RenameIndex(
                name: "IX_MediaContent_PostId",
                table: "MediaContents",
                newName: "IX_MediaContents_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaContents",
                table: "MediaContents",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f3217fd1-643d-40c1-a144-747b0a7aa0f9", null, "Admin", "ADMIN" },
                    { "fb29aad4-ba8c-42ff-bb3f-0dcbd5e52485", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MediaContents_Posts_PostId",
                table: "MediaContents",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaContents_Posts_PostId",
                table: "MediaContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaContents",
                table: "MediaContents");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3217fd1-643d-40c1-a144-747b0a7aa0f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb29aad4-ba8c-42ff-bb3f-0dcbd5e52485");

            migrationBuilder.RenameTable(
                name: "MediaContents",
                newName: "MediaContent");

            migrationBuilder.RenameIndex(
                name: "IX_MediaContents_PostId",
                table: "MediaContent",
                newName: "IX_MediaContent_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaContent",
                table: "MediaContent",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ce91c250-c5a9-48f9-a194-784752768626", null, "User", "USER" },
                    { "f5b52c54-7eb3-482e-a4fa-2194576257e5", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MediaContent_Posts_PostId",
                table: "MediaContent",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
