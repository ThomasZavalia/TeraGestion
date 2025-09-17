using Core.Entidades;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISesionService
    {
        public Task<Sesion> GetSesionByIdAsync(int id);
        public Task<IEnumerable<Sesion>> GetSesionesAsync();
        public Task<Sesion> CrearSesionAsync(Sesion sesion);
        public Task<Sesion> ActualizarSesionAsync(int id, SesionDTO sesionDTO);
        public Task<bool> EliminarSesionAsync(int id);


    }
}
