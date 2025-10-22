using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Turno.Output
{
    public class TurnoCalendarioDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal? Precio { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set;}
        public string PacienteApellido { get; set; }
    }
}
