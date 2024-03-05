using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class UserBelongToFaculty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_User_HeadOfDepartment",
                table: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_Faculty_HeadOfDepartment",
                table: "Faculty");

            migrationBuilder.DropColumn(
                name: "HeadOfDepartment",
                table: "Faculty");

            migrationBuilder.CreateTable(
                name: "UserBelongToFaculty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBelongToFaculty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBelongToFaculty_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBelongToFaculty_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBelongToFaculty_FacultyId",
                table: "UserBelongToFaculty",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBelongToFaculty_UserId",
                table: "UserBelongToFaculty",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBelongToFaculty");

            migrationBuilder.AddColumn<Guid>(
                name: "HeadOfDepartment",
                table: "Faculty",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_HeadOfDepartment",
                table: "Faculty",
                column: "HeadOfDepartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculty_User_HeadOfDepartment",
                table: "Faculty",
                column: "HeadOfDepartment",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
