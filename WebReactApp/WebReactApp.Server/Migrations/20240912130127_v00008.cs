using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class v00008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountItemsParameters_AccountPostEnclosures_AccountPostEncl~",
                table: "AccountItemsParameters");

            migrationBuilder.DropIndex(
                name: "IX_AccountItemsParameters_AccountPostEnclosureID",
                table: "AccountItemsParameters");

            migrationBuilder.DropColumn(
                name: "AccountPostEnclosureID",
                table: "AccountItemsParameters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountPostEnclosureID",
                table: "AccountItemsParameters",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItemsParameters_AccountPostEnclosureID",
                table: "AccountItemsParameters",
                column: "AccountPostEnclosureID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountItemsParameters_AccountPostEnclosures_AccountPostEncl~",
                table: "AccountItemsParameters",
                column: "AccountPostEnclosureID",
                principalTable: "AccountPostEnclosures",
                principalColumn: "ID");
        }
    }
}
