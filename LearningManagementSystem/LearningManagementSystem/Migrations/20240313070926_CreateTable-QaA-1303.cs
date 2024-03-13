using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class CreateTableQaA1303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionAndAnswer",
                columns: table => new
                {
                    QuestionAndAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAndAnswerTitle = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    QuestionAndAnswerContent = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    countLike = table.Column<int>(type: "int", nullable: false),
                    QaAInOtherQaA = table.Column<int>(type: "int", nullable: false),
                    QaAReplyQaA = table.Column<int>(type: "int", nullable: false),
                    UserIdComment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassIdComment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonIdComment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAndAnswer", x => x.QuestionAndAnswerId);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_Class_ClassIdComment",
                        column: x => x.ClassIdComment,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_Lesson_LessonIdComment",
                        column: x => x.LessonIdComment,
                        principalTable: "Lesson",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_QuestionAndAnswer_QaAInOtherQaA",
                        column: x => x.QaAInOtherQaA,
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "QuestionAndAnswerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_QuestionAndAnswer_QaAReplyQaA",
                        column: x => x.QaAReplyQaA,
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "QuestionAndAnswerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswer_User_UserIdComment",
                        column: x => x.UserIdComment,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_ClassIdComment",
                table: "QuestionAndAnswer",
                column: "ClassIdComment");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_LessonIdComment",
                table: "QuestionAndAnswer",
                column: "LessonIdComment");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_QaAInOtherQaA",
                table: "QuestionAndAnswer",
                column: "QaAInOtherQaA");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_QaAReplyQaA",
                table: "QuestionAndAnswer",
                column: "QaAReplyQaA");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswer_UserIdComment",
                table: "QuestionAndAnswer",
                column: "UserIdComment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAndAnswer");
        }
    }
}
