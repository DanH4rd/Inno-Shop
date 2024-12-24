using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Products.API.Migrations
{
    /// <inheritdoc />
    public partial class FixModifiedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedFDate",
                table: "Products",
                newName: "ModifiedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "Products",
                newName: "ModifiedFDate");
        }
    }
}
