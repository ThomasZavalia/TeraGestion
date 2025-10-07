using Core.DTOs;
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

        public SesionService(ITurnoService turnoService, ISesionRepository sesionRepository, IPacienteService pacienteService)
        {
            _turnoService = turnoService;
            _sesionRepository = sesionRepository;
            _pacienteService = pacienteService;
        }






        public async Task<Sesion> ActualizarSesionAsync(int id, SesionDTO sesionDTO)
        {
            if (sesionDTO == null)
            {
                throw new ArgumentNullException(nameof(sesionDTO), "El objeto SesionDTO no puede ser nulo.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("El ID es invalido.");
            }

            var sesionExistente = await GetSesionByIdAsync(id);


            sesionExistente.Asistencia = sesionDTO.Asistencia;
            sesionExistente.Notas = sesionDTO.Notas;

            return await _sesionRepository.Actualizar(sesionExistente);
        }





        public async Task<Sesion> CrearSesionAsync(CrearSesionDTO sesionDTO)
        {

            var turnoExistente = await _turnoService.GetTurnoAsync(sesionDTO.TurnoId);
            var pacienteId = turnoExistente.PacienteId;
            //var sesionExistente = await GetSesionByIdAsync(sesionDTO.Id);
            var pacienteExistente = turnoExistente.Paciente;


            if(sesionDTO == null)
            {
                throw new ArgumentNullException(nameof(sesionDTO), "El objeto SesionDTO no puede ser nulo.");
            }
            

            if (turnoExistente == null)
            {
                throw new ArgumentException("El turno con ID " + sesionDTO.TurnoId + " no fue encontrado.");
            }


            if (pacienteExistente == null)
            {
                throw new ArgumentException("El paciente con ID " + pacienteId + " no fue encontrado.");
            }


            if (pacienteId != sesionDTO.PacienteId)
            {
                throw new ArgumentException("El pacienteId no coincide con el del turno.");
            }


            if (sesionDTO.Fecha < DateTime.Now)
            {
                throw new ArgumentException("La fecha de la sesion no puede ser en el pasado.");
            }
            


            var nuevaSesion = new Sesion
            {
                FechaHoraInicio = sesionDTO.Fecha,
                TurnoId = turnoExistente.Id,
                PacienteId = sesionDTO.PacienteId
            };

            return await _sesionRepository.Agregar(nuevaSesion);
        }






        public async Task<bool> EliminarSesionAsync(int id)
        {
            var sesionAEliminar = await GetSesionByIdAsync(id);

            return await _sesionRepository.Eliminar(sesionAEliminar.Id);
        }






        public async Task<Sesion> GetSesionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID es invalido.");
            }
            
            var sesion = await _sesionRepository.GetById(id);

            if (sesion == null)
            {
                throw new ArgumentException("La sesion con ID" + id + " no fue encontrada.");
            }

            return sesion;
        }








        public async Task<IEnumerable<Sesion>> GetSesionesAsync()
        {
            var TodasLasSesiones = await _sesionRepository.ObtenerTodos();

            if (TodasLasSesiones == null || !TodasLasSesiones.Any())
            {
                throw new ArgumentException("No se encontraron sesiones.");
            }

            return TodasLasSesiones;
        }
    }
}
