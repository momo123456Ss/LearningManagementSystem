using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class CreateTableLearningStatistics2203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeBegin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    totalMinutes = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningStatistics_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningStatistics_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningStatistics_User_StudentId",
                        column: x => x.StudentId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningStatistics_ClassId",
                table: "LearningStatistics",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningStatistics_StudentId",
                table: "LearningStatistics",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningStatistics_SubjectId",
                table: "LearningStatistics",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningStatistics");
        }
    }
}
