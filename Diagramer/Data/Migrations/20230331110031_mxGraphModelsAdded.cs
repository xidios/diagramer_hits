using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class mxGraphModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MxArrays",
                columns: table => new
                {
                    MxArrayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    As = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxArrays", x => x.MxArrayId);
                });

            migrationBuilder.CreateTable(
                name: "MxGraphModels",
                columns: table => new
                {
                    MxGraphModelId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxGraphModels", x => x.MxGraphModelId);
                });

            migrationBuilder.CreateTable(
                name: "MxGeometries",
                columns: table => new
                {
                    MxGeometryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CellId = table.Column<string>(type: "TEXT", nullable: false),
                    X = table.Column<float>(type: "REAL", nullable: false),
                    Y = table.Column<float>(type: "REAL", nullable: false),
                    Height = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<float>(type: "REAL", nullable: false),
                    Relative = table.Column<int>(type: "INTEGER", nullable: false),
                    ArrayMxArrayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    As = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxGeometries", x => x.MxGeometryId);
                    table.ForeignKey(
                        name: "FK_MxGeometries_MxArrays_ArrayMxArrayId",
                        column: x => x.ArrayMxArrayId,
                        principalTable: "MxArrays",
                        principalColumn: "MxArrayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MxCells",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MxCellId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentId = table.Column<string>(type: "TEXT", nullable: false),
                    IsVertex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEdge = table.Column<int>(type: "INTEGER", nullable: false),
                    Style = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    SourceId = table.Column<string>(type: "TEXT", nullable: false),
                    TargetId = table.Column<string>(type: "TEXT", nullable: false),
                    MxGeometryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MxGraphModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MxCells_MxGeometries_MxGeometryId",
                        column: x => x.MxGeometryId,
                        principalTable: "MxGeometries",
                        principalColumn: "MxGeometryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MxCells_MxGraphModels_MxGraphModelId",
                        column: x => x.MxGraphModelId,
                        principalTable: "MxGraphModels",
                        principalColumn: "MxGraphModelId");
                });

            migrationBuilder.CreateTable(
                name: "MxPoints",
                columns: table => new
                {
                    MxPointId = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<float>(type: "REAL", nullable: false),
                    Y = table.Column<float>(type: "REAL", nullable: false),
                    As = table.Column<string>(type: "TEXT", nullable: false),
                    MxArrayId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MxGeometryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MxPoints", x => x.MxPointId);
                    table.ForeignKey(
                        name: "FK_MxPoints_MxArrays_MxArrayId",
                        column: x => x.MxArrayId,
                        principalTable: "MxArrays",
                        principalColumn: "MxArrayId");
                    table.ForeignKey(
                        name: "FK_MxPoints_MxGeometries_MxGeometryId",
                        column: x => x.MxGeometryId,
                        principalTable: "MxGeometries",
                        principalColumn: "MxGeometryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MxCells_MxGeometryId",
                table: "MxCells",
                column: "MxGeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_MxCells_MxGraphModelId",
                table: "MxCells",
                column: "MxGraphModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MxGeometries_ArrayMxArrayId",
                table: "MxGeometries",
                column: "ArrayMxArrayId");

            migrationBuilder.CreateIndex(
                name: "IX_MxPoints_MxArrayId",
                table: "MxPoints",
                column: "MxArrayId");

            migrationBuilder.CreateIndex(
                name: "IX_MxPoints_MxGeometryId",
                table: "MxPoints",
                column: "MxGeometryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MxCells");

            migrationBuilder.DropTable(
                name: "MxPoints");

            migrationBuilder.DropTable(
                name: "MxGraphModels");

            migrationBuilder.DropTable(
                name: "MxGeometries");

            migrationBuilder.DropTable(
                name: "MxArrays");
        }
    }
}
