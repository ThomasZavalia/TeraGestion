using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarObraSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_ObraSocial_ObraSocialId",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_ObraSocial_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObraSocial",
                table: "ObraSocial");

            migrationBuilder.RenameTable(
                name: "ObraSocial",
                newName: "ObrasSociales");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObrasSociales",
                table: "ObrasSociales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_ObrasSociales_ObraSocialId",
                table: "Pacientes",
                column: "ObraSocialId",
                principalTable: "ObrasSociales",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_ObrasSociales_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId",
                principalTable: "ObrasSociales",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_ObrasSociales_ObraSocialId",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_ObrasSociales_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObrasSociales",
                table: "ObrasSociales");

            migrationBuilder.RenameTable(
                name: "ObrasSociales",
                newName: "ObraSocial");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObraSocial",
                table: "ObraSocial",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_ObraSocial_ObraSocialId",
                table: "Pacientes",
                column: "ObraSocialId",
                principalTable: "ObraSocial",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_ObraSocial_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId",
                principalTable: "ObraSocial",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
