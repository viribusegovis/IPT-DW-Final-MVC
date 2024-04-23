using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quize.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Tags_TagsId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_TagsId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Quizzes");

            migrationBuilder.CreateTable(
                name: "QuizTag",
                columns: table => new
                {
                    QuizListId = table.Column<int>(type: "int", nullable: false),
                    TagListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizTag", x => new { x.QuizListId, x.TagListId });
                    table.ForeignKey(
                        name: "FK_QuizTag_Quizzes_QuizListId",
                        column: x => x.QuizListId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizTag_Tags_TagListId",
                        column: x => x.TagListId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizTag_TagListId",
                table: "QuizTag",
                column: "TagListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizTag");

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
    }
}
