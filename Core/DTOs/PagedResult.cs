using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CantidadTotal { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanioPagina { get; set; }
    }
}
