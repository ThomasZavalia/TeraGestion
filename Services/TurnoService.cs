using AutoMapper;
using Core.DTOs.Paciente;
using Core.DTOs.Pago.Output;
using Core.DTOs.Public;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using Core.Interfaces.Email;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
using Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
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
        private readonly INotificacionService _notificacionService;
        private readonly IConfiguracionService _configService;


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
         IEmailService emailService,
         INotificacionService notificacionService,
         IConfiguracionService configService
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
            _notificacionService = notificacionService;
            _configService = configService;
        }


        public async Task<TurnoCalendarioDto> ActualizarTurnoAsync(int id, TurnoDtoActualizar dto)
        {
            var turnoExistente = await _turnoRepository.GetByIdConPaciente(id);
            if (turnoExistente == null)
            { throw new KeyNotFoundException("Turno no encontrado"); }




            if (dto.ObraSocialId.HasValue)
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
                throw new ArgumentException("Se debe proporcionar un PacienteId o los datos de un nuevo paciente.");
            }

           
            if (!dto.ObraSocialId.HasValue)
            {
                throw new ArgumentException("Se debe seleccionar una Cobertura (o Particular).");
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

                   
                    if (pacienteAsignado.ObraSocialId != dto.ObraSocialId)
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
                        ObraSocialId =  dto.ObraSocialId,
                        Activo = true
                    };
                    pacienteAsignado = await _pacienteService.CrearPacienteAsync(nuevoPacienteDto);

                    if (pacienteAsignado == null)
                    {
                       
                        throw new InvalidOperationException("Error inesperado al intentar crear el nuevo paciente.");
                    }
                }


                decimal precioTurno = await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);

                int duracion = await _configService.GetDuracionAsync(2);

                var turno = new Turno
                {
                    FechaHora = dto.Fecha,
                    PacienteId = pacienteAsignado.Id,
                    Precio = precioTurno,
                    Estado = "Pendiente",
                    ObraSocialId = dto.ObraSocialId ,
                    Duracion = duracion
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
            int userId;

           

            
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int parsedId))
            {
               
                userId = parsedId;
            }
            else
            {
                
                userId = 2;
            }

            
            var diaDeLaSemana = date.DayOfWeek;


            var disponibilidadDia = await _disponibilidadRepository.GetByUserIdAndDayAsync(userId, diaDeLaSemana);

           
            if (disponibilidadDia == null || !disponibilidadDia.Disponible ||
                !disponibilidadDia.HoraInicio.HasValue || !disponibilidadDia.HoraFin.HasValue)
            {
                
                return allSlots; 
            }


            int duracionMinutos = await _configService.GetDuracionAsync(userId);

            TimeSpan currentSlotTime = disponibilidadDia.HoraInicio.Value;
            TimeSpan endTime = disponibilidadDia.HoraFin.Value;

            while (currentSlotTime < endTime)
            {
               
                if (currentSlotTime.Add(TimeSpan.FromMinutes(duracionMinutos)) <= endTime)
                {
                    allSlots.Add(currentSlotTime.ToString(@"hh\:mm"));
                }

               
                currentSlotTime = currentSlotTime.Add(TimeSpan.FromMinutes(duracionMinutos));
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

        public async Task<TurnoCalendarioDto> ReservarTurnoPublicoAsync(ReservaDto dto)
        {
            
            var slots = await GetAvailableSlotsAsync(dto.FechaHora.Date);
            var horaSolicitada = dto.FechaHora.ToLocalTime().ToString("HH:mm");

            if (!slots.Contains(horaSolicitada))
            {
                throw new InvalidOperationException("Lo sentimos, ese horario ya no está disponible.");
            }

            using var transaction = await _teraDbContext.Database.BeginTransactionAsync();
            try
            {
                
                var paciente = await _pacienteService.GetPacientePorDniAsync(dto.DNI);
                int pacienteId;
                string nombreFinal;
                string apellidoFinal;

                if (paciente != null)
                {
                    
                    pacienteId = paciente.Id;
                    nombreFinal = paciente.Nombre;
                    apellidoFinal = paciente.Apellido;


                }
                else
                {
                    
                    var nuevoPaciente = new PacienteDTO
                    {
                        Nombre = dto.Nombre,
                        Apellido = dto.Apellido,
                        DNI = dto.DNI,
                        Email = dto.Email,
                        Telefono = dto.Telefono,
                        Activo = true
                       
                    };
                    var pacienteCreado = await _pacienteService.CrearPacienteAsync(nuevoPaciente);
                    pacienteId = pacienteCreado.Id;
                    nombreFinal = dto.Nombre;
                    apellidoFinal = dto.Apellido;
                }

               
                var yaTieneTurno = await _turnoRepository.ExisteTurnoPorPacienteYFecha(pacienteId, dto.FechaHora);
                if (yaTieneTurno)
                {
                    throw new InvalidOperationException("Ya tienes un turno reservado para este día.");
                }

                decimal precioCalculado = 0;

              
                if (dto.ObraSocialId.HasValue)
                {
                    
                    precioCalculado = await _obraSocialService.CalcularPrecioTurnoAsync(dto.ObraSocialId);
                }
                var token = Guid.NewGuid().ToString("N");

                int duracion = await _configService.GetDuracionAsync(2);

                var turno = new Turno
                {
                    FechaHora = dto.FechaHora,
                    PacienteId = pacienteId,
                    Estado = "PendienteConfirmacion",
                    TokenConfirmacion = token,
                    ObraSocialId = dto.ObraSocialId, 
                    Precio = precioCalculado,
                    Duracion = duracion

                };

                var turnoCreado = await _turnoRepository.Agregar(turno);
                await transaction.CommitAsync();



                var baseUrl = "http://localhost:5173"; 
                var link = $"{baseUrl}/confirmar-turno?token={token}&id={turnoCreado.Id}";

               

                string timeZoneId = "Argentina Standard Time";
                TimeZoneInfo zonaHoraria;

                try
                {
                    zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    
                    zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
                }

               
                var fechaUtc = DateTime.SpecifyKind(dto.FechaHora, DateTimeKind.Utc);
                var fechaLocal = TimeZoneInfo.ConvertTimeFromUtc(fechaUtc, zonaHoraria);

                var cuerpoPaciente = $@"
    <h1>Turno Confirmado</h1>
    <p>Hola {dto.Nombre}, te esperamos el {fechaLocal:dd/MM/yyyy} a las {fechaLocal:HH:mm} hs.</p>
    <p><strong>Para confirmar tu reserva, por favor haz clic en el siguiente enlace:</strong></p>
    <p><a href='{link}'>CONFIRMAR MI TURNO</a></p>
    <p>Si no fuiste tú, ignora este mensaje.</p>";

                _ = _emailService.SendEmailAsync(dto.Email, "Acción Requerida: Confirma tu Turno", cuerpoPaciente);


                // _ = _emailService.SendEmailAsync("tu_mail@gmail.com", "Nuevo Turno Online", $"El paciente {dto.Nombre} reservó para el {dto.FechaHora}");


                var turnoConPaciente = await _turnoRepository.GetByIdConPaciente(turnoCreado.Id);
                return _mapper.Map<TurnoCalendarioDto>(turnoConPaciente);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ConfirmarTurnoAsync(int id, string token)
        {
            var turno = await _turnoRepository.GetByIdConPaciente(id);

            if (turno == null) return false;

            if (turno.TokenConfirmacion == token && turno.Estado == "PendienteConfirmacion")
            {
                turno.Estado = "Pendiente";
                turno.TokenConfirmacion = null;

                await _turnoRepository.Actualizar(turno);

                
                string timeZoneId = "Argentina Standard Time";
                TimeZoneInfo zonaHoraria;
                try
                {
                    zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
                }

               
                var fechaUtc = DateTime.SpecifyKind(turno.FechaHora, DateTimeKind.Utc);
                var fechaLocal = TimeZoneInfo.ConvertTimeFromUtc(fechaUtc, zonaHoraria);
             
                try
                {
                    await _notificacionService.CrearNotificacionAsync(
                        usuarioDestinoId: 2,
                        titulo: "Turno Confirmado",
                      
                        mensaje: $"El paciente {turno.Paciente.Nombre} {turno.Paciente.Apellido} ha confirmado su turno del {fechaLocal:dd/MM} a las {fechaLocal:HH:mm} hs.",
                        referenciaId: turno.Id
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notificación: {ex.Message}");
                }

                return true;
            }

            return false;
        }

    }
}

