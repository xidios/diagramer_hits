using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class SubjectUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SubjectId",
                table: "Tasks",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SubjectId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Tasks");
        }
    }
}
