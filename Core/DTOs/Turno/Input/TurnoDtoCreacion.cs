using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Turno.Input
{
    public class TurnoDtoCreacion
    {
        public DateTime Fecha { get; set; }
        public string DniPaciente { get; set; }
        public string NombrePaciente { get; set; }

        public string ApellidoPaciente { get; set; }

        public decimal Precio { get; set; }

        public string ObraSocial { get; set; }


    }
}
