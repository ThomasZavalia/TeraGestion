using Core.Entidades;
using Core.Interfaces.Repositorios;
using Core.DTOs.Paciente;
using AutoMapper;
using Core.Interfaces.Services;

namespace Services
{
    public class PacienteService : IPacienteService

    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMapper _mapper;
        public PacienteService(IPacienteRepository pacienteRepository, IMapper mapper)
        {
            _pacienteRepository = pacienteRepository;
            _mapper = mapper;
        }

        public async Task<PacienteDTO> ActualizarPacienteAsync(int id, PacienteDTO pacienteDto)
        {
            /*  if (pacienteDto == null)
                    throw new ArgumentNullException(nameof(pacienteDto));

              if (string.IsNullOrWhiteSpace(pacienteDto.Nombre) || string.IsNullOrWhiteSpace(pacienteDto.Apellido))
                    throw new ArgumentException("Nombre y Apellido son obligatorios");
              var pacienteExistente = await _pacienteRepository.GetById(id);
        if (pacienteExistente == null) { throw new KeyNotFoundException("Paciente no encontrado"); }

        pacienteExistente.ObraSocial = null; // Evitar problemas de seguimiento de entidades


        _mapper.Map(pacienteDto, pacienteExistente);

        var actualizado = await _pacienteRepository.Actualizar(pacienteExistente);
        return _mapper.Map<PacienteDTO>(actualizado);*/



            if (pacienteDto == null) throw new ArgumentNullException(nameof(pacienteDto));
            if (string.IsNullOrWhiteSpace(pacienteDto.Nombre) || string.IsNullOrWhiteSpace(pacienteDto.Apellido))
                throw new ArgumentException("Nombre y Apellido son obligatorios");

            // 1. Mapea el DTO a una entidad "limpia" (no vigilada).
            //    El perfil de AutoMapper que ignora 'Id' y 'ObraSocial' es clave aquí.
            var pacienteParaActualizar = _mapper.Map<Paciente>(pacienteDto);

            // 2. Pasamos el 'id' (de la URL) y los 'nuevos datos' (del DTO)
            //    al repositorio. Él se encargará de todo.
            var actualizado = await _pacienteRepository.Actualizar(id, pacienteParaActualizar);

            // 3. Si el repositorio devuelve null, es que no lo encontró.
            if (actualizado == null)
            {
                throw new KeyNotFoundException("Paciente no encontrado");
            }

            // 4. Mapea la entidad final (ya guardada) de vuelta a un DTO.
            return _mapper.Map<PacienteDTO>(actualizado);






        }

        public async Task<PacienteDTO> CrearPacienteAsync(PacienteDTO pacienteDto)
        {
            try
            {

                var nuevoPaciente = _mapper.Map<Paciente>(pacienteDto);
                var creado = await _pacienteRepository.Agregar(nuevoPaciente);
                return _mapper.Map<PacienteDTO>(creado);
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, registrar el error)
                throw new Exception("Error al crear el paciente", ex);
            }


        }

        public async Task<bool> EliminarPacienteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido");
            var pacienteExistente = await _pacienteRepository.GetById(id);
            if (pacienteExistente == null) { throw new KeyNotFoundException("Paciente no encontrado"); }

            await _pacienteRepository.Eliminar(id);

            return true;
        }

        public async Task<PacienteDTO> GetPacienteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido");
            var paciente = await _pacienteRepository.GetById(id);
            if (paciente == null) { throw new KeyNotFoundException("Paciente no encontrado"); }

            return MapToDto(paciente);
        }

        public async Task<PacienteDTO> GetPacientePorDniAsync(string dni)
        {
            if (dni == null) { throw new ArgumentException("Obligatorio introducir el dni"); }
            var pacientes = await _pacienteRepository.ObtenerTodos();
            var paciente = pacientes.FirstOrDefault(p => p.DNI == dni);

            return MapToDto(paciente);

        }

        public async Task<IEnumerable<PacienteDTO>> GetPacientesAsync()
        {
            var pacientes = await _pacienteRepository.ObtenerTodos();

            var pacientesDTO = _mapper.Map<IEnumerable<PacienteDTO>>(pacientes);

            return pacientesDTO;
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
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                DNI = paciente.DNI,

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
                ObraSocialId = dto.ObraSocialId,
                Telefono = dto.Telefono,
                Email = dto.Email,
                DNI = dto.DNI
            };
        }

        public async Task<IEnumerable<Paciente>> GetPacientesSinDto()
        {

            return await _pacienteRepository.ObtenerTodos();
        }

        public async Task<IEnumerable<PacienteSimpleDto>> BuscarPacientesAsync(string query)
        {
            var pacientes = await _pacienteRepository.BuscarAsync(query);
            return _mapper.Map<IEnumerable<PacienteSimpleDto>>(pacientes);
        }





        public async Task<PacienteDetalleDTO> GetPacienteDetallesAsync(int id)
        {
            var paciente = await _pacienteRepository.GetDetallesByIdAsync(id);

            if (paciente == null)
            {
                throw new KeyNotFoundException("Paciente no encontrado");
            }

            return _mapper.Map<PacienteDetalleDTO>(paciente);
        }

            public async Task<bool> CheckDniExistsAsync(string dni)
            {
                if (string.IsNullOrEmpty(dni))
                {
                    return false;


                }


                var paciente = await GetPacientePorDniAsync(dni);
                return paciente != null;

            }
        }
    } 


