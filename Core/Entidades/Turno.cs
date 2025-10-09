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
        public string Estado { get; set; } // Ejemplo: "Pendiente", "Completado", "Cancelado"

        public decimal? Precio { get; set; }
        public int PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        public int? ObraSocialId { get; set; }  // Para calcular precio si el paciente no tiene
        public ObraSocial ObraSocial { get; set; }

        public ICollection<Sesion> Sesiones { get; set; }
        public ICollection<Pago> Pagos { get; set; }

    }

}

