using InnoShop.Users.API.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Users.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Name", "Email", "EmailConfirmationToken", "IsEmailConfirmed", "PasswordHash", "Role", "CreatedDate", "LastLogin", "IsActive" },
                values: new object[]
                {
                    "Admin",
                    "admin@admin.com",
                    Guid.NewGuid().ToString(), // EmailConfirmationToken
                    true, // IsEmailConfirmed
                    UserLoginInfo.CalcMD5Hash("admin"), // PasswordHash
                    1, // admin role
                    DateTime.UtcNow, // CreatedDate
                    null, // LastLogin
                    true // IsActive
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                                table: "Users",
                                keyColumn: "Email",
                                keyValue: "admin@admin.com");
        }
    }
}
