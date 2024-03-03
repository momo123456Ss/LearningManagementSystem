using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class DBEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRole_RoleName",
                table: "UserRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleName",
                table: "UserRole",
                column: "RoleName",
                unique: true);
        }
    }
}
