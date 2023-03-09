using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class RoomAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Connections",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Connections",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_GroupId",
                table: "Connections",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_RoomId",
                table: "Connections",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Groups_GroupId",
                table: "Connections",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Rooms_RoomId",
                table: "Connections",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Groups_GroupId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Rooms_RoomId",
                table: "Connections");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Connections_GroupId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_RoomId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Connections");
        }
    }
}
