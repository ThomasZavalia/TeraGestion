using AutoMapper;
using Core.DTOs.Sesion.Input;
using Core.DTOs.Sesion.Output;
using Core.Entidades;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _sesionRepository;

        private readonly ITurnoService _turnoService;
        private readonly IPacienteService _pacienteService;
        private readonly IMapper _mapper;

        public SesionService(ITurnoService turnoService, ISesionRepository sesionRepository, IPacienteService pacienteService, IMapper mapper)
        {
            _turnoService = turnoService;
            _sesionRepository = sesionRepository;
            _pacienteService = pacienteService;
            _mapper = mapper;
        }






        public async Task<SesionDTO> ActualizarSesionAsync(int id, SesionActualizarDto dto)
        {
            var sesionExistente = await _sesionRepository.GetById(id);
            if (sesionExistente == null)
            {
                throw new KeyNotFoundException($"Sesión con ID {id} no encontrada.");
            }


            sesionExistente.Notas = dto.Notas;

            if (dto.Asistencia != null)
            {
                sesionExistente.Asistencia = dto.Asistencia;
            }



            var sesionActualizada = await _sesionRepository.Actualizar(sesionExistente);
            return _mapper.Map<SesionDTO>(sesionActualizada);
        }






        public async Task<SesionDTO> CrearSesionAsync(SesionCreacionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));


            var sesionExistente = await _sesionRepository.GetByTurnoIdAsync(dto.TurnoId);
            if (sesionExistente != null)
            {

                throw new ArgumentException($"Ya existe una sesión registrada para el turno ID {dto.TurnoId}.");

            }

            var turnoDto = await _turnoService.GetTurnoAsync(dto.TurnoId);
            if (turnoDto == null)
            {
                throw new KeyNotFoundException($"El turno con ID {dto.TurnoId} no fue encontrado.");
            }


            var nuevaSesion = new Sesion
            {
                TurnoId = dto.TurnoId,
                PacienteId = turnoDto.PacienteId,
                FechaHoraInicio = DateTime.SpecifyKind(turnoDto.FechaHora, DateTimeKind.Utc),
              
                Asistencia = dto.Asistencia,
                Notas = null
            };


            var sesionCreada = await _sesionRepository.Agregar(nuevaSesion);
            return _mapper.Map<SesionDTO>(sesionCreada);
        }






        public async Task<bool> EliminarSesionAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID es invalido.");

            var eliminado = await _sesionRepository.Eliminar(id);
            if (!eliminado)
                throw new KeyNotFoundException($"No se encontro la sesión con ID {id}.");

            return true;
        }






        public async Task<SesionDTO> GetSesionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID es invalido.");
            }

            var sesion = await _sesionRepository.GetById(id);

            if (sesion == null)
            {
                throw new KeyNotFoundException($"La sesión con ID {id} no fue encontrada.");
            }

            var sesionDto = _mapper.Map<SesionDTO>(sesion);
            return sesionDto;
        }








        public async Task<IEnumerable<SesionDTO>> GetSesionesAsync()
        {
            var TodasLasSesiones = await _sesionRepository.ObtenerTodos();

            if (TodasLasSesiones == null || !TodasLasSesiones.Any())
            {
                throw new ArgumentException("No se encontraron sesiones.");
            }
            var sesionesDto = _mapper.Map<IEnumerable<SesionDTO>>(TodasLasSesiones);

            return sesionesDto;
        }

        public async Task<SesionDTO> RegistrarAsistenciaAsync(SesionAsistenciaDto dto)
        {
           
            var sesionExistente = await _sesionRepository.GetByTurnoIdAsync(dto.TurnoId);

            if (sesionExistente != null)
            {
                

              
                var sesionParaActualizar = await _sesionRepository.GetById(sesionExistente.Id);

                sesionParaActualizar.Asistencia = dto.Asistencia;
                
                 if (dto.Asistencia == "Ausente") sesionParaActualizar.Notas = "Paciente ausente";

                var sesionActualizada = await _sesionRepository.Actualizar(sesionParaActualizar);
                return _mapper.Map<SesionDTO>(sesionActualizada);
            }
            else
            {

                var turnoDto = await _turnoService.GetTurnoAsync(dto.TurnoId);
                if (turnoDto == null)
                {
                    throw new KeyNotFoundException($"El turno con ID {dto.TurnoId} no fue encontrado.");
                }

                var nuevaSesion = new Sesion
                {
                    TurnoId = dto.TurnoId,
                    PacienteId = turnoDto.PacienteId,
                    FechaHoraInicio = turnoDto.FechaHora,
                    Asistencia = dto.Asistencia,
                    Notas = "" 
                };

                var sesionCreada = await _sesionRepository.Agregar(nuevaSesion);
                return _mapper.Map<SesionDTO>(sesionCreada);
            }
        }
    }
}