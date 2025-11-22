using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Entity.Migrations
{
    /// <inheritdoc />
    public partial class productimages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_ProductsSet_ProductId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "productImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_ProductId",
                table: "productImages",
                newName: "IX_productImages_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productImages",
                table: "productImages",
                column: "ProductImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_productImages_ProductsSet_ProductId",
                table: "productImages",
                column: "ProductId",
                principalTable: "ProductsSet",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productImages_ProductsSet_ProductId",
                table: "productImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productImages",
                table: "productImages");

            migrationBuilder.RenameTable(
                name: "productImages",
                newName: "ProductImage");

            migrationBuilder.RenameIndex(
                name: "IX_productImages_ProductId",
                table: "ProductImage",
                newName: "IX_ProductImage_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                column: "ProductImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_ProductsSet_ProductId",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "ProductsSet",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
