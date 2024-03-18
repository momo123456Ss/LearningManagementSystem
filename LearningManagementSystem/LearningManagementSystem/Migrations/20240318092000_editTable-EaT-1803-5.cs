using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class editTableEaT18035 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EaTQuestionId",
                table: "ExamAndTestAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExamAndTestAnswers_EaTQuestionId",
                table: "ExamAndTestAnswers",
                column: "EaTQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAndTestAnswers_ExamAndTestQuestions_EaTQuestionId",
                table: "ExamAndTestAnswers",
                column: "EaTQuestionId",
                principalTable: "ExamAndTestQuestions",
                principalColumn: "EaTQuestionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAndTestAnswers_ExamAndTestQuestions_EaTQuestionId",
                table: "ExamAndTestAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ExamAndTestAnswers_EaTQuestionId",
                table: "ExamAndTestAnswers");

            migrationBuilder.DropColumn(
                name: "EaTQuestionId",
                table: "ExamAndTestAnswers");
        }
    }
}
