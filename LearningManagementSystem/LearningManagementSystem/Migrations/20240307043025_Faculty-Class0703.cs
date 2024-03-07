using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class FacultyClass0703 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Faculty",
                table: "Class",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Class_Faculty",
                table: "Class",
                column: "Faculty");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Faculty_Faculty",
                table: "Class",
                column: "Faculty",
                principalTable: "Faculty",
                principalColumn: "FacultyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Faculty_Faculty",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_Faculty",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Class");
        }
    }
}
