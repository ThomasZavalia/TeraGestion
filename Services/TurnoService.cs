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
        public TurnoService(ITurnoRepository turnoRepository, IPacienteService pacienteService)
        {
            _turnoRepository = turnoRepository;
            _pacienteService = pacienteService;
        }


        public async Task<Turno> ActualizarTurnoAsync(Turno turno)
        {
           turno = await _turnoRepository.Actualizar(turno);
            if (turno == null)
            {
                throw new Exception("No se pudo actualizar el turno");
            }
            return turno;


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
                Paciente = pacienteAbuscar
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
