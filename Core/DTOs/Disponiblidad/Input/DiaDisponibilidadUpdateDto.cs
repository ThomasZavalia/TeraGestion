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
        public bool Disponible { get; set; } = true;


        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Formato de hora inválido (HH:mm).")]
        public string HoraInicio { get; set; }

        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Formato de hora inválido (HH:mm).")]
        public string HoraFin { get; set; }

    }

    
    public class DisponibilidadUpdateDto
    {
        [Required]
        [MinLength(7)] 
        public List<DiaDisponibilidadUpdateDto> Dias { get; set; }
    }
}

