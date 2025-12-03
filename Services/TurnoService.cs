using AutoMapper;
using Core.DTOs.Paciente;
using Core.DTOs.Pago.Output;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using Core.Interfaces.Email;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
using Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly ISesionRepository _sesionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDisponibilidadRepository _disponibilidadRepository;
        private readonly IEmailService _emailService;

        public TurnoService(
         ITurnoRepository turnoRepository,
         IPacienteService pacienteService,
         IPagoService pagoService,
         TeraDbContext teraDbContext,
         IMapper mapper,
         IObraSocialService obraSocialService,
         ISesionRepository sesionRepository,
         IHttpContextAccessor httpContextAccessor,
         IDisponibilidadRepository disponibilidadRepository,
         IEmailService emailService
     )
        {
            _turnoRepository = turnoRepository;
            _pacienteService = pacienteService;
            _pagoService = pagoService;
            _teraDbContext = teraDbContext;
            _mapper = mapper;
            _obraSocialService = obraSocialService;
            _sesionRepository = sesionRepository;
            _httpContextAccessor = httpContextAccessor;
            _disponibilidadRepository = disponibilidadRepository;
            _emailService = emailService;
        }


        public async Task<TurnoCalendarioDto> ActualizarTurnoAsync(int id, TurnoDtoActualizar dto)
        {
            var turnoExistente = await _turnoRepository.GetByIdConPaciente(id);
            if (turnoExistente == null)
            { throw new KeyNotFoundException("Turno no encontrado"); }




            if (dto.EsParticular)
            {
                turnoExistente.Precio = dto.Precio;
                turnoExistente.ObraSocialId = null; 
            }
            else
            {
                
                turnoExistente.ObraSocialId = dto.ObraSocialId;
               
                turnoExistente.Precio = await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);
            }





            await _turnoRepository.Actualizar(turnoExistente);


            return _mapper.Map<TurnoCalendarioDto>(turnoExistente);
        }

        public async Task MarcarComoPagadoAsync(int turnoId, string metodoPago)
        {
            var turno = await _turnoRepository.GetById(turnoId)
                ?? throw new KeyNotFoundException("Turno no encontrado");

            if (turno.Estado.ToLower() == "pagado") 
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

            if (!dto.PacienteId.HasValue && string.IsNullOrWhiteSpace(dto.DNI))
            {
                throw new ArgumentException("Se debe proporcionar un PacienteId o los datos de un nuevo paciente (incluyendo DNI).");
            }
            if (!dto.PacienteId.HasValue && (string.IsNullOrWhiteSpace(dto.NombrePaciente) || string.IsNullOrWhiteSpace(dto.ApellidoPaciente)))
            {
                throw new ArgumentException("Nombre y Apellido son requeridos para un nuevo paciente.");
            }
            if (dto.EsParticular && !dto.Precio.HasValue)
            {
                throw new ArgumentException("Se debe especificar un precio para turnos particulares.");
            }
            if (!dto.EsParticular && !dto.ObraSocialId.HasValue)
            {
                throw new ArgumentException("Se debe seleccionar una Obra Social para turnos no particulares.");
            }


            using var transaction = await _teraDbContext.Database.BeginTransactionAsync();

            try
            {
                PacienteDTO pacienteAsignado;
               
                if (dto.PacienteId.HasValue)
                {


                    pacienteAsignado = await _pacienteService.GetPacienteAsync(dto.PacienteId.Value);
                    if (pacienteAsignado == null)
                    {
                
                        throw new KeyNotFoundException($"No se encontró el paciente con ID {dto.PacienteId.Value}.");
                    }

                  
                    bool necesitaActualizar = false;

                  
                    if (dto.ObraSocialId.HasValue && pacienteAsignado.ObraSocialId != dto.ObraSocialId && !dto.EsParticular)
                    {
                        pacienteAsignado.ObraSocialId = dto.ObraSocialId;
                        necesitaActualizar = true;
                    }

                   
                    if (!pacienteAsignado.Activo)
                    {
                        pacienteAsignado.Activo = true; 
                        necesitaActualizar = true;
                    }

                   
                    if (necesitaActualizar)
                    {
                       
                        await _pacienteService.ActualizarPacienteAsync(pacienteAsignado.Id, pacienteAsignado);
                    }
                }
                else
                {
                    var pacienteExistente = await _pacienteService.GetPacientePorDniAsync(dto.DNI);

                    if (pacienteExistente != null)
                    {
                       
                        throw new ArgumentException($"Ya existe un paciente registrado con el DNI {dto.DNI}. Por favor, seleccione 'Paciente Existente'.");
                    }

                    var nuevoPacienteDto = new PacienteDTO
                    {
                        DNI = dto.DNI,
                        Nombre = dto.NombrePaciente,
                        Apellido = dto.ApellidoPaciente,
                        ObraSocialId = !dto.EsParticular ? dto.ObraSocialId : null,
                        Activo = true
                    };
                    pacienteAsignado = await _pacienteService.CrearPacienteAsync(nuevoPacienteDto);

                    if (pacienteAsignado == null)
                    {
                       
                        throw new InvalidOperationException("Error inesperado al intentar crear el nuevo paciente.");
                    }
                }


                decimal precioTurno;
                if (dto.EsParticular)
                {
                    precioTurno = dto.Precio.Value;
                }
                else
                {
                    precioTurno = await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);
                }


                var turno = new Turno
                {
                    FechaHora = dto.Fecha,
                    PacienteId = pacienteAsignado.Id,
                    Precio = precioTurno,
                    Estado = "Pendiente",
                    ObraSocialId = !dto.EsParticular ? dto.ObraSocialId : null
                };

                var turnoCreado = await _turnoRepository.Agregar(turno);

               
                await transaction.CommitAsync();


                var turnoConPaciente = await _turnoRepository.GetByIdConPaciente(turnoCreado.Id);
                if (turnoConPaciente == null)
                {

                    throw new InvalidOperationException("No se pudo recuperar el turno recién creado con los datos del paciente.");
                }

                var turnoDtoRespuesta = _mapper.Map<TurnoCalendarioDto>(turnoConPaciente);
                return turnoDtoRespuesta;
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();


                throw;
            }
        }



        public async Task<bool> EliminarTurnoAsync(int id)
        {
           
            var turnoACancelar = await _turnoRepository.GetByIdConPaciente(id);

            if (turnoACancelar == null) throw new KeyNotFoundException("Turno no encontrado");
            if (turnoACancelar.Estado.ToLower() == "pagado") throw new ArgumentException("No se puede cancelar un turno pagado.");

           
            turnoACancelar.Estado = "Cancelado";
            await _turnoRepository.Actualizar(turnoACancelar);

           
            if (!string.IsNullOrEmpty(turnoACancelar.Paciente.Email))
            {
                var asunto = "Turno Cancelado";
                var cuerpo = $@"
            <p>Hola {turnoACancelar.Paciente.Nombre},</p>
            <p>Su turno del día <strong>{turnoACancelar.FechaHora:dd/MM/yyyy}</strong> a las <strong>{turnoACancelar.FechaHora:HH:mm} hs</strong> ha sido cancelado.</p>
            <p>Saludos, TeraGestion.</p>";

               
                _ = _emailService.SendEmailAsync(turnoACancelar.Paciente.Email, asunto, cuerpo);
            }
          

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
            var allSlots = new List<string>();

           
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
               
                return allSlots;
            }

          
            var diaDeLaSemana = date.DayOfWeek;

          
            var disponibilidadDia = await _disponibilidadRepository.GetByUserIdAndDayAsync(userId, diaDeLaSemana);

           
            if (disponibilidadDia == null || !disponibilidadDia.Disponible ||
                !disponibilidadDia.HoraInicio.HasValue || !disponibilidadDia.HoraFin.HasValue)
            {
                
                return allSlots; 
            }

            TimeSpan currentSlotTime = disponibilidadDia.HoraInicio.Value;
            TimeSpan endTime = disponibilidadDia.HoraFin.Value;

            while (currentSlotTime < endTime)
            {
                
                allSlots.Add(currentSlotTime.ToString(@"hh\:mm"));
                
                currentSlotTime = currentSlotTime.Add(TimeSpan.FromHours(1));
            }

          
            var fechaUtc = date.ToUniversalTime().Date;
            var turnosDelDia = await _turnoRepository.GetTurnosByDayAsync(fechaUtc); 

            var bookedSlots = turnosDelDia
                .Select(t => t.FechaHora.ToLocalTime().ToString("HH:mm")) 
                .ToHashSet();

          
            var availableSlots = allSlots.Where(slot => !bookedSlots.Contains(slot));

            return availableSlots;
        }

        public async Task<IEnumerable<TurnoCalendarioDto>> GetTurnosDelDiaAsync(DateTime date)
        {

            var turnos = await _turnoRepository.GetTurnosByDayAsync(date.Date); 


            return _mapper.Map<IEnumerable<TurnoCalendarioDto>>(turnos);
        }


        public async Task<TurnoDetalleDto> GetTurnoDetalleAsync(int id)
        {
            var turno = await _turnoRepository.GetByIdConPaciente(id);
            if (turno == null)
            {
                throw new KeyNotFoundException("Turno no encontrado");
            }


            var turnoDto = _mapper.Map<TurnoDetalleDto>(turno);


            var sesionExistente = await _sesionRepository.GetByTurnoIdAsync(id);


            if (sesionExistente != null)
            {

                turnoDto.Asistencia = sesionExistente.Asistencia;
                turnoDto.NotasSesion = sesionExistente.Notas;
                turnoDto.SesionId = sesionExistente.Id;
            }
            else
            {

                turnoDto.Asistencia = null;
                turnoDto.NotasSesion = null;
                turnoDto.SesionId = null;
            }

            return turnoDto;
        }

        public async Task<TurnoCalendarioDto> ReprogramarTurnoAsync(int id, DateTime nuevaFecha)
        {
            var turno = await _turnoRepository.GetByIdConPaciente(id);
            if (turno == null) throw new KeyNotFoundException("Turno no encontrado");

            
            //if (turno.Estado == "Pagado") throw new ArgumentException("No se puede reprogramar un turno pagado");

            
            turno.FechaHora = nuevaFecha;

          
            if (turno.Estado == "Cancelado") turno.Estado = "Pendiente";

            await _turnoRepository.Actualizar(turno);
            return _mapper.Map<TurnoCalendarioDto>(turno);
        }

    }
}

