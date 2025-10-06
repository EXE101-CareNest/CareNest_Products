using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNest_Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_ProductCategories_CategoryId",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "Products",
                newName: "ProductCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                newName: "IX_Products_ProductCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_CategoryId",
                table: "ProductDetails",
                newName: "IX_ProductDetails_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductCategories",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ProductId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_Products_ProductId",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryId",
                table: "Products",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                newName: "IX_Products_ShopId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductDetails",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                newName: "IX_ProductDetails_CategoryId");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "ProductCategories",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ShopId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_ProductCategories_CategoryId",
                table: "ProductDetails",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
