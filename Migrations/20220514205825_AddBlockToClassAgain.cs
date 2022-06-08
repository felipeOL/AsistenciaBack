using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsistenciaBack.Migrations
{
    public partial class AddBlockToClassAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "clase",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "clase");
        }
    }
}
