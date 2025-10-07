using Core.DTOs;
using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
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
        public TurnoService(ITurnoRepository turnoRepository, IPacienteService pacienteService, IPagoService pagoService)
        {
            _turnoRepository = turnoRepository;
            _pacienteService = pacienteService;
            _pagoService = pagoService;

        }


        public async Task<Turno> ActualizarTurnoAsync(Turno turno)
        {
           var turnoExistente = await _turnoRepository.GetById(turno.Id);
            if (turnoExistente == null)
            {
                return null;
            }
            var turnoActualizado = await _turnoRepository.Actualizar(turno);
            return turnoActualizado;


        }

        public async Task MarcarComoPagadoAsync(int turnoId, string metodoPago)
        {
            var turno = await _turnoRepository.GetById(turnoId);
            if (turno == null) throw new Exception("Turno no encontrado");
            if (turno.Estado == "pagado") throw new Exception("El turno ya está pagado");

            var pago = new Pago
            {
                TurnoId = turnoId,
                MetodoPago = metodoPago,
                Fecha = DateTime.Now,
                Monto = turno.Precio 
            };

            await _pagoService.CrearPago(pago);

            turno.Estado = "pagado";
           

            await ActualizarTurnoAsync(turno);
        }


        public async Task<Turno> CrearTurnoAsync(TurnoDtoCreacion dto)
        {

            var pacienteAbuscar = await _pacienteService.GetPacientePorDniAsync(dto.DniPaciente);
            if (pacienteAbuscar == null)
            {
                var nuevoPaciente = new Paciente
                {
                    DNI = dto.DniPaciente,
                    Nombre = dto.NombrePaciente,
                    Apellido = dto.ApellidoPaciente
                };
                pacienteAbuscar = await _pacienteService.CrearPacienteAsync(nuevoPaciente);
            }
            var turno = new Turno
            {
                FechaHora = dto.Fecha,
                PacienteId = pacienteAbuscar.Id,
                Paciente = pacienteAbuscar,
                Estado = "Pendiente"
            };
            return await _turnoRepository.Agregar(turno);
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

        public async Task<Turno> GetTurnoAsync(int id)
        {
         var turnoAbuscar = await _turnoRepository.GetById(id);
            if (turnoAbuscar == null)
            {
                throw new Exception("No se encontro el turno");
            }
            return turnoAbuscar;
        }

        public async Task<IEnumerable<Turno>> GetTurnosAsync()
        {
           var turnos = await _turnoRepository.ObtenerTodos();
            return turnos;
        }
    }
}
