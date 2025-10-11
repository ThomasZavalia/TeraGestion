using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Pago.Output
{
    public class PagoDto
    {

        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Required]
        public string MetodoPago { get; set; }
        [Required]
        public int TurnoId { get; set; }
    }
}
