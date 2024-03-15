using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class createtableusernotification1503 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    UserNotificationsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserNotificationsContent = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserIdNotifications = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QaAFollowersId = table.Column<int>(type: "int", nullable: true),
                    SubjectAnnouncementId = table.Column<int>(type: "int", nullable: true),
                    QuestionAndAnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.UserNotificationsId);
                    table.ForeignKey(
                        name: "FK_UserNotifications_QaAFollowers_QaAFollowersId",
                        column: x => x.QaAFollowersId,
                        principalTable: "QaAFollowers",
                        principalColumn: "QaAFollowersId");
                    table.ForeignKey(
                        name: "FK_UserNotifications_QuestionAndAnswer_QuestionAndAnswerId",
                        column: x => x.QuestionAndAnswerId,
                        principalTable: "QuestionAndAnswer",
                        principalColumn: "QuestionAndAnswerId");
                    table.ForeignKey(
                        name: "FK_UserNotifications_SubjectAnnouncement_SubjectAnnouncementId",
                        column: x => x.SubjectAnnouncementId,
                        principalTable: "SubjectAnnouncement",
                        principalColumn: "SubjectAnnouncementId");
                    table.ForeignKey(
                        name: "FK_UserNotifications_User_UserIdNotifications",
                        column: x => x.UserIdNotifications,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_QaAFollowersId",
                table: "UserNotifications",
                column: "QaAFollowersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_QuestionAndAnswerId",
                table: "UserNotifications",
                column: "QuestionAndAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_SubjectAnnouncementId",
                table: "UserNotifications",
                column: "SubjectAnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserIdNotifications",
                table: "UserNotifications",
                column: "UserIdNotifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotifications");
        }
    }
}
