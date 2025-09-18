using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SesionDTO
    {
        public string Asistencia { get; set; } // Ejemplo: "Asistió", "No asistió", "Reprogramado"
        public string Notas { get; set; }
        public DateTime Fecha { get; set; }
        public int TurnoId { get; set; }
        public int PacienteId { get; set; }
    }
}