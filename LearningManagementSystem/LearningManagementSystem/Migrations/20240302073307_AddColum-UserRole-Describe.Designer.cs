﻿// <auto-generated />
using System;
using LearningManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearningManagementSystem.Migrations
{
    [DbContext(typeof(LearningManagementSystemContext))]
    [Migration("20240302073307_AddColum-UserRole-Describe")]
    partial class AddColumUserRoleDescribe
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

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

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("User");
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

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("UserRole");
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

            modelBuilder.Entity("LearningManagementSystem.Entity.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
