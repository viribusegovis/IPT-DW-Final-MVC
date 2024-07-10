using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class _09072024_1633 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ac6a7baa-32c0-48a3-82ec-9fd9a56733c4", "62354be0-b7ca-4c06-835c-43acae02eac9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac6a7baa-32c0-48a3-82ec-9fd9a56733c4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62354be0-b7ca-4c06-835c-43acae02eac9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d1cd6f11-28f3-4a31-a97f-167565264be0", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec", 0, "e6df5c79-3fec-4850-bdc8-5a90d5a3640f", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEPrZPvXloshT4x7VG57HrDziatguGDqbC70QZ1NVNNqLwAICs3VR6tq3EFzcXlBEVA==", null, false, "6dbd4e9b-304a-43b3-8015-12cb60dd941e", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d1cd6f11-28f3-4a31-a97f-167565264be0", "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec" });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d1cd6f11-28f3-4a31-a97f-167565264be0", "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1cd6f11-28f3-4a31-a97f-167565264be0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ac6a7baa-32c0-48a3-82ec-9fd9a56733c4", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "62354be0-b7ca-4c06-835c-43acae02eac9", 0, "06458638-4f91-4fcb-b334-4d04f4cf3cc4", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAELdghJVQe9HWAz+BVpMivKzgKxBn4fe4+2pbAQPjir8r96gLNpot3mrGAFwjsSDVZg==", null, false, "be43c18f-f7c1-4928-9ee3-43c98aa3765d", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ac6a7baa-32c0-48a3-82ec-9fd9a56733c4", "62354be0-b7ca-4c06-835c-43acae02eac9" });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AuthorId",
                table: "Quizzes",
                column: "AuthorId",
                unique: true);
        }
    }
}
