using Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {

        private readonly Core.Interfaces.ITurnoService _turnoService;
        public TurnoController(Core.Interfaces.ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTurno(int id)
        {
           
           var turno = await _turnoService.GetTurnoAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            return Ok(turno);
        }
        [HttpGet]
        public async Task<IActionResult> GetTurnos()
        {
         var turnos = await _turnoService.GetTurnosAsync();
            return Ok(turnos);
        }
        [HttpPost]
        public async Task<IActionResult> CrearTurno([FromBody] TurnoDtoCreacion turno)
        {
         var turnoCreado = await _turnoService.CrearTurnoAsync(turno);
            if (turnoCreado == null)
            {
                return BadRequest("No se pudo crear el turno");
            }
            return CreatedAtAction(nameof(GetTurno), new { id = turnoCreado.Id }, turnoCreado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTurno(int id, [FromBody] Core.Entidades.Turno turno)
        {
            if (id != turno.Id)
            {
                return BadRequest("El ID del turno no coincide");
            }
            
           var turnoActualizado = await _turnoService.ActualizarTurnoAsync(turno);
            if (turnoActualizado == null)
            {
                return NotFound();
            }
            var turnoRespuesta =
new TurnoDtoRespuesta
{
   Id= turnoActualizado.Id,
    Fecha = turnoActualizado.FechaHora,
    Estado = turnoActualizado.Estado,
   PacienteNombre = turnoActualizado.Paciente.Nombre,
   Precio= turnoActualizado.Precio,



};   
            return Ok(turnoActualizado);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTurno(int id)
        {
           var turno = await _turnoService.EliminarTurnoAsync(id);
            if (!turno)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
