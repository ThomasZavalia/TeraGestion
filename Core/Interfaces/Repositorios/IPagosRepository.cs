using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositorios
{
    public interface IPagosRepository : IRepository<Entidades.Pago>
    {

        Task<IEnumerable<Pago>> GetPagosFiltradosAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? pacienteId);

    }
}
