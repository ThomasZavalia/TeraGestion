using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Reportes
{
    public class ReporteMesDto
    {
       
            public string Mes { get; set; } = string.Empty;
            public decimal Valor { get; set; }  // puede representar cantidad o monto
        
    }
}
