using Core.DTOs;
using Core.DTOs.Pago.Input;
using Core.DTOs.Turno;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TurnoController : ControllerBase
    {

        private readonly ITurnoService _turnoService;
        public TurnoController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTurno(int id)
        {
           
           var turno = await _turnoService.GetTurnoAsync(id);
           
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
            return Ok(turnoCreado);
            }
            
           
        

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTurno(int id, [FromBody]TurnoDtoActualizar turnoDto)
        {
           
            
           var turnoActualizado = await _turnoService.ActualizarTurnoAsync(id, turnoDto);
           
            
            return Ok(turnoActualizado);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTurno(int id)
        {
           var turno = await _turnoService.EliminarTurnoAsync(id);
           
            return NoContent();
        }

        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> MarcarComoPagado(int id, [FromBody] PagoRequestDto request)
        {
            await _turnoService.MarcarComoPagadoAsync(id, request.MetodoPago);
                return Ok("Turno marcado como pagado correctamente");
            
            
        }

        [HttpGet("disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] DateTime fecha)
        {
            var slots = await _turnoService.GetAvailableSlotsAsync(fecha);
            return Ok(slots);
        }

        [HttpGet("hoy")] 
        public async Task<IActionResult> GetTurnosDeHoy()
        {

            var hoyUtc = DateTime.UtcNow;
            var turnos = await _turnoService.GetTurnosDelDiaAsync(hoyUtc);
            return Ok(turnos); 
        }

        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> GetTurnoDetalle(int id)
        {
        
            var turno = await _turnoService.GetTurnoDetalleAsync(id);
            return Ok(turno);
        }
    }
}
