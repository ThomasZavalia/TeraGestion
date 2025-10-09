using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Pago.Output
{
    public class PagoDto
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } // Ejemplo: "Efectivo", "Tarjeta", "Transferencia"
        public int TurnoId { get; set; }
    }
}
