using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsistenciaBack.Migrations
{
    public partial class AddMissingRequiredProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clase_curso_CourseId",
                table: "clase");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "clase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_clase_curso_CourseId",
                table: "clase",
                column: "CourseId",
                principalTable: "curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clase_curso_CourseId",
                table: "clase");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "clase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_clase_curso_CourseId",
                table: "clase",
                column: "CourseId",
                principalTable: "curso",
                principalColumn: "Id");
        }
    }
}
