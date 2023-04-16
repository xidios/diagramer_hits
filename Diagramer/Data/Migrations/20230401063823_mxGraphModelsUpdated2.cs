using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class mxGraphModelsUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MxGeometryId",
                table: "MxPoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MxPoints_MxGeometryId",
                table: "MxPoints",
                column: "MxGeometryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MxPoints_MxGeometries_MxGeometryId",
                table: "MxPoints",
                column: "MxGeometryId",
                principalTable: "MxGeometries",
                principalColumn: "MxGeometryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MxPoints_MxGeometries_MxGeometryId",
                table: "MxPoints");

            migrationBuilder.DropIndex(
                name: "IX_MxPoints_MxGeometryId",
                table: "MxPoints");

            migrationBuilder.DropColumn(
                name: "MxGeometryId",
                table: "MxPoints");
        }
    }
}
