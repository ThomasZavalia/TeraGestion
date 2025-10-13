using Core.DTOs.ObraSocial;
using Core.DTOs.Paciente;
using Core.Entidades;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IObraSocialService
    {
        Task<ObraSocial> GetByIdAsync(int id);
        Task<decimal> CalcularPrecioTurnoAsync(int? id);
        public Task<IEnumerable<ObraSocialDto>> GetObrasSocialesAsync();

        public Task<ObraSocialDto> CrearObraSocialAsync(ObraSocialDto obraSocialDto);
        public Task<ObraSocialDto> ActualizarObraSocialAsync(int id,ObraSocialDto obraSocialDto);
        public Task<bool> EliminarObraSocialAsync(int id);


    }
}
