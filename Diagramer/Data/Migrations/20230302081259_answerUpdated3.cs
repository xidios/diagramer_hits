using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class answerUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Answers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_GroupId",
                table: "Answers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Groups_GroupId",
                table: "Answers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Groups_GroupId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_GroupId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Answers");
        }
    }
}
