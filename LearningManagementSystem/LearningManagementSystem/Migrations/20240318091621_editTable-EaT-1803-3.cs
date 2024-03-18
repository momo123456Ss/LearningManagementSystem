using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class editTableEaT18033 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions");

            migrationBuilder.DropColumn(
                name: "ExamAndTestQuestionId",
                table: "ExamAndTestQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions",
                column: "EaTQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions");

            migrationBuilder.AddColumn<string>(
                name: "ExamAndTestQuestionId",
                table: "ExamAndTestQuestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions",
                columns: new[] { "ExamAndTestQuestionId", "EaTQuestionId" });
        }
    }
}
