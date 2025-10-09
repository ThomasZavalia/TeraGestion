using Core.DTOs;
using Core.DTOs.Turno;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Microsoft.AspNetCore.Authorization;
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
            try { 
         var turnoCreado = await _turnoService.CrearTurnoAsync(turno);
            if (turnoCreado == null)
            {
                return BadRequest("No se pudo crear el turno");
            }
            return Ok(turnoCreado);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear el turno: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTurno(int id, [FromBody]TurnoDto turnoDto)
        {
            if (id != turnoDto.Id)
            {
                return BadRequest("El ID del turno no coincide");
            }
            
           var turnoActualizado = await _turnoService.ActualizarTurnoAsync(turnoDto);
            if (turnoActualizado == null)
            {
                return NotFound();
            }
            
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

        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> MarcarComoPagado(int id, [FromBody] string metodoPago)
        {
            try
            {
                await _turnoService.MarcarComoPagadoAsync(id, metodoPago);
                return Ok("Turno marcado como pagado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
