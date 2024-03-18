using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class CreateTableExamQuestion_Answer1803 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamAndTestQuestions",
                columns: table => new
                {
                    ExamAndTestQuestionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExamAndTestQuestionContent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    KindOfQuestion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAndTestQuestions", x => x.ExamAndTestQuestionId);
                    table.ForeignKey(
                        name: "FK_ExamAndTestQuestions_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamAndTestQuestions_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamAndTestAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerContent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    isAnswer = table.Column<bool>(type: "bit", nullable: false),
                    ExamAndTestQuestionId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAndTestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamAndTestAnswers_ExamAndTestQuestions_ExamAndTestQuestionId",
                        column: x => x.ExamAndTestQuestionId,
                        principalTable: "ExamAndTestQuestions",
                        principalColumn: "ExamAndTestQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamAndTestAnswers_ExamAndTestQuestionId",
                table: "ExamAndTestAnswers",
                column: "ExamAndTestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAndTestQuestions_FacultyId",
                table: "ExamAndTestQuestions",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAndTestQuestions_SubjectId",
                table: "ExamAndTestQuestions",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamAndTestAnswers");

            migrationBuilder.DropTable(
                name: "ExamAndTestQuestions");
        }
    }
}
