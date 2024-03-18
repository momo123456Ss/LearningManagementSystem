using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class editTableEaT1803 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAndTestAnswers_ExamAndTestQuestions_ExamAndTestQuestionId",
                table: "ExamAndTestAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ExamAndTestAnswers_ExamAndTestQuestionId",
                table: "ExamAndTestAnswers");

            migrationBuilder.DropColumn(
                name: "ExamAndTestQuestionId",
                table: "ExamAndTestAnswers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamAndTestQuestionId",
                table: "ExamAndTestAnswers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAndTestAnswers_ExamAndTestQuestionId",
                table: "ExamAndTestAnswers",
                column: "ExamAndTestQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAndTestAnswers_ExamAndTestQuestions_ExamAndTestQuestionId",
                table: "ExamAndTestAnswers",
                column: "ExamAndTestQuestionId",
                principalTable: "ExamAndTestQuestions",
                principalColumn: "ExamAndTestQuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
