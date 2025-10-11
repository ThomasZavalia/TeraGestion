using Core.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Turno.Input
{
    public class TurnoDtoCreacion
    {
        public int? PacienteId { get; set; } // null si es nuevo paciente
        public string? NombrePaciente { get; set; }
        public string? ApellidoPaciente { get; set; }

        [StringLength(8)]
        public string? DNI { get; set; }

     
        public int? ObraSocialId { get; set; } // solo si tiene obra social
        public bool EsParticular { get; set; } // true = precio manual
        public decimal? Precio { get; set; } // opcional si es particular
        [Required]
        public DateTime Fecha { get; set; }
       


    }
}
