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

        public async Task<Paciente> ActualizarPacienteAsync(Paciente paciente)
        {
              return await _pacienteRepository.Actualizar(paciente);
        }

        public async Task<Paciente> CrearPacienteAsync(Paciente paciente)
        {
              return await _pacienteRepository.Agregar(paciente);
        }

        public async Task<bool> EliminarPacienteAsync(int id)
        {
              return await _pacienteRepository.Eliminar(id);
        }

        public async Task<Paciente> GetPacienteAsync(int id)
        {
              return await _pacienteRepository.GetById(id);
        }

        public async Task<Paciente> GetPacientePorDniAsync(string dni)
        {
              var pacientes = await _pacienteRepository.ObtenerTodos();
            return pacientes.FirstOrDefault(p => p.DNI.ToString() == dni);
        }

        public async Task<IEnumerable<Paciente>> GetPacientesAsync()
        {
              return await _pacienteRepository.ObtenerTodos();
        }
    }
}
