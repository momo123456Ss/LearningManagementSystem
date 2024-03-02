using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    public partial class UpdateTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenThereAreChangesInSubjectContent",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenThereAreChangesInSubjectManagement",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenYouConfirmOrCancelTheTest",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenYouMakeChangesInTheRoleList",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LeadershipNotificationWhenYouMakeChangesInTheUserList",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationWhenChangingPassword",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationWhenUpdatingAccount",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StudentNotificationsWhenInstructorsCreateSubjectAnnouncements",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StudentNotificationsWhenSomeoneCommentsOnASubjectAnnouncement",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StudentNotificationsWhenSomeoneInteractsWithYourQuestionOrAnswer",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StudentNotificationsWhenTheLecturerAsksAQuestionInTheSubject",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenSomeoneAsksAQuestionInTheCourseOrInteractsWithYourAnswer",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenSomeoneCommentsOnTheCourseAnnouncement",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenYouAddDocumentsOrUpdateDocumentsAndAssignDocumentsToTeachingClasses",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenYouCreateOrChangeTheNameOrDeleteALectureAndMoveTheLectureToTheSubjectTopic",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenYouUpdateTheTestBankWhenUploadOrCreateNewOrEditAndDelete",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotificationWhenYouUploadOrCreateNewTestQuestionsAndRenameTestQuestions",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeacherNotifyWhenYouCreateOrChangeTheNameOrDeleteAResourcesAndMoveTheResourcesToTheSubjectTopic",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenThereAreChangesInSubjectContent",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenThereAreChangesInSubjectManagement",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenYouConfirmOrCancelTheTest",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenYouMakeChangesInTheRoleList",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LeadershipNotificationWhenYouMakeChangesInTheUserList",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NotificationWhenChangingPassword",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NotificationWhenUpdatingAccount",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudentNotificationsWhenInstructorsCreateSubjectAnnouncements",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudentNotificationsWhenSomeoneCommentsOnASubjectAnnouncement",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudentNotificationsWhenSomeoneInteractsWithYourQuestionOrAnswer",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudentNotificationsWhenTheLecturerAsksAQuestionInTheSubject",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenSomeoneAsksAQuestionInTheCourseOrInteractsWithYourAnswer",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenSomeoneCommentsOnTheCourseAnnouncement",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenYouAddDocumentsOrUpdateDocumentsAndAssignDocumentsToTeachingClasses",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenYouCreateOrChangeTheNameOrDeleteALectureAndMoveTheLectureToTheSubjectTopic",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenYouUpdateTheTestBankWhenUploadOrCreateNewOrEditAndDelete",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotificationWhenYouUploadOrCreateNewTestQuestionsAndRenameTestQuestions",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeacherNotifyWhenYouCreateOrChangeTheNameOrDeleteAResourcesAndMoveTheResourcesToTheSubjectTopic",
                table: "User");
        }
    }
}
