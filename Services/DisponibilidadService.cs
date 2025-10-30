using AutoMapper;
using Core.DTOs.Disponiblidad.Input;
using Core.DTOs.Disponiblidad.Output;
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
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _disponibilidadRepository;
        private readonly IMapper _mapper;

        public DisponibilidadService(IDisponibilidadRepository disponibilidadRepository, IMapper mapper)
        {
            _disponibilidadRepository = disponibilidadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetDisponibilidadAsync(int userId)
        {
            var disponibilidades = await _disponibilidadRepository.GetByUserIdAsync(userId);
           
            return _mapper.Map<IEnumerable<DisponibilidadDto>>(disponibilidades);
        }

        public async Task UpdateDisponibilidadAsync(int userId, DisponibilidadUpdateDto dto)
        {
            if (dto.Dias == null || dto.Dias.Count != 7)
            {
                throw new ArgumentException("Se deben proporcionar los datos de disponibilidad para los 7 días de la semana.");
            }

          
            foreach (var dia in dto.Dias)
            {
                var inicio = ParseTimeSpan(dia.HoraInicio);
                var fin = ParseTimeSpan(dia.HoraFin);
                if (inicio.HasValue && fin.HasValue && fin <= inicio)
                {
                    throw new ArgumentException($"La hora de fin ({dia.HoraFin}) debe ser posterior a la hora de inicio ({dia.HoraInicio}) para el día {dia.DiaSemana}.");
                }
                if (dia.Disponible && (!inicio.HasValue || !fin.HasValue))
                {
                    
                }
            }

            
            var disponibilidadesActualizadas = _mapper.Map<List<Disponibilidad>>(dto.Dias);

          
            disponibilidadesActualizadas.ForEach(d => d.UsuarioId = userId);


            await _disponibilidadRepository.UpdateDisponibilidadAsync(disponibilidadesActualizadas);
        }

       
        private TimeSpan? ParseTimeSpan(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString)) return null;
            if (TimeSpan.TryParseExact(timeString, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture, out TimeSpan result))
            { return result; }
            return null;
        }
    }
}
