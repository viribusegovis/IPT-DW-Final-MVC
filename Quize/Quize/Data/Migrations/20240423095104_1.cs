using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Data.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagFK",
                table: "Quizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagFK",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
