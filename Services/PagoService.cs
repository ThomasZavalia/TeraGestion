using AutoMapper;
using Core.DTOs;
using Core.DTOs.Pago.Output;
using Core.Entidades;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
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
        private readonly IMapper _mapper;
        public PagoService(IPagosRepository pagoRepository,IMapper mapper)
        {
            _pagoRepository = pagoRepository;
            _mapper = mapper;
        }




        public async Task<Pago> ActualizarPago(Pago pago)
        {
            throw new NotImplementedException();
        }




        public async Task<PagoDto> CrearPago(PagoDto pagoDto)
        {
            if (pagoDto.Monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor que cero.");
            }

            if (string.IsNullOrWhiteSpace(pagoDto.MetodoPago))
            {
                throw new ArgumentException("El método de pago no puede estar vacío.");
            }

            var nuevoPago = _mapper.Map<Pago>(pagoDto);

            var pagoCreado = await _pagoRepository.Agregar(nuevoPago);
            return _mapper.Map<PagoDto>(pagoCreado);
        }




        public async Task<bool> EliminarPago(int id)
        {
            var pagoExistente = await GetPago(id);
            if (pagoExistente == null)
            {
                throw new KeyNotFoundException("Pago no encontrado.");
            }

            return await _pagoRepository.Eliminar(id);
        }



        public async Task<PagoDto> GetPago(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo.");

            }
          
            var pago = await _pagoRepository.GetById(id);

            var pagoDto = _mapper.Map<PagoDto>(pago);

            if (pago == null)
            {
                throw new KeyNotFoundException("Pago no encontrado.");
            }

            return pagoDto;
        }




        public async Task<IEnumerable<PagoDto>> GetPagos()
        {
            var todosLosPagos = await _pagoRepository.ObtenerTodos();

            if (todosLosPagos == null || !todosLosPagos.Any())
            {
                throw new InvalidOperationException("No hay pagos disponibles.");
            }
            var pagosDto = _mapper.Map<IEnumerable<PagoDto>>(todosLosPagos);

            return pagosDto;
        }
        public async  Task<IEnumerable<Pago>> GetPagosSinDto()
        {
            return await _pagoRepository.ObtenerTodos();
        }

        public async Task<IEnumerable<PagoDetallesDto>> GetPagosAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? pacienteId)
        {
            var pagos = await _pagoRepository.GetPagosFiltradosAsync(fechaDesde, fechaHasta, pacienteId);
           
            return _mapper.Map<IEnumerable<PagoDetallesDto>>(pagos);
        }
    }
}
