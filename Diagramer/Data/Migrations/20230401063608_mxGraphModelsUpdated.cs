using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class mxGraphModelsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MxCells_MxGeometries_MxGeometryId",
                table: "MxCells");

            migrationBuilder.DropForeignKey(
                name: "FK_MxCells_MxGraphModels_MxGraphModelId",
                table: "MxCells");

            migrationBuilder.DropForeignKey(
                name: "FK_MxGeometries_MxArrays_ArrayMxArrayId",
                table: "MxGeometries");

            migrationBuilder.DropForeignKey(
                name: "FK_MxPoints_MxGeometries_MxGeometryId",
                table: "MxPoints");

            migrationBuilder.DropIndex(
                name: "IX_MxPoints_MxGeometryId",
                table: "MxPoints");

            migrationBuilder.DropIndex(
                name: "IX_MxGeometries_ArrayMxArrayId",
                table: "MxGeometries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MxCells",
                table: "MxCells");

            migrationBuilder.DropIndex(
                name: "IX_MxCells_MxGeometryId",
                table: "MxCells");

            migrationBuilder.DropColumn(
                name: "MxGeometryId",
                table: "MxPoints");

            migrationBuilder.RenameColumn(
                name: "ArrayMxArrayId",
                table: "MxGeometries",
                newName: "MxCellId");

            migrationBuilder.AlterColumn<string>(
                name: "CellId",
                table: "MxGeometries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "TargetId",
                table: "MxCells",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "MxCells",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "MxCells",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "MxCells",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "MxGeometryId",
                table: "MxCells",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MxCells",
                table: "MxCells",
                column: "MxCellId");

            migrationBuilder.CreateIndex(
                name: "IX_MxCells_MxGeometryId",
                table: "MxCells",
                column: "MxGeometryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MxCells_MxGeometries_MxGeometryId",
                table: "MxCells",
                column: "MxGeometryId",
                principalTable: "MxGeometries",
                principalColumn: "MxGeometryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MxCells_MxGraphModels_MxGraphModelId",
                table: "MxCells",
                column: "MxGraphModelId",
                principalTable: "MxGraphModels",
                principalColumn: "MxGraphModelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MxCells_MxGeometries_MxGeometryId",
                table: "MxCells");

            migrationBuilder.DropForeignKey(
                name: "FK_MxCells_MxGraphModels_MxGraphModelId",
                table: "MxCells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MxCells",
                table: "MxCells");

            migrationBuilder.DropIndex(
                name: "IX_MxCells_MxGeometryId",
                table: "MxCells");

            migrationBuilder.RenameColumn(
                name: "MxCellId",
                table: "MxGeometries",
                newName: "ArrayMxArrayId");

            migrationBuilder.AddColumn<Guid>(
                name: "MxGeometryId",
                table: "MxPoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CellId",
                table: "MxGeometries",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TargetId",
                table: "MxCells",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Style",
                table: "MxCells",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "MxCells",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "MxCells",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MxGeometryId",
                table: "MxCells",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MxCells",
                table: "MxCells",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MxPoints_MxGeometryId",
                table: "MxPoints",
                column: "MxGeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_MxGeometries_ArrayMxArrayId",
                table: "MxGeometries",
                column: "ArrayMxArrayId");

            migrationBuilder.CreateIndex(
                name: "IX_MxCells_MxGeometryId",
                table: "MxCells",
                column: "MxGeometryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MxCells_MxGeometries_MxGeometryId",
                table: "MxCells",
                column: "MxGeometryId",
                principalTable: "MxGeometries",
                principalColumn: "MxGeometryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MxCells_MxGraphModels_MxGraphModelId",
                table: "MxCells",
                column: "MxGraphModelId",
                principalTable: "MxGraphModels",
                principalColumn: "MxGraphModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MxGeometries_MxArrays_ArrayMxArrayId",
                table: "MxGeometries",
                column: "ArrayMxArrayId",
                principalTable: "MxArrays",
                principalColumn: "MxArrayId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MxPoints_MxGeometries_MxGeometryId",
                table: "MxPoints",
                column: "MxGeometryId",
                principalTable: "MxGeometries",
                principalColumn: "MxGeometryId");
        }
    }
}
