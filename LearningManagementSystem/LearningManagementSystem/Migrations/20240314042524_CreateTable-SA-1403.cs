using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class CreateTableSA1403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectAnnouncement",
                columns: table => new
                {
                    SubjectAnnouncementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectAnnouncementTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectAnnouncementContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SACreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SAInOtherSA = table.Column<int>(type: "int", nullable: true),
                    SAReplySA = table.Column<int>(type: "int", nullable: true),
                    UserIdAnnouncement = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassIdAnnouncement = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAnnouncement", x => x.SubjectAnnouncementId);
                    table.ForeignKey(
                        name: "FK_SubjectAnnouncement_Class_ClassIdAnnouncement",
                        column: x => x.ClassIdAnnouncement,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectAnnouncement_SubjectAnnouncement_SAInOtherSA",
                        column: x => x.SAInOtherSA,
                        principalTable: "SubjectAnnouncement",
                        principalColumn: "SubjectAnnouncementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAnnouncement_SubjectAnnouncement_SAReplySA",
                        column: x => x.SAReplySA,
                        principalTable: "SubjectAnnouncement",
                        principalColumn: "SubjectAnnouncementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectAnnouncement_User_UserIdAnnouncement",
                        column: x => x.UserIdAnnouncement,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAnnouncement_ClassIdAnnouncement",
                table: "SubjectAnnouncement",
                column: "ClassIdAnnouncement");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAnnouncement_SAInOtherSA",
                table: "SubjectAnnouncement",
                column: "SAInOtherSA");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAnnouncement_SAReplySA",
                table: "SubjectAnnouncement",
                column: "SAReplySA");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAnnouncement_UserIdAnnouncement",
                table: "SubjectAnnouncement",
                column: "UserIdAnnouncement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectAnnouncement");
        }
    }
}
