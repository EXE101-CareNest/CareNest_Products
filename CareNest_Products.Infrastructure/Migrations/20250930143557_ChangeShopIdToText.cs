using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNest_Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShopIdToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
         name: "IX_Products_ShopId",
         table: "Products");

            migrationBuilder.Sql(@"ALTER TABLE ""Products""
        ALTER COLUMN ""ShopId"" TYPE text USING ""ShopId""::text;");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ShopId",
                table: "Products");

            migrationBuilder.Sql(@"ALTER TABLE ""Products""
        ALTER COLUMN ""ShopId"" TYPE uuid USING ""ShopId""::uuid;");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                column: "ShopId");
        }
    }
}
