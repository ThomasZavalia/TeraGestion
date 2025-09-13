using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;

namespace Services
{
    public class PacienteService : IPacienteService

    {
        private readonly IPacienteRepository _pacienteRepository;
        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public Task<Paciente> ActualizarPacienteAsync(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> CrearPacienteAsync(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarPacienteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> GetPacienteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> GetPacientePorDniAsync(string dni)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
