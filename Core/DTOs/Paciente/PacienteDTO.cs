using System;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Paciente
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        [Phone]
        public string? Telefono { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string DNI { get; set; }

        public int? ObraSocialId { get; set; }  // nullable si no tiene
    }
}
