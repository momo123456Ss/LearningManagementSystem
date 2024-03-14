using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class UpdateTableSA1403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectIdAnnouncement",
                table: "SubjectAnnouncement",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAnnouncement_SubjectIdAnnouncement",
                table: "SubjectAnnouncement",
                column: "SubjectIdAnnouncement");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAnnouncement_Subject_SubjectIdAnnouncement",
                table: "SubjectAnnouncement",
                column: "SubjectIdAnnouncement",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAnnouncement_Subject_SubjectIdAnnouncement",
                table: "SubjectAnnouncement");

            migrationBuilder.DropIndex(
                name: "IX_SubjectAnnouncement_SubjectIdAnnouncement",
                table: "SubjectAnnouncement");

            migrationBuilder.DropColumn(
                name: "SubjectIdAnnouncement",
                table: "SubjectAnnouncement");
        }
    }
}
