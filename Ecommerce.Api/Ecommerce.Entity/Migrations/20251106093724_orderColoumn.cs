using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Entity.Migrations
{
    /// <inheritdoc />
    public partial class orderColoumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "OrdersSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "OrdersSet",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "pincode",
                table: "OrdersSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "OrdersSet");

            migrationBuilder.DropColumn(
                name: "address",
                table: "OrdersSet");

            migrationBuilder.DropColumn(
                name: "pincode",
                table: "OrdersSet");
        }
    }
}
