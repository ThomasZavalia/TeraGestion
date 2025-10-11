using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Sesion
{
    public class SesionDTO
    {
        public int Id { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public string Asistencia { get; set; } // Ejemplo: "Asistió", "No asistió", "Reprogramado"
        public string Notas { get; set; }

        [Required]
        public int PacienteId { get; set; }
        [Required]
        public int TurnoId { get; set; }
    }
}