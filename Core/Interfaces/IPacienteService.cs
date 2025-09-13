using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPacienteService
    {
      public Task<Paciente> GetPacienteAsync(int id);
        public Task<Paciente> GetPacientePorDniAsync(string dni);
        public Task<IEnumerable<Paciente>> GetPacientesAsync();
        public Task<Paciente> CrearPacienteAsync(Paciente paciente);
        public Task<Paciente> ActualizarPacienteAsync(Paciente paciente);
        public Task<bool> EliminarPacienteAsync(int id);

    }
}
