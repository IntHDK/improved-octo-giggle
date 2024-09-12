using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebReactApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class v00010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ParamName",
                table: "AccountPostEnclosuresItemParameters",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ItemMetaName",
                table: "AccountPostEnclosures",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ParamName",
                table: "AccountItemsParameters",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ItemMetaName",
                table: "AccountItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostEnclosuresItemParameters_ParamName",
                table: "AccountPostEnclosuresItemParameters",
                column: "ParamName");

            migrationBuilder.CreateIndex(
                name: "IX_AccountPostEnclosures_ItemMetaName",
                table: "AccountPostEnclosures",
                column: "ItemMetaName");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItemsParameters_ParamName",
                table: "AccountItemsParameters",
                column: "ParamName");

            migrationBuilder.CreateIndex(
                name: "IX_AccountItems_ItemMetaName",
                table: "AccountItems",
                column: "ItemMetaName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountPostEnclosuresItemParameters_ParamName",
                table: "AccountPostEnclosuresItemParameters");

            migrationBuilder.DropIndex(
                name: "IX_AccountPostEnclosures_ItemMetaName",
                table: "AccountPostEnclosures");

            migrationBuilder.DropIndex(
                name: "IX_AccountItemsParameters_ParamName",
                table: "AccountItemsParameters");

            migrationBuilder.DropIndex(
                name: "IX_AccountItems_ItemMetaName",
                table: "AccountItems");

            migrationBuilder.AlterColumn<string>(
                name: "ParamName",
                table: "AccountPostEnclosuresItemParameters",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ItemMetaName",
                table: "AccountPostEnclosures",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ParamName",
                table: "AccountItemsParameters",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ItemMetaName",
                table: "AccountItems",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
