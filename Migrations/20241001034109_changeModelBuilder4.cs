using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaServer.Migrations
{
    /// <inheritdoc />
    public partial class changeModelBuilder4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messenge",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sent_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    ReplyToId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messenge", x => x.id);
                    table.ForeignKey(
                        name: "FK_Messenge_Messenge_ReplyToId",
                        column: x => x.ReplyToId,
                        principalTable: "Messenge",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Messenge_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messenge_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessengeMediaContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Media_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Media_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessengeMediaContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessengeMediaContent_Messenge_MessengeId",
                        column: x => x.MessengeId,
                        principalTable: "Messenge",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messenge_ReceiverId",
                table: "Messenge",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messenge_ReplyToId",
                table: "Messenge",
                column: "ReplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_Messenge_SenderId",
                table: "Messenge",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MessengeMediaContent_MessengeId",
                table: "MessengeMediaContent",
                column: "MessengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessengeMediaContent");

            migrationBuilder.DropTable(
                name: "Messenge");
        }
    }
}
