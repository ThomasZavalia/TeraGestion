using AutoMapper;
using Core.DTOs.Paciente;
using Core.DTOs.Pago.Output;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
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


        public async Task<TurnoCalendarioDto> ActualizarTurnoAsync(int id,TurnoDtoActualizar dto)
        {
            var turnoExistente = await _turnoRepository.GetByIdConPaciente(id);
            if (turnoExistente == null)
            { throw new KeyNotFoundException("Turno no encontrado"); }

          

            // Lógica de Precio/OS (Ajusta según tus reglas de negocio)
            if (dto.EsParticular)
            {
                turnoExistente.Precio = dto.Precio; // Toma el precio manual del DTO
                turnoExistente.ObraSocialId = null; // Borra la OS
            }
            else
            {
                // Si NO es particular, usa la OS del DTO y recalcula precio
                turnoExistente.ObraSocialId = dto.ObraSocialId;
                // Recalcula SIEMPRE el precio basado en la OS para evitar inconsistencias
                turnoExistente.Precio = await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);
            }

            
            

           
            await _turnoRepository.Actualizar(turnoExistente);

            // Mapea la entidad actualizada (que ya tiene Paciente) al DTO de respuesta
            return _mapper.Map<TurnoCalendarioDto>(turnoExistente);
        }

        public async Task MarcarComoPagadoAsync(int turnoId, string metodoPago)
        {
            var turno = await _turnoRepository.GetById(turnoId)
                ?? throw new KeyNotFoundException("Turno no encontrado");

            if (turno.Estado == "Pagado")
                throw new ArgumentException("El turno ya está pagado");

            await _pagoService.CrearPago(new PagoDto
            {
                TurnoId = turnoId,
                MetodoPago = metodoPago,
                Fecha = DateTime.Now,
                Monto = turno.Precio
            });

            turno.Estado = "Pagado";
            await _turnoRepository.Actualizar(turno);
        }


        public async Task<TurnoCalendarioDto> CrearTurnoAsync(TurnoDtoCreacion dto)
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
                    else
                    {
                        if (dto.ObraSocialId.HasValue && pacienteAbuscar.ObraSocialId != dto.ObraSocialId)
                        {
                            pacienteAbuscar.ObraSocialId = dto.ObraSocialId;
                            await _pacienteService.ActualizarPacienteAsync(pacienteAbuscar.Id,pacienteAbuscar);
                        }
                    }
                }
                

                decimal precioTurno;

                if (dto.EsParticular && dto.Precio.HasValue) 
                {
                    precioTurno = dto.Precio.Value;
                }
                else
                {
                    precioTurno= await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);

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
                var turnoConPaciente = await _turnoRepository.GetByIdConPaciente(turnoCreado.Id); 
                var turnoDto = _mapper.Map<TurnoCalendarioDto>(turnoCreado);
                return turnoDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"Error al crear el turno: {ex.Message}", ex);
            } 
            
            }
          
        

        public async Task<bool> EliminarTurnoAsync(int id)
        {
            var turnoAeliminar = await _turnoRepository.GetById(id);
            if (turnoAeliminar == null)
            {
                
               throw new KeyNotFoundException("No se encontro el turno");
            }
            await _turnoRepository.Eliminar(id);
            return true;


        }

        public async Task<TurnoDto> GetTurnoAsync(int id)
        {
         var turnoAbuscar = await _turnoRepository.GetById(id);
            if (turnoAbuscar == null)
            {
                throw new KeyNotFoundException("No se encontro el turno");
            }
            var turnoDto = _mapper.Map<TurnoDto>(turnoAbuscar);

            return turnoDto;
        }

        public async Task<IEnumerable<TurnoCalendarioDto>> GetTurnosAsync()
        {
           var turnos = await _turnoRepository.ObtenerTodos();
          var turnosDto = _mapper.Map<IEnumerable<TurnoCalendarioDto>>(turnos);
            return turnosDto;
        }
        public async Task<IEnumerable<Turno>> GetTurnosSinDto() 
        {
        return await _turnoRepository.ObtenerTodos();
        }

        public async Task<IEnumerable<string>> GetAvailableSlotsAsync(DateTime date)
        {
            
            var allSlots = new List<string>
    {
        "16:00", "17:00", "18:00", "19:00", "20:00"
    };
            var fechaUtc = date.ToUniversalTime().Date;
           
            var turnosDelDia = await _turnoRepository.GetTurnosByDayAsync(fechaUtc);


            var bookedSlots = turnosDelDia
         // Convierte la FechaHora (que puede ser UTC) a la hora local del servidor ANTES de formatear
         .Select(t => t.FechaHora.ToLocalTime().ToString("HH:mm"))
         .ToHashSet();

            // 4. Filtra la lista total y devuelve solo los que NO están en bookedSlots
            var availableSlots = allSlots.Where(slot => !bookedSlots.Contains(slot));

            return availableSlots;
        }
    }
}
