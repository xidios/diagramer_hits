using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class RoomUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MxGraphModelId",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_MxGraphModels_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_MxGraphModelId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MxGraphModelId",
                table: "Rooms");
        }
    }
}
