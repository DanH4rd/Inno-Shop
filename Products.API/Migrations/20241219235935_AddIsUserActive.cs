using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Products.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsUserActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Product_UserActive",
                table: "Products",
                column: "IsUserActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_UserActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsUserActive",
                table: "Products");
        }
    }
}
