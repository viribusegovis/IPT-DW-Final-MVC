using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bbc7ad41-254d-4577-9a9f-63a6071c49e7", "7ebb1558-d062-49b6-b4bb-d0a816b36d09" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbc7ad41-254d-4577-9a9f-63a6071c49e7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7ebb1558-d062-49b6-b4bb-d0a816b36d09");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0e11453-a754-4ca0-adfc-9d89f4586d09", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "692d4e36-7529-4342-8fcb-374b68aab8dc", 0, "aa7970b5-0db2-4fef-b19c-c75aca8c3fa0", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEKvSyoHvyrgcj80C73Ef6pnFijWqlLjgZcOV7G9kE3O7amfiXEwkt2QHBHBWwulF7Q==", null, false, "ce97d136-0e55-486a-8be4-11bfc3a683d0", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a0e11453-a754-4ca0-adfc-9d89f4586d09", "692d4e36-7529-4342-8fcb-374b68aab8dc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a0e11453-a754-4ca0-adfc-9d89f4586d09", "692d4e36-7529-4342-8fcb-374b68aab8dc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0e11453-a754-4ca0-adfc-9d89f4586d09");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "692d4e36-7529-4342-8fcb-374b68aab8dc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bbc7ad41-254d-4577-9a9f-63a6071c49e7", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7ebb1558-d062-49b6-b4bb-d0a816b36d09", 0, "f70a5c27-0e1b-416d-9908-aa9651a696cc", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEJk8XS+RQG1p+Lmic6YefVRxSEWm8GD13U/REtvvlMozHmEvDPXjIfry2R0MR0F0WA==", null, false, "2d113a7f-9455-4b22-bb68-de43949699ab", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "bbc7ad41-254d-4577-9a9f-63a6071c49e7", "7ebb1558-d062-49b6-b4bb-d0a816b36d09" });
        }
    }
}
