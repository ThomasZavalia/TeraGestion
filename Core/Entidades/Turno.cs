using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Turno
    {

        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } 

        public decimal? Precio { get; set; }
        public int PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        public int? ObraSocialId { get; set; }
        public ObraSocial ObraSocial { get; set; }

        public string? TokenConfirmacion { get; set; }

        public int Duracion { get; set; }

        public ICollection<Sesion> Sesiones { get; set; }
        public ICollection<Pago> Pagos { get; set; }

    }

}

