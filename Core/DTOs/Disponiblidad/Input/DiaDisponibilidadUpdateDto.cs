using Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Disponiblidad.Input
{
    public class DiaDisponibilidadUpdateDto
    {
        [Required]
        public DayOfWeek DiaSemana { get; set; }
        public bool Disponible { get; set; }

      
        [ValidTimeFormat] 
        public string? HoraInicio { get; set; } 

        [ValidTimeFormat] 
        public string? HoraFin { get; set; } 
     
    }

    public class DisponibilidadUpdateDto
    {
        [Required]
        [MinLength(7, ErrorMessage = "Se deben enviar los 7 días de la semana.")]
        public List<DiaDisponibilidadUpdateDto> Dias { get; set; }
    }
}

