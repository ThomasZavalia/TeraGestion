using System;

namespace Core.DTOs.Paciente
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string DNI { get; set; }

        public int? ObraSocialId { get; set; }  // nullable si no tiene
    }
}
