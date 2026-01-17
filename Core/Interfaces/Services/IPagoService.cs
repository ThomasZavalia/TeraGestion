using Core.DTOs;
using Core.DTOs.Pago.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPagoService
    {
        public Task<PagoDto> GetPago(int id);
        public Task<IEnumerable<PagoDto>> GetPagos();
        public Task<PagoDto> CrearPago(PagoDto pago);
        public Task<Pago> ActualizarPago(Pago pago);
        public Task<bool> EliminarPago(int id);
        public Task<IEnumerable<Pago>> GetPagosSinDto();
        public Task<IEnumerable<PagoDetallesDto>> GetPagosAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? pacienteId);

    }
}
