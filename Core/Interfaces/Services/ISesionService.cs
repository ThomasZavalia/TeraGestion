using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Sesion.Output;
using Core.DTOs.Sesion.Input;

namespace Core.Interfaces.Services
{
    public interface ISesionService
    {
        public Task<SesionDTO> GetSesionByIdAsync(int id);
        public Task<IEnumerable<SesionDTO>> GetSesionesAsync();
        
        Task<SesionDTO> CrearSesionAsync(SesionCreacionDto dto);
       
        Task<SesionDTO> ActualizarSesionAsync(int id, SesionActualizarDto dto);
        public Task<bool> EliminarSesionAsync(int id);

        Task<SesionDTO> RegistrarAsistenciaAsync(SesionAsistenciaDto dto);


    }
}
