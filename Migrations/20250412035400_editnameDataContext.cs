using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equipment_store.Migrations
{
    /// <inheritdoc />
    public partial class editnameDataContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Categorys_CategoryId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorys",
                table: "Categorys");

            migrationBuilder.RenameTable(
                name: "Categorys",
                newName: "Categoríes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoríes",
                table: "Categoríes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producs_Categoríes_CategoryId",
                table: "Producs",
                column: "CategoryId",
                principalTable: "Categoríes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Categoríes_CategoryId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoríes",
                table: "Categoríes");

            migrationBuilder.RenameTable(
                name: "Categoríes",
                newName: "Categorys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorys",
                table: "Categorys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producs_Categorys_CategoryId",
                table: "Producs",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
