using System;

namespace Core.DTOs
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ObraSocial { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
    }
}
