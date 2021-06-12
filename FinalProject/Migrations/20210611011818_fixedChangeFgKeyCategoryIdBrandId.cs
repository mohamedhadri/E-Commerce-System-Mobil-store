using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Migrations
{
    public partial class fixedChangeFgKeyCategoryIdBrandId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Brands_BrandsId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoriesId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BrandsId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoriesId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BrandsId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BrandId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "BrandsId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriesId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandsId",
                table: "Items",
                column: "BrandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoriesId",
                table: "Items",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Brands_BrandsId",
                table: "Items",
                column: "BrandsId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoriesId",
                table: "Items",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
