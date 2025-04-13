using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equipment_store.Migrations
{
    /// <inheritdoc />
    public partial class editdatacontext2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Categoríes_CategoryId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoríes",
                table: "Categoríes");

            migrationBuilder.RenameTable(
                name: "Categoríes",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producs_Categories_CategoryId",
                table: "Producs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Categories_CategoryId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
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
    }
}
