using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class DiagramUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Diagram",
                table: "Tasks",
                newName: "DiagramId");

            migrationBuilder.CreateTable(
                name: "Diagrams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    XML = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagrams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DiagramId",
                table: "Tasks",
                column: "DiagramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Diagrams_DiagramId",
                table: "Tasks",
                column: "DiagramId",
                principalTable: "Diagrams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Diagrams_DiagramId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Diagrams");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_DiagramId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DiagramId",
                table: "Tasks",
                newName: "Diagram");
        }
    }
}
