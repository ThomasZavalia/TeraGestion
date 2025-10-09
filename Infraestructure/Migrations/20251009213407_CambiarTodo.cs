using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambiarTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObraSocial",
                table: "Pacientes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Turnos",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "ObraSocialId",
                table: "Turnos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Sesiones",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Pagos",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "MetodoPago",
                table: "Pagos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ObraSocialId",
                table: "Pacientes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ObraSocial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PrecioTurno = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObraSocial", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_TurnoId",
                table: "Sesiones",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ObraSocialId",
                table: "Pacientes",
                column: "ObraSocialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_ObraSocial_ObraSocialId",
                table: "Pacientes",
                column: "ObraSocialId",
                principalTable: "ObraSocial",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_Turnos_TurnoId",
                table: "Sesiones",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_ObraSocial_ObraSocialId",
                table: "Turnos",
                column: "ObraSocialId",
                principalTable: "ObraSocial",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_ObraSocial_ObraSocialId",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_Turnos_TurnoId",
                table: "Sesiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_ObraSocial_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "ObraSocial");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Sesiones_TurnoId",
                table: "Sesiones");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_ObraSocialId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "ObraSocialId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "ObraSocialId",
                table: "Pacientes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Turnos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Sesiones",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Pagos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "MetodoPago",
                table: "Pagos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Pacientes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Pacientes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ObraSocial",
                table: "Pacientes",
                type: "text",
                nullable: true);
        }
    }
}
