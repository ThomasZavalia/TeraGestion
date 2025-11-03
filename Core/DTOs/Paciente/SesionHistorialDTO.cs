using System;

namespace Core.DTOs.Paciente
{
    public class SesionHistorialDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Notas { get; set; }
        public string Asistencia { get; set; }
    }
}