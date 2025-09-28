using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TurnoDtoRespuesta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string PacienteNombre { get; set; }
        public string TerapeutaNombre { get; set; }
        public string Estado { get; set; }
    }
}
