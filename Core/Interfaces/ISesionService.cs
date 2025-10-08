using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Sesion;

namespace Core.Interfaces
{
    public interface ISesionService
    {
        public Task<SesionDTO> GetSesionByIdAsync(int id);
        public Task<IEnumerable<Sesion>> GetSesionesAsync();
        public Task<Sesion> CrearSesionAsync(SesionDTO sesionDTO);
        public Task<SesionDTO> ActualizarSesionAsync( SesionDTO sesionDTO);
        public Task<bool> EliminarSesionAsync(int id);


    }
}
