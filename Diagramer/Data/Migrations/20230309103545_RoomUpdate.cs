using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class RoomUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_GroupId",
                table: "Rooms",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_TaskId",
                table: "Rooms",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Tasks_TaskId",
                table: "Rooms",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Tasks_TaskId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_GroupId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_TaskId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Rooms");
        }
    }
}
