using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class TaskUpdate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Tasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Subjects_SubjectId",
                table: "Tasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
