using Core.DTOs;
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




        public async Task<Pago> ActualizarPago(Pago pago)
        {
            throw new NotImplementedException();
        }




        public async Task<Pago> CrearPago(CrearPagoDTO pagoDto)
        {
            if (pagoDto.Monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor que cero.");
            }

            if (string.IsNullOrWhiteSpace(pagoDto.MetodoPago))
            {
                throw new ArgumentException("El método de pago no puede estar vacío.");
            }

            var nuevoPago = new Pago
            {
                Monto = pagoDto.Monto,
                MetodoPago = pagoDto.MetodoPago,
                TurnoId = pagoDto.TurnoId,
                Fecha = DateTime.Now
            };

            return await _pagoRepository.Agregar(nuevoPago);
        }




        public async Task<bool> EliminarPago(int id)
        {
            var pagoExistente = await GetPago(id);

            return await _pagoRepository.Eliminar(id);
        }



        public async Task<Pago> GetPago(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo.");

            }

            var pago = await _pagoRepository.GetById(id);


            if (pago == null)
            {
                throw new KeyNotFoundException("Pago no encontrado.");
            }

            return pago;
        }




        public async Task<IEnumerable<Pago>> GetPagos()
        {
            var todosLosPagos = await _pagoRepository.ObtenerTodos();

            if (todosLosPagos == null || !todosLosPagos.Any())
            {
                throw new InvalidOperationException("No hay pagos disponibles.");
            }

            return todosLosPagos;
        }
    }
}
