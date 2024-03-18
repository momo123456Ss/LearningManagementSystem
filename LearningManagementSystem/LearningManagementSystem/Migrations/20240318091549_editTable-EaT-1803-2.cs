using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class editTableEaT18032 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions");

            migrationBuilder.AddColumn<int>(
                name: "EaTQuestionId",
                table: "ExamAndTestQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions",
                columns: new[] { "ExamAndTestQuestionId", "EaTQuestionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions");

            migrationBuilder.DropColumn(
                name: "EaTQuestionId",
                table: "ExamAndTestQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamAndTestQuestions",
                table: "ExamAndTestQuestions",
                column: "ExamAndTestQuestionId");
        }
    }
}
