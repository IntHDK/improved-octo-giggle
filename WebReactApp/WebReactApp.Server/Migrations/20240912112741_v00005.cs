using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class v00005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency_Cash",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Currency_Point",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountPosts",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccountID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Context = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpireAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsEnclosureTaken = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPosts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountPosts_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountItems",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccountID = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ItemMetaName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpireAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AccountPostID = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountItems_AccountPosts_AccountPostID",
                        column: x => x.AccountPostID,
                        principalTable: "AccountPosts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AccountItems_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountItemsParameters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountItemID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParamName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    NumberValue = table.Column<int>(type: "int", nullable: false),
                    StringValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountItemsParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountItemsParameters_AccountItems_AccountItemID",
                        column: x => x.AccountItemID,
                        principalTable: "AccountItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItems_AccountID",
                table: "AccountItems",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItems_AccountPostID",
                table: "AccountItems",
                column: "AccountPostID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItemsParameters_AccountItemID",
                table: "AccountItemsParameters",
                column: "AccountItemID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPosts_AccountID",
                table: "AccountPosts",
                column: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountItemsParameters");

            migrationBuilder.DropTable(
                name: "AccountItems");

            migrationBuilder.DropTable(
                name: "AccountPosts");

            migrationBuilder.DropColumn(
                name: "Currency_Cash",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Currency_Point",
                table: "Accounts");
        }
    }
}
