using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class v00007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountItems_AccountPosts_AccountPostID",
                table: "AccountItems");

            migrationBuilder.DropIndex(
                name: "IX_AccountItems_AccountPostID",
                table: "AccountItems");

            migrationBuilder.DropColumn(
                name: "AccountPostID",
                table: "AccountItems");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountPostEnclosureID",
                table: "AccountItemsParameters",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "AccountPostEnclosures",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ItemMetaName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpireAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AccountPostID = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPostEnclosures", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountPostEnclosures_AccountPosts_AccountPostID",
                        column: x => x.AccountPostID,
                        principalTable: "AccountPosts",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountPostEnclosuresItemParameters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountPostEnclosureID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParamName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    NumberValue = table.Column<int>(type: "int", nullable: false),
                    StringValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPostEnclosuresItemParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountPostEnclosuresItemParameters_AccountPostEnclosures_Ac~",
                        column: x => x.AccountPostEnclosureID,
                        principalTable: "AccountPostEnclosures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItemsParameters_AccountPostEnclosureID",
                table: "AccountItemsParameters",
                column: "AccountPostEnclosureID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostEnclosures_AccountPostID",
                table: "AccountPostEnclosures",
                column: "AccountPostID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostEnclosuresItemParameters_AccountPostEnclosureID",
                table: "AccountPostEnclosuresItemParameters",
                column: "AccountPostEnclosureID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountItemsParameters_AccountPostEnclosures_AccountPostEncl~",
                table: "AccountItemsParameters",
                column: "AccountPostEnclosureID",
                principalTable: "AccountPostEnclosures",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountItemsParameters_AccountPostEnclosures_AccountPostEncl~",
                table: "AccountItemsParameters");

            migrationBuilder.DropTable(
                name: "AccountPostEnclosuresItemParameters");

            migrationBuilder.DropTable(
                name: "AccountPostEnclosures");

            migrationBuilder.DropIndex(
                name: "IX_AccountItemsParameters_AccountPostEnclosureID",
                table: "AccountItemsParameters");

            migrationBuilder.DropColumn(
                name: "AccountPostEnclosureID",
                table: "AccountItemsParameters");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountPostID",
                table: "AccountItems",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItems_AccountPostID",
                table: "AccountItems",
                column: "AccountPostID");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountItems_AccountPosts_AccountPostID",
                table: "AccountItems",
                column: "AccountPostID",
                principalTable: "AccountPosts",
                principalColumn: "ID");
        }
    }
}
