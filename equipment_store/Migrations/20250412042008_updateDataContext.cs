using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace equipment_store.Migrations
{
    /// <inheritdoc />
    public partial class updateDataContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Brans_BrandId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brans",
                table: "Brans");

            migrationBuilder.RenameTable(
                name: "Brans",
                newName: "Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producs_Brands_BrandId",
                table: "Producs",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producs_Brands_BrandId",
                table: "Producs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brans",
                table: "Brans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Producs_Brans_BrandId",
                table: "Producs",
                column: "BrandId",
                principalTable: "Brans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
