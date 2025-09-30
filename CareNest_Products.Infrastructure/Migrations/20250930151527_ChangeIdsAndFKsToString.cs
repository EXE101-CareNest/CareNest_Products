using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNest_Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdsAndFKsToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Drop FKs + Indexes cũ
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                table: "ProductCategories");
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_ProductCategories_CategoryId",
                table: "ProductDetails");

            migrationBuilder.DropIndex(name: "IX_ProductCategories_ProductId", table: "ProductCategories");
            migrationBuilder.DropIndex(name: "IX_ProductDetails_CategoryId", table: "ProductDetails");
            migrationBuilder.DropIndex(name: "IX_Products_ShopId", table: "Products");

            // 2) Đổi kiểu cột bằng USING ::text (an toàn dữ liệu)
            migrationBuilder.Sql(@"ALTER TABLE ""Products"" ALTER COLUMN ""Id"" TYPE text USING ""Id""::text;");
            migrationBuilder.Sql(@"ALTER TABLE ""Products"" ALTER COLUMN ""ShopId"" TYPE text USING ""ShopId""::text;"); // nếu ShopId đang uuid

            migrationBuilder.Sql(@"ALTER TABLE ""ProductCategories"" ALTER COLUMN ""Id"" TYPE text USING ""Id""::text;");
            migrationBuilder.Sql(@"ALTER TABLE ""ProductCategories"" ALTER COLUMN ""ProductId"" TYPE text USING ""ProductId""::text;");

            migrationBuilder.Sql(@"ALTER TABLE ""ProductDetails"" ALTER COLUMN ""Id"" TYPE text USING ""Id""::text;");
            migrationBuilder.Sql(@"ALTER TABLE ""ProductDetails"" ALTER COLUMN ""CategoryId"" TYPE text USING ""CategoryId""::text;");

            // 3) Tạo lại Indexes
            migrationBuilder.CreateIndex(name: "IX_Products_ShopId", table: "Products", column: "ShopId");
            migrationBuilder.CreateIndex(name: "IX_ProductCategories_ProductId", table: "ProductCategories", column: "ProductId");
            migrationBuilder.CreateIndex(name: "IX_ProductDetails_CategoryId", table: "ProductDetails", column: "CategoryId");

            // 4) Tạo lại FKs
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_ProductCategories_Products_ProductId", "ProductCategories");
            migrationBuilder.DropForeignKey("FK_ProductDetails_ProductCategories_CategoryId", "ProductDetails");

            migrationBuilder.DropIndex("IX_ProductCategories_ProductId", "ProductCategories");
            migrationBuilder.DropIndex("IX_ProductDetails_CategoryId", "ProductDetails");
            migrationBuilder.DropIndex("IX_Products_ShopId", "Products");

            migrationBuilder.Sql(@"ALTER TABLE ""ProductDetails"" ALTER COLUMN ""CategoryId"" TYPE uuid USING ""CategoryId""::uuid;");
            migrationBuilder.Sql(@"ALTER TABLE ""ProductDetails"" ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid;");

            migrationBuilder.Sql(@"ALTER TABLE ""ProductCategories"" ALTER COLUMN ""ProductId"" TYPE uuid USING ""ProductId""::uuid;");
            migrationBuilder.Sql(@"ALTER TABLE ""ProductCategories"" ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid;");

            migrationBuilder.Sql(@"ALTER TABLE ""Products"" ALTER COLUMN ""ShopId"" TYPE uuid USING ""ShopId""::uuid;");
            migrationBuilder.Sql(@"ALTER TABLE ""Products"" ALTER COLUMN ""Id"" TYPE uuid USING ""Id""::uuid;");

            migrationBuilder.CreateIndex(name: "IX_ProductCategories_ProductId", table: "ProductCategories", column: "ProductId");
            migrationBuilder.CreateIndex(name: "IX_ProductDetails_CategoryId", table: "ProductDetails", column: "CategoryId");
            migrationBuilder.CreateIndex(name: "IX_Products_ShopId", table: "Products", column: "ShopId");

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
