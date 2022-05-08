using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsistenciaBack.Migrations
{
    public partial class ClaseDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Clases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clases_CursoId",
                table: "Clases",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Cursos_CursoId",
                table: "Clases",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Cursos_CursoId",
                table: "Clases");

            migrationBuilder.DropIndex(
                name: "IX_Clases_CursoId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Clases");
        }
    }
}
