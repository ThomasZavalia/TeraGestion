using Core.Entidades;
using Core.DTOs;
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

            public async Task<PacienteDTO> ActualizarPacienteAsync(PacienteDTO pacienteDto)
            {
                  if (pacienteDto == null)
                        throw new ArgumentNullException(nameof(pacienteDto));
                  if (string.IsNullOrWhiteSpace(pacienteDto.Nombre) || string.IsNullOrWhiteSpace(pacienteDto.Apellido))
                        throw new ArgumentException("Nombre y Apellido son obligatorios");
                  var paciente = MapToEntity(pacienteDto);
                  var actualizado = await _pacienteRepository.Actualizar(paciente);
                  return MapToDto(actualizado);
            }

            public async Task<Paciente> CrearPacienteAsync(Paciente paciente)
            {
                  if (paciente == null)
            {
                return null;
            }
                  var creado = await _pacienteRepository.Agregar(paciente);
            return creado;


        }

            public async Task<bool> EliminarPacienteAsync(int id)
            {
                  if (id <= 0)
                        throw new ArgumentException("Id inválido");
                  return await _pacienteRepository.Eliminar(id);
            }

            public async Task<PacienteDTO> GetPacienteAsync(int id)
            {
                  if (id <= 0)
                        throw new ArgumentException("Id inválido");
                  var paciente = await _pacienteRepository.GetById(id);
                  if (paciente == null)
                        return null;
                  return MapToDto(paciente);
            }

            public async Task<Paciente> GetPacientePorDniAsync(string dni)
            {
           if (dni == null) { return null; }
            var pacientes = await _pacienteRepository.ObtenerTodos();
            return pacientes.FirstOrDefault(p => p.DNI == dni);

            }

            public async Task<IEnumerable<PacienteDTO>> GetPacientesAsync()
            {
                  var pacientes = await _pacienteRepository.ObtenerTodos();
                  return pacientes.Select(MapToDto);
            }

            private PacienteDTO MapToDto(Paciente paciente)
            {
                  if (paciente == null) return null;
                  return new PacienteDTO
                  {
                        Id = paciente.Id,
                        Nombre = paciente.Nombre,
                        Apellido = paciente.Apellido,
                        FechaNacimiento = paciente.FechaNacimiento,
                        ObraSocial = paciente.ObraSocial,
                        Telefono = paciente.Telefono,
                        Email = paciente.Email,
                        DNI = paciente.DNI
                  };
            }

            private Paciente MapToEntity(PacienteDTO dto)
            {
                  if (dto == null) return null;
                  return new Paciente
                  {
                        Id = dto.Id,
                        Nombre = dto.Nombre,
                        Apellido = dto.Apellido,
                        FechaNacimiento = dto.FechaNacimiento,
                        ObraSocial = dto.ObraSocial,
                        Telefono = dto.Telefono,
                        Email = dto.Email,
                        DNI = dto.DNI
                  };
            }
    }
}
