using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class RoomUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_MxGraphModels_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "MxGraphModels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_MxGraphModelId",
                table: "Rooms",
                column: "MxGraphModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_MxGraphModels_MxGraphModelId",
                table: "Rooms",
                column: "MxGraphModelId",
                principalTable: "MxGraphModels",
                principalColumn: "MxGraphModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_MxGraphModels_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "MxGraphModels");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_MxGraphModelId",
                table: "Rooms",
                column: "MxGraphModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_MxGraphModels_MxGraphModelId",
                table: "Rooms",
                column: "MxGraphModelId",
                principalTable: "MxGraphModels",
                principalColumn: "MxGraphModelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
