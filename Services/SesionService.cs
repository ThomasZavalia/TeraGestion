using AutoMapper;
using Core.DTOs.Sesion;
using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
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

        public SesionService(ITurnoService turnoService, ISesionRepository sesionRepository, IPacienteService pacienteService,IMapper mapper)
        {
            _turnoService = turnoService;
            _sesionRepository = sesionRepository;
            _pacienteService = pacienteService;
            _mapper = mapper;
        }






        public async Task<SesionDTO> ActualizarSesionAsync(int id,SesionDTO sesionDTO)
        {
            var sesionExistente = await _sesionRepository.GetById(id);

            if (sesionExistente == null)
            {
               throw new ArgumentException("Sesion no encontrada");
            }

            // Mapear los valores del DTO sobre la entidad existente
            _mapper.Map(sesionDTO, sesionExistente);

            // Actualizar directamente la sesión existente
            var sesionActualizada = await _sesionRepository.Actualizar(sesionExistente);

            return _mapper.Map<SesionDTO>(sesionActualizada);
        }






        public async Task<SesionDTO> CrearSesionAsync(SesionDTO sesionDTO)
        {
            if (sesionDTO == null)
            {
                throw new ArgumentNullException(nameof(sesionDTO), "El objeto SesionDTO no puede ser nulo.");
            }


            var turnoExistente = await _turnoService.GetTurnoAsync(sesionDTO.TurnoId);
            if (turnoExistente == null)
            {
                throw new ArgumentException("El turno con ID " + sesionDTO.TurnoId + " no fue encontrado.");
            }
            var pacienteId = turnoExistente.PacienteId;
            //var sesionExistente = await GetSesionByIdAsync(sesionDTO.Id);
            //var pacienteExistente = turnoExistente.Paciente;


           

            




            if (pacienteId != sesionDTO.PacienteId)
            {
                throw new ArgumentException("El pacienteId no coincide con el del turno.");
            }






            var nuevaSesion = _mapper.Map<Sesion>(sesionDTO);

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
    }
}
