using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Turno.Output
{
    public class TurnoDto
    {

        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } // Ejemplo: "Pendiente", "Completado", "Cancelado"

        public decimal Precio { get; set; }
        public int PacienteId { get; set; }


    }
}
