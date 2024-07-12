using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e65a4531-809f-4b7c-96a0-2ae5dffd7af4", "519891ed-2ee0-4444-9f88-f93b4d92c6f6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e65a4531-809f-4b7c-96a0-2ae5dffd7af4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "519891ed-2ee0-4444-9f88-f93b4d92c6f6");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.AddColumn<bool>(
                name: "CorrectAnswer",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswer",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e65a4531-809f-4b7c-96a0-2ae5dffd7af4", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "519891ed-2ee0-4444-9f88-f93b4d92c6f6", 0, "45e9b0a0-4489-4738-8544-9bfe73fe0305", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAECZcVIYqqjmgEaur+SUKCzZIiRllNTtnXSAyA8clH6/6mQ1AhMW9TC6Jvzo3qJ2YWQ==", null, false, "3e8afbda-9464-4620-a2c4-9f850a2b5e28", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e65a4531-809f-4b7c-96a0-2ae5dffd7af4", "519891ed-2ee0-4444-9f88-f93b4d92c6f6" });
        }
    }
}
