using Core.DTOs.Disponiblidad.Input;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers.Controllers
{
    [Route("api/usuario/me/disponibilidad")] 
    [ApiController]
    [Authorize] // Requiere login para acceder
    public class DisponibilidadController : ControllerBase
    {
        private readonly IDisponibilidadService _disponibilidadService;

        public DisponibilidadController(IDisponibilidadService disponibilidadService)
        {
            _disponibilidadService = disponibilidadService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetMiDisponibilidad()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Token inválido.");
            }

            var disponibilidad = await _disponibilidadService.GetDisponibilidadAsync(userId);
            return Ok(disponibilidad); 
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateMiDisponibilidad([FromBody] DisponibilidadUpdateDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Token inválido.");
            }

          
            await _disponibilidadService.UpdateDisponibilidadAsync(userId, dto);

            return Ok(new { message = "Disponibilidad actualizada correctamente." });
          
        }
    }
}
