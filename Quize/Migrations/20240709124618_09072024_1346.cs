using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class _09072024_1346 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d204ec55-6615-4262-8fbe-73ba85fe4fbb", "3fde6260-a129-4af8-be63-95026b7a21aa" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d204ec55-6615-4262-8fbe-73ba85fe4fbb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3fde6260-a129-4af8-be63-95026b7a21aa");

            migrationBuilder.AlterColumn<string>(
                name: "SplashImage",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28809248-269f-4301-891c-1958e73a7377", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f07c7f3a-9bd6-48f1-8026-43e53ba7e6c8", 0, "fe800019-e66e-4846-b178-7bc4547e1f50", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEADL0I4UAWDCs1uAsplDBuNy5TXwCBCl3up8M/ru3sYxLelyTTmydUFnLVTMP5W55w==", null, false, "a7ad9b0f-90a9-45ef-b80f-45a8999ecedb", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "28809248-269f-4301-891c-1958e73a7377", "f07c7f3a-9bd6-48f1-8026-43e53ba7e6c8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "28809248-269f-4301-891c-1958e73a7377", "f07c7f3a-9bd6-48f1-8026-43e53ba7e6c8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28809248-269f-4301-891c-1958e73a7377");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f07c7f3a-9bd6-48f1-8026-43e53ba7e6c8");

            migrationBuilder.AlterColumn<string>(
                name: "SplashImage",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d204ec55-6615-4262-8fbe-73ba85fe4fbb", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3fde6260-a129-4af8-be63-95026b7a21aa", 0, "24f4ad97-95cb-42d3-a0bd-0297ee4ca4a9", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEF5UaKwuCQvVpy8rpEiHc84LyVX5uUzxLjzM2hdWgP3cFQ2a3/3gH3Iiqhujxn/52Q==", null, false, "3f1bc8c6-fe0c-4621-989e-09c403bd1fa0", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d204ec55-6615-4262-8fbe-73ba85fe4fbb", "3fde6260-a129-4af8-be63-95026b7a21aa" });
        }
    }
}
