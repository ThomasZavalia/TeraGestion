using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Paciente;

namespace Core.Interfaces
{
    public interface IPacienteService
    {
        public Task<PacienteDTO> GetPacienteAsync(int id);
        public Task<Paciente> GetPacientePorDniAsync(string dni);
        public Task<IEnumerable<PacienteDTO>> GetPacientesAsync();
        public Task<Paciente> CrearPacienteAsync(PacienteDTO pacienteDto);
        public Task<PacienteDTO> ActualizarPacienteAsync(PacienteDTO paciente);
        public Task<bool> EliminarPacienteAsync(int id);

    }
}
