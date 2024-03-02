using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectSee = table.Column<bool>(type: "bit", nullable: false),
                    SubjectEdit = table.Column<bool>(type: "bit", nullable: false),
                    PrivateFilesSee = table.Column<bool>(type: "bit", nullable: false),
                    PrivateFilesEdit = table.Column<bool>(type: "bit", nullable: false),
                    PrivateFilesDelete = table.Column<bool>(type: "bit", nullable: false),
                    PrivateFilesCreateNew = table.Column<bool>(type: "bit", nullable: false),
                    PrivateFilesDownload = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesSee = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesEdit = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesDelete = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesCreateNew = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesDownload = table.Column<bool>(type: "bit", nullable: false),
                    LecturesAndResourcesAddToSubject = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsSee = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsEdit = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsCreateNew = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsDownload = table.Column<bool>(type: "bit", nullable: false),
                    ExamsAndTestsAcceptance = table.Column<bool>(type: "bit", nullable: false),
                    NotificationSee = table.Column<bool>(type: "bit", nullable: false),
                    NotificationEdit = table.Column<bool>(type: "bit", nullable: false),
                    NotificationDelete = table.Column<bool>(type: "bit", nullable: false),
                    NotificationSettings = table.Column<bool>(type: "bit", nullable: false),
                    DecentralizationSee = table.Column<bool>(type: "bit", nullable: false),
                    DecentralizationEdit = table.Column<bool>(type: "bit", nullable: false),
                    DecentralizationDelete = table.Column<bool>(type: "bit", nullable: false),
                    DecentralizationCreateNew = table.Column<bool>(type: "bit", nullable: false),
                    UserAccountSee = table.Column<bool>(type: "bit", nullable: false),
                    UserAccountEdit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleName",
                table: "UserRole",
                column: "RoleName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
