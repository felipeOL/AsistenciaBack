using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsistenciaBack.Migrations
{
    public partial class PeriodFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "curso",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_curso_PeriodId",
                table: "curso",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_curso_periodo_PeriodId",
                table: "curso",
                column: "PeriodId",
                principalTable: "periodo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_curso_periodo_PeriodId",
                table: "curso");

            migrationBuilder.DropIndex(
                name: "IX_curso_PeriodId",
                table: "curso");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "curso");
        }
    }
}
