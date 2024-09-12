using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class v00009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPostEnclosures_AccountPosts_AccountPostID",
                table: "AccountPostEnclosures");

            migrationBuilder.DropIndex(
                name: "IX_AccountPostEnclosures_AccountPostID",
                table: "AccountPostEnclosures");

            migrationBuilder.DropColumn(
                name: "AccountPostID",
                table: "AccountPostEnclosures");

            migrationBuilder.CreateTable(
                name: "AccountPostAccountPostEnclosure",
                columns: table => new
                {
                    AccountPostID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountPostenclosureID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPostAccountPostEnclosure", x => new { x.AccountPostID, x.AccountPostenclosureID });
                    table.ForeignKey(
                        name: "FK_AccountPostAccountPostEnclosure_AccountPostEnclosures_Accoun~",
                        column: x => x.AccountPostenclosureID,
                        principalTable: "AccountPostEnclosures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPostAccountPostEnclosure_AccountPosts_AccountPostID",
                        column: x => x.AccountPostID,
                        principalTable: "AccountPosts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostAccountPostEnclosure_AccountPostenclosureID",
                table: "AccountPostAccountPostEnclosure",
                column: "AccountPostenclosureID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPostAccountPostEnclosure");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountPostID",
                table: "AccountPostEnclosures",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostEnclosures_AccountPostID",
                table: "AccountPostEnclosures",
                column: "AccountPostID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPostEnclosures_AccountPosts_AccountPostID",
                table: "AccountPostEnclosures",
                column: "AccountPostID",
                principalTable: "AccountPosts",
                principalColumn: "ID");
        }
    }
}
