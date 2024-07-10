using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class _09072024_1414 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "256323eb-15ff-427f-aab5-3931bf6c8129", "d475045b-c324-43ea-b416-36a2484753d0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "256323eb-15ff-427f-aab5-3931bf6c8129");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d475045b-c324-43ea-b416-36a2484753d0");

            migrationBuilder.AlterColumn<string>(
                name: "SplashImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "SplashImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "256323eb-15ff-427f-aab5-3931bf6c8129", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d475045b-c324-43ea-b416-36a2484753d0", 0, "170288bd-23e5-4697-8fc5-d64ecf8ffa0c", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEGdQfMUZaFBB/7p0dPqmYNw5Ov8PHsJXeD1RdSLcyvPtkkHSImH18Q2ktksgB4QcWQ==", null, false, "ff31e2ab-7a46-4b2b-9067-e22018c9c91e", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "256323eb-15ff-427f-aab5-3931bf6c8129", "d475045b-c324-43ea-b416-36a2484753d0" });
        }
    }
}
