using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class UpdateTableLessonResource0903 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonResources_Class_ClassId",
                table: "LessonResources");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonResources_LecturesAndResources_LecturesAndResourcesId",
                table: "LessonResources");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "LessonResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LecturesAndResourcesId",
                table: "LessonResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "LessonResources",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonResources_Class_ClassId",
                table: "LessonResources",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonResources_LecturesAndResources_LecturesAndResourcesId",
                table: "LessonResources",
                column: "LecturesAndResourcesId",
                principalTable: "LecturesAndResources",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonResources_Class_ClassId",
                table: "LessonResources");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonResources_LecturesAndResources_LecturesAndResourcesId",
                table: "LessonResources");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "LessonResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LecturesAndResourcesId",
                table: "LessonResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "LessonResources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonResources_Class_ClassId",
                table: "LessonResources",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonResources_LecturesAndResources_LecturesAndResourcesId",
                table: "LessonResources",
                column: "LecturesAndResourcesId",
                principalTable: "LecturesAndResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
