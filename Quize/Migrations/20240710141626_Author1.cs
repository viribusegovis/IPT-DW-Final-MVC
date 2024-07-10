using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class Author1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9c866f99-955e-42e7-8fd8-63c26ed89c26", "7b085d9d-8855-41ad-b3bc-3b6478430622" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c866f99-955e-42e7-8fd8-63c26ed89c26");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b085d9d-8855-41ad-b3bc-3b6478430622");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bd2ac25d-a42e-4c0a-b431-1817833e5ae9", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cf7ff158-8e0d-4c80-82fe-e51ad57a41c2", 0, "fe3c4c3f-91b8-4a08-a254-6ba454fe03f4", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEJRQkzQf0tLqwkry4dpzRExN8Q9eFxKtV9aP+wCBxGQDH5iFOLAsjkSj7ZlJy/8gAg==", null, false, "1b4a0b3d-4f77-4971-9dda-43dd23925c0b", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "bd2ac25d-a42e-4c0a-b431-1817833e5ae9", "cf7ff158-8e0d-4c80-82fe-e51ad57a41c2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bd2ac25d-a42e-4c0a-b431-1817833e5ae9", "cf7ff158-8e0d-4c80-82fe-e51ad57a41c2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd2ac25d-a42e-4c0a-b431-1817833e5ae9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf7ff158-8e0d-4c80-82fe-e51ad57a41c2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c866f99-955e-42e7-8fd8-63c26ed89c26", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7b085d9d-8855-41ad-b3bc-3b6478430622", 0, "de7929a8-5968-43c8-89ef-aac4e97e5aa6", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEJMSZlo4qh231sHBLDmkgJ7vaz3gWsRLFnr+z2i3y6u3z8so41GclMXJ5+sWe09nOA==", null, false, "440e82e9-8037-49c9-a7c6-eb3ba0a3310d", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9c866f99-955e-42e7-8fd8-63c26ed89c26", "7b085d9d-8855-41ad-b3bc-3b6478430622" });
        }
    }
}
