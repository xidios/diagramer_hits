﻿// <auto-generated />
using System;
using Diagramer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Diagramer.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230401080841_RoomUpdate3")]
    partial class RoomUpdate3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("ApplicationUserGroup", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentsId")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupsId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("ApplicationUserGroup");
                });

            modelBuilder.Entity("ApplicationUserSubject", b =>
                {
                    b.Property<Guid>("SubjectsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ApplicationUserSubject");
                });

            modelBuilder.Entity("CategoryTask", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("TEXT");

                    b.HasKey("CategoriesId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("CategoryTask");
                });

            modelBuilder.Entity("Diagramer.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<float?>("Mark")
                        .HasColumnType("REAL");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StudentDiagramId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherDiagramId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("StudentDiagramId");

                    b.HasIndex("TaskId");

                    b.HasIndex("TeacherDiagramId");

                    b.HasIndex("UserId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Diagramer.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Diagramer.Models.Diagram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("XML")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Diagrams");
                });

            modelBuilder.Entity("Diagramer.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Diagramer.Models.Hub.HubConnection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoomId");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("Diagramer.Models.Hub.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MxGraphModelId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("MxGraphModelId")
                        .IsUnique();

                    b.HasIndex("TaskId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Diagramer.Models.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxArray", b =>
                {
                    b.Property<Guid>("MxArrayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("As")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MxGeometryId")
                        .HasColumnType("TEXT");

                    b.HasKey("MxArrayId");

                    b.HasIndex("MxGeometryId")
                        .IsUnique();

                    b.ToTable("MxArrays");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxCell", b =>
                {
                    b.Property<Guid>("MxCellId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IsEdge")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IsVertex")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("MxGeometryId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MxGraphModelId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Style")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MxCellId");

                    b.HasIndex("MxGeometryId")
                        .IsUnique();

                    b.HasIndex("MxGraphModelId");

                    b.ToTable("MxCells");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxGraphModel", b =>
                {
                    b.Property<Guid>("MxGraphModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("TEXT");

                    b.HasKey("MxGraphModelId");

                    b.ToTable("MxGraphModels");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxPoint", b =>
                {
                    b.Property<Guid>("MxPointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("As")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MxArrayId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MxGeometryId")
                        .HasColumnType("TEXT");

                    b.Property<float>("X")
                        .HasColumnType("REAL");

                    b.Property<float>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("MxPointId");

                    b.HasIndex("MxArrayId");

                    b.HasIndex("MxGeometryId");

                    b.ToTable("MxPoints");
                });

            modelBuilder.Entity("Diagramer.Models.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Diagramer.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DiagramId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsGroupTask")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastResponse")
                        .HasColumnType("TEXT");

                    b.Property<float?>("Mark")
                        .HasColumnType("REAL");

                    b.Property<string>("MarkDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DiagramId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("GroupTask", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("GroupTask");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MxGeometry", b =>
                {
                    b.Property<Guid>("MxGeometryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("As")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CellId")
                        .HasColumnType("TEXT");

                    b.Property<float>("Height")
                        .HasColumnType("REAL");

                    b.Property<Guid>("MxCellId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Relative")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Width")
                        .HasColumnType("REAL");

                    b.Property<float>("X")
                        .HasColumnType("REAL");

                    b.Property<float>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("MxGeometryId");

                    b.ToTable("MxGeometries");
                });

            modelBuilder.Entity("ApplicationUserGroup", b =>
                {
                    b.HasOne("Diagramer.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationUserSubject", b =>
                {
                    b.HasOne("Diagramer.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CategoryTask", b =>
                {
                    b.HasOne("Diagramer.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Diagramer.Models.Answer", b =>
                {
                    b.HasOne("Diagramer.Models.Group", "Group")
                        .WithMany("Answers")
                        .HasForeignKey("GroupId");

                    b.HasOne("Diagramer.Models.Diagram", "StudentDiagram")
                        .WithMany()
                        .HasForeignKey("StudentDiagramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Task", "Task")
                        .WithMany("Answers")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Diagram", "TeacherDiagram")
                        .WithMany()
                        .HasForeignKey("TeacherDiagramId");

                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", "User")
                        .WithMany("Answers")
                        .HasForeignKey("UserId");

                    b.Navigation("Group");

                    b.Navigation("StudentDiagram");

                    b.Navigation("Task");

                    b.Navigation("TeacherDiagram");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diagramer.Models.Hub.HubConnection", b =>
                {
                    b.HasOne("Diagramer.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Hub.Room", "Room")
                        .WithMany("HubConnections")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Room");

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diagramer.Models.Hub.Room", b =>
                {
                    b.HasOne("Diagramer.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.mxGraph.MxGraphModel", "MxGraphModel")
                        .WithOne("Room")
                        .HasForeignKey("Diagramer.Models.Hub.Room", "MxGraphModelId");

                    b.HasOne("Diagramer.Models.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("MxGraphModel");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxArray", b =>
                {
                    b.HasOne("MxGeometry", "MxGeometry")
                        .WithOne("Array")
                        .HasForeignKey("Diagramer.Models.mxGraph.MxArray", "MxGeometryId");

                    b.Navigation("MxGeometry");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxCell", b =>
                {
                    b.HasOne("MxGeometry", "MxGeometry")
                        .WithOne("MxCell")
                        .HasForeignKey("Diagramer.Models.mxGraph.MxCell", "MxGeometryId");

                    b.HasOne("Diagramer.Models.mxGraph.MxGraphModel", null)
                        .WithMany("Cells")
                        .HasForeignKey("MxGraphModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("MxGeometry");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxPoint", b =>
                {
                    b.HasOne("Diagramer.Models.mxGraph.MxArray", "MxArray")
                        .WithMany("MxPoint")
                        .HasForeignKey("MxArrayId");

                    b.HasOne("MxGeometry", "MxGeometry")
                        .WithMany("Position")
                        .HasForeignKey("MxGeometryId");

                    b.Navigation("MxArray");

                    b.Navigation("MxGeometry");
                });

            modelBuilder.Entity("Diagramer.Models.Task", b =>
                {
                    b.HasOne("Diagramer.Models.Diagram", "Diagram")
                        .WithMany()
                        .HasForeignKey("DiagramId");

                    b.HasOne("Diagramer.Models.Subject", "Subject")
                        .WithMany("Tasks")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diagram");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("GroupTask", b =>
                {
                    b.HasOne("Diagramer.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Diagramer.Models.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Diagramer.Models.Group", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Diagramer.Models.Hub.Room", b =>
                {
                    b.Navigation("HubConnections");
                });

            modelBuilder.Entity("Diagramer.Models.Identity.ApplicationUser", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxArray", b =>
                {
                    b.Navigation("MxPoint");
                });

            modelBuilder.Entity("Diagramer.Models.mxGraph.MxGraphModel", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Diagramer.Models.Subject", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Diagramer.Models.Task", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("MxGeometry", b =>
                {
                    b.Navigation("Array");

                    b.Navigation("MxCell");

                    b.Navigation("Position");
                });
#pragma warning restore 612, 618
        }
    }
}
