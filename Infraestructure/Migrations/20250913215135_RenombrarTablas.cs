using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_Paciente_PacienteId",
                table: "Sesiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Paciente_PacienteId",
                table: "Turnos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Paciente",
                table: "Paciente");

            migrationBuilder.RenameTable(
                name: "Paciente",
                newName: "Pacientes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pacientes",
                table: "Pacientes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_Pacientes_PacienteId",
                table: "Sesiones",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Pacientes_PacienteId",
                table: "Turnos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_Pacientes_PacienteId",
                table: "Sesiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Pacientes_PacienteId",
                table: "Turnos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pacientes",
                table: "Pacientes");

            migrationBuilder.RenameTable(
                name: "Pacientes",
                newName: "Paciente");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Paciente",
                table: "Paciente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_Paciente_PacienteId",
                table: "Sesiones",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Paciente_PacienteId",
                table: "Turnos",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
