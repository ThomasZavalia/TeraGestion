using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisponibilidadAgregadaSi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiaSemana = table.Column<int>(type: "integer", nullable: false),
                    Disponible = table.Column<bool>(type: "boolean", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time without time zone", nullable: true),
                    HoraFin = table.Column<TimeSpan>(type: "time without time zone", nullable: true),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disponibilidades_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Disponibilidades",
                columns: new[] { "Id", "DiaSemana", "Disponible", "HoraFin", "HoraInicio", "UsuarioId" },
                values: new object[,]
                {
                    { -7, 6, false, null, null, 2 },
                    { -6, 5, true, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), 2 },
                    { -5, 4, true, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), 2 },
                    { -4, 3, true, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), 2 },
                    { -3, 2, true, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), 2 },
                    { -2, 1, true, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0), 2 },
                    { -1, 0, false, null, null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_UsuarioId_DiaSemana",
                table: "Disponibilidades",
                columns: new[] { "UsuarioId", "DiaSemana" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disponibilidades");
        }
    }
}
