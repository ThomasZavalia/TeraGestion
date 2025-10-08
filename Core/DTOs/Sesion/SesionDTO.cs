using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Sesion
{
    public class SesionDTO
    {
        public int Id { get; set; }
        public DateTime FechaHoraInicio { get; set; }

        public string Asistencia { get; set; } // Ejemplo: "Asistió", "No asistió", "Reprogramado"
        public string Notas { get; set; }

        public int PacienteId { get; set; }

        public int TurnoId { get; set; }
    }
}