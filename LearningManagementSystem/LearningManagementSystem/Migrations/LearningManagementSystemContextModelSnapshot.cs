﻿// <auto-generated />
using System;
using LearningManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    [DbContext(typeof(LearningManagementSystemContext))]
    partial class LearningManagementSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LearningManagementSystem.Entity.Class", b =>
                {
                    b.Property<Guid>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ClassClosingDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("ClassCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ClassOpeningDay")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.CloudinaryConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApiSecret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CloudName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CloudinaryConfiguration");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.Faculty", b =>
                {
                    b.Property<Guid>("FacultyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ContactInformation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("EstablishmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FacultyCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FacultyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumberOfStudents")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfTeacher")
                        .HasColumnType("int");

                    b.HasKey("FacultyId");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.MailConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("EnableSsl")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SmtpAuth")
                        .HasColumnType("bit");

                    b.Property<string>("SmtpServer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SmtpStartTlsEnable")
                        .HasColumnType("bit");

                    b.Property<bool>("UseDefaultCredentials")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("MailConfiguration");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenThereAreChangesInSubjectContent")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenThereAreChangesInSubjectManagement")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenYouConfirmOrCancelTheTest")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenYouMakeChangesInTheRoleList")
                        .HasColumnType("bit");

                    b.Property<bool>("LeadershipNotificationWhenYouMakeChangesInTheUserList")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationWhenChangingPassword")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationWhenUpdatingAccount")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("StudentNotificationsWhenInstructorsCreateSubjectAnnouncements")
                        .HasColumnType("bit");

                    b.Property<bool>("StudentNotificationsWhenSomeoneCommentsOnASubjectAnnouncement")
                        .HasColumnType("bit");

                    b.Property<bool>("StudentNotificationsWhenSomeoneInteractsWithYourQuestionOrAnswer")
                        .HasColumnType("bit");

                    b.Property<bool>("StudentNotificationsWhenTheLecturerAsksAQuestionInTheSubject")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenSomeoneAsksAQuestionInTheCourseOrInteractsWithYourAnswer")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenSomeoneCommentsOnTheCourseAnnouncement")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenYouAddDocumentsOrUpdateDocumentsAndAssignDocumentsToTeachingClasses")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenYouCreateOrChangeTheNameOrDeleteALectureAndMoveTheLectureToTheSubjectTopic")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenYouUpdateTheTestBankWhenUploadOrCreateNewOrEditAndDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotificationWhenYouUploadOrCreateNewTestQuestionsAndRenameTestQuestions")
                        .HasColumnType("bit");

                    b.Property<bool>("TeacherNotifyWhenYouCreateOrChangeTheNameOrDeleteAResourcesAndMoveTheResourcesToTheSubjectTopic")
                        .HasColumnType("bit");

                    b.Property<string>("UserCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UserCode")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.UserBelongToFaculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsHeadOfDepartment")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBelongToFaculty");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.UserRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("DecentralizationCreateNew")
                        .HasColumnType("bit");

                    b.Property<bool>("DecentralizationDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("DecentralizationEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("DecentralizationSee")
                        .HasColumnType("bit");

                    b.Property<string>("Describe")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("ExamsAndTestsAcceptance")
                        .HasColumnType("bit");

                    b.Property<bool>("ExamsAndTestsCreateNew")
                        .HasColumnType("bit");

                    b.Property<bool>("ExamsAndTestsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("ExamsAndTestsDownload")
                        .HasColumnType("bit");

                    b.Property<bool>("ExamsAndTestsEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("ExamsAndTestsSee")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesAddToSubject")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesCreateNew")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesDownload")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("LecturesAndResourcesSee")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationSee")
                        .HasColumnType("bit");

                    b.Property<bool>("NotificationSettings")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateFilesCreateNew")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateFilesDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateFilesDownload")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateFilesEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("PrivateFilesSee")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("SubjectEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("SubjectSee")
                        .HasColumnType("bit");

                    b.Property<bool>("UserAccountEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("UserAccountSee")
                        .HasColumnType("bit");

                    b.HasKey("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.RefreshToken", b =>
                {
                    b.HasOne("LearningManagementSystem.Entity.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.User", b =>
                {
                    b.HasOne("LearningManagementSystem.Entity.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.UserBelongToFaculty", b =>
                {
                    b.HasOne("LearningManagementSystem.Entity.Faculty", "Faculty")
                        .WithMany("UserBelongToFacultys")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearningManagementSystem.Entity.User", "User")
                        .WithMany("UserBelongToFacultys")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.Faculty", b =>
                {
                    b.Navigation("UserBelongToFacultys");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("UserBelongToFacultys");
                });

            modelBuilder.Entity("LearningManagementSystem.Entity.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
