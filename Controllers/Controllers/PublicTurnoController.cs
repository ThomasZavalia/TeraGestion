using Core.DTOs.Public;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers.Controllers
{
    [Route("api/public/turnos")]
    [ApiController]
    public class PublicTurnosController : ControllerBase
    {
        private readonly ITurnoService _turnoService;
        private readonly IObraSocialService _obraSocialService;

        public PublicTurnosController(ITurnoService turnoService, IObraSocialService obraSocialService)
        {
            _turnoService = turnoService;
            _obraSocialService = obraSocialService;
        }


        [HttpGet("disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] DateTime fecha)
        {
           
            var slots = await _turnoService.GetAvailableSlotsAsync(fecha);
            return Ok(slots);
        }

        
        [HttpPost("reservar")]
        public async Task<IActionResult> Reservar([FromBody] ReservaDto dto)
        {
            try
            {
                var turno = await _turnoService.ReservarTurnoPublicoAsync(dto);
                return Ok(new { message = "Turno reservado con éxito", turnoId = turno.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message }); 
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { error = "Ocurrió un error al procesar la reserva." });
            }
        }

        [HttpGet("obras-sociales")]
        public async Task<IActionResult> GetObrasSocialesPublicas()
        {
            
            var obras = await _obraSocialService.GetObrasSocialesAsync();
            return Ok(obras);
        }
    }
}
