using Core.DTOs;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPagoService
    {
        public Task<Pago> GetPago(int id);
        public Task<IEnumerable<Pago>> GetPagos();
        public Task<Pago> CrearPago(CrearPagoDTO pagoDTO);
        public Task<Pago> ActualizarPago(Pago pago);
        public Task<bool> EliminarPago(int id);

    }
}
