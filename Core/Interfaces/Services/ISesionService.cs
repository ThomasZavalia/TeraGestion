using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Sesion;

namespace Core.Interfaces.Services
{
    public interface ISesionService
    {
        public Task<SesionDTO> GetSesionByIdAsync(int id);
        public Task<IEnumerable<SesionDTO>> GetSesionesAsync();
        public Task<SesionDTO> CrearSesionAsync(SesionDTO sesionDTO);
        public Task<SesionDTO> ActualizarSesionAsync(int id, SesionDTO sesionDTO);
        public Task<bool> EliminarSesionAsync(int id);


    }
}
