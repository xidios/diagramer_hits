using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diagramer.Data.Migrations
{
    public partial class answerUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Answers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Mark",
                table: "Answers",
                type: "REAL",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Answers");
        }
    }
}
