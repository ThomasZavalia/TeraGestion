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

        public SesionService(ISesionRepository sesionRepository)
        {
            _sesionRepository = sesionRepository;
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
        




        public async Task<Sesion> CrearSesionAsync(Sesion sesion)
        {
            throw new NotImplementedException();

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
