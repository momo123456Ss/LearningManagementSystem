using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class editTableEaT18034 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamAndTestQuestionCode",
                table: "ExamAndTestQuestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExamAndTestQuestionType",
                table: "ExamAndTestQuestions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamAndTestQuestionCode",
                table: "ExamAndTestQuestions");

            migrationBuilder.DropColumn(
                name: "ExamAndTestQuestionType",
                table: "ExamAndTestQuestions");
        }
    }
}
