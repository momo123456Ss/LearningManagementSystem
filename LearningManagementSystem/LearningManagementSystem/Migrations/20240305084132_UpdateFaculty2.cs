using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class UpdateFaculty2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_User_UserId",
                table: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_Faculty_UserId",
                table: "Faculty");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Faculty");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculty_User_HeadOfDepartment",
                table: "Faculty");

            migrationBuilder.DropIndex(
                name: "IX_Faculty_HeadOfDepartment",
                table: "Faculty");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Faculty",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_UserId",
                table: "Faculty",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculty_User_UserId",
                table: "Faculty",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
