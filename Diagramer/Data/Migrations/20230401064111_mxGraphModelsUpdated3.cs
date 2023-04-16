using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class mxGraphModelsUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "As",
                table: "MxPoints",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "MxGeometryId",
                table: "MxArrays",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MxArrays_MxGeometryId",
                table: "MxArrays",
                column: "MxGeometryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MxArrays_MxGeometries_MxGeometryId",
                table: "MxArrays",
                column: "MxGeometryId",
                principalTable: "MxGeometries",
                principalColumn: "MxGeometryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MxArrays_MxGeometries_MxGeometryId",
                table: "MxArrays");

            migrationBuilder.DropIndex(
                name: "IX_MxArrays_MxGeometryId",
                table: "MxArrays");

            migrationBuilder.DropColumn(
                name: "MxGeometryId",
                table: "MxArrays");

            migrationBuilder.AlterColumn<string>(
                name: "As",
                table: "MxPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
