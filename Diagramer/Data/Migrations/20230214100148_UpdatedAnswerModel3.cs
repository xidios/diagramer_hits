using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class UpdatedAnswerModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Diagrams_DiagramId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "DiagramId",
                table: "Answers",
                newName: "StudentDiagramId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_DiagramId",
                table: "Answers",
                newName: "IX_Answers_StudentDiagramId");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherDiagramId",
                table: "Answers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TeacherDiagramId",
                table: "Answers",
                column: "TeacherDiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Diagrams_StudentDiagramId",
                table: "Answers",
                column: "StudentDiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Diagrams_TeacherDiagramId",
                table: "Answers",
                column: "TeacherDiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Diagrams_StudentDiagramId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Diagrams_TeacherDiagramId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_TeacherDiagramId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TeacherDiagramId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "StudentDiagramId",
                table: "Answers",
                newName: "DiagramId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_StudentDiagramId",
                table: "Answers",
                newName: "IX_Answers_DiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Diagrams_DiagramId",
                table: "Answers",
                column: "DiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
