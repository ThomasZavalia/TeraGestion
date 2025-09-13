using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagosRepository _pagoRepository;
        public PagoService(IPagosRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }
        public Task<Pago> ActualizarPago(Pago pago)
        {
            throw new NotImplementedException();
        }

        public Task<Pago> CrearPago(Pago pago)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarPago(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pago> GetPago(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pago>> GetPagos()
        {
            throw new NotImplementedException();
        }
    }
}
