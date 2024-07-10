using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Migrations
{
    /// <inheritdoc />
    public partial class Author : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "d1cd6f11-28f3-4a31-a97f-167565264be0", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec", 0, "e6df5c79-3fec-4850-bdc8-5a90d5a3640f", "quize.general@gmail.com", true, false, null, "QUIZE.GENERAL@GMAIL.COM", "QUIZE.GENERAL@GMAIL.COM", "AQAAAAIAAYagAAAAEPrZPvXloshT4x7VG57HrDziatguGDqbC70QZ1NVNNqLwAICs3VR6tq3EFzcXlBEVA==", null, false, "6dbd4e9b-304a-43b3-8015-12cb60dd941e", false, "quize.general@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d1cd6f11-28f3-4a31-a97f-167565264be0", "c34bf08f-9aa4-4215-a15b-bb268eb6d2ec" });
        }
    }
}
