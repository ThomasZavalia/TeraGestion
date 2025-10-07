using Core.Entidades;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPacienteService
    {
        public Task<PacienteDTO> GetPacienteAsync(int id);
        public Task<PacienteDTO> GetPacientePorDniAsync(string dni);
        public Task<IEnumerable<PacienteDTO>> GetPacientesAsync();
        public Task<PacienteDTO> CrearPacienteAsync(PacienteDTO paciente);
        public Task<PacienteDTO> ActualizarPacienteAsync(PacienteDTO paciente);
        public Task<bool> EliminarPacienteAsync(int id);

    }
}
