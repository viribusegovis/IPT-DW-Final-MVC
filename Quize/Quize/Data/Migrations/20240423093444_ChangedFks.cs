using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tags_TagsId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TagsId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TagFK",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "TagFK",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_TagsId",
                table: "Quizzes",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Tags_TagsId",
                table: "Quizzes",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Tags_TagsId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_TagsId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TagFK",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "TagFK",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TagsId",
                table: "Questions",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tags_TagsId",
                table: "Questions",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
