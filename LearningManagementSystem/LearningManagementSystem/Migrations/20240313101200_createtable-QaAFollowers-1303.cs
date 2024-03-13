using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class createtableQaAFollowers1303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QaAFollowers",
                columns: table => new
                {
                    QaAFollowersId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFollower = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QaAIdFollow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaAFollowers", x => x.QaAFollowersId);
                    table.ForeignKey(
                        name: "FK_QaAFollowers_QuestionAndAnswer_QaAIdFollow",
                        column: x => x.QaAIdFollow,
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "QuestionAndAnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QaAFollowers_User_UserIdFollower",
                        column: x => x.UserIdFollower,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QaAFollowers_QaAIdFollow",
                table: "QaAFollowers",
                column: "QaAIdFollow");

            migrationBuilder.CreateIndex(
                name: "IX_QaAFollowers_UserIdFollower",
                table: "QaAFollowers",
                column: "UserIdFollower");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QaAFollowers");
        }
    }
}
