using AutoMapper;
using Core.DTOs.Paciente;
using Core.DTOs.Pago.Output;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TurnoService : ITurnoService


    {
        private readonly IPacienteService _pacienteService;
        private readonly ITurnoRepository _turnoRepository;
        private readonly IPagoService _pagoService;
        private readonly TeraDbContext _teraDbContext;
        private readonly IMapper _mapper;
        private readonly IObraSocialService _obraSocialService;
        public TurnoService(ITurnoRepository turnoRepository, IPacienteService pacienteService, IPagoService pagoService, TeraDbContext teraDbContext,IMapper mapper, IObraSocialService obraSocialService)
        {
            _turnoRepository = turnoRepository;
            _pacienteService = pacienteService;
            _pagoService = pagoService;
            _teraDbContext = teraDbContext;
            _mapper = mapper;
            _obraSocialService = obraSocialService;
        }


        public async Task<TurnoDto> ActualizarTurnoAsync(TurnoDto turnoDto)
        {
            var turnoExistente = await _turnoRepository.GetById(turnoDto.Id);
            if (turnoExistente == null)
                return null;

            // Actualiza solo las propiedades del DTO sobre la entidad existente
            _mapper.Map(turnoDto, turnoExistente);

            var turnoActualizado = await _turnoRepository.Actualizar(turnoExistente);

            // Devuelve el DTO actualizado (por si el repositorio cambia algo, como Fecha o Estado)
            return _mapper.Map<TurnoDto>(turnoActualizado);
        }

        public async Task MarcarComoPagadoAsync(int turnoId, string metodoPago)
        {
            var turno = await GetTurnoAsync(turnoId);
            if (turno == null) throw new Exception("Turno no encontrado");
            if (turno.Estado == "pagado") throw new Exception("El turno ya está pagado");

            var pago = new PagoDto
            {
                TurnoId = turnoId,
                MetodoPago = metodoPago,
                Fecha = DateTime.UtcNow,
                Monto = turno.Precio 
            };

            await _pagoService.CrearPago(pago);

            turno.Estado = "pagado";
           

            await ActualizarTurnoAsync(turno);
        }


        public async Task<TurnoDto> CrearTurnoAsync(TurnoDtoCreacion dto)
        {
            using var transaction = await _teraDbContext.Database.BeginTransactionAsync();

            try {
                PacienteDTO pacienteAbuscar;

                if (dto.PacienteId.HasValue)
                {
                    pacienteAbuscar = await _pacienteService.GetPacienteAsync(dto.PacienteId.Value);
                }
                else
                {
                    pacienteAbuscar = await _pacienteService.GetPacientePorDniAsync(dto.DNI);

                    if (pacienteAbuscar == null)
                    {
                        var nuevoPaciente = new PacienteDTO
                        {
                            DNI = dto.DNI,
                            Nombre = dto.NombrePaciente,
                            Apellido = dto.ApellidoPaciente,
                            ObraSocialId = dto.ObraSocialId
                        };
                        pacienteAbuscar = await _pacienteService.CrearPacienteAsync(nuevoPaciente);
                    }
                }

                decimal precioTurno;

                if (dto.EsParticular && dto.Precio.HasValue) 
                {
                    precioTurno = dto.Precio.Value;
                }
                else
                {
                    precioTurno= await _obraSocialService.CalcularPrecioTurnoAsync(pacienteAbuscar);

                }

               

                var turno = new Turno
                {

                    FechaHora = dto.Fecha,
                    PacienteId = pacienteAbuscar.Id,
                    Precio = precioTurno,
                    Estado = "Pendiente",
                    ObraSocialId = dto.ObraSocialId

                };
               var turnoCreado = await _turnoRepository.Agregar(turno);


                await transaction.CommitAsync();
                var turnoDto = _mapper.Map<TurnoDto>(turnoCreado);
                return turnoDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear el turno. Detalle: {ex.Message}", ex);
            } 
            
            }
          
        

        public async Task<bool> EliminarTurnoAsync(int id)
        {
            var turnoAeliminar = await _turnoRepository.GetById(id);
            if (turnoAeliminar == null)
            {
                
                return false;
            }
            await _turnoRepository.Eliminar(id);
            return true;


        }

        public async Task<TurnoDto> GetTurnoAsync(int id)
        {
         var turnoAbuscar = await _turnoRepository.GetById(id);
            if (turnoAbuscar == null)
            {
                throw new Exception("No se encontro el turno");
            }
            var turnoDto = _mapper.Map<TurnoDto>(turnoAbuscar);

            return turnoDto;
        }

        public async Task<IEnumerable<TurnoDto>> GetTurnosAsync()
        {
           var turnos = await _turnoRepository.ObtenerTodos();
          var turnosDto = _mapper.Map<IEnumerable<TurnoDto>>(turnos);
            return turnosDto;
        }
    }
}
