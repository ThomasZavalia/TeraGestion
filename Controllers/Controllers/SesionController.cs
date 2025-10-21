using Core.DTOs.Sesion;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {

        private readonly ISesionService _sesionService;
        public SesionController(ISesionService sesionService)
        {
            _sesionService = sesionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSesion(int id)
        {
           
                var sesion = await _sesionService.GetSesionByIdAsync(id);
                return Ok(sesion);
          
        }



        [HttpGet]
        public async Task<IActionResult> GetSesiones()
        {
            
                var sesiones = await _sesionService.GetSesionesAsync();
                return Ok(sesiones);
           
        }



        [HttpPost]
        public async Task<IActionResult> CrearSesion([FromBody] SesionDTO sesion)
        {
           
                var nuevaSesion = await _sesionService.CrearSesionAsync(sesion);

                return Ok(nuevaSesion);
            
           
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSesion(int id, [FromBody] SesionDTO sesionDTO)
        {

            var sesionActualizada = await _sesionService.ActualizarSesionAsync(id, sesionDTO);

           
            return Ok(sesionActualizada);
        }
    
        



        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarSesion(int id)
        {
            
                var resultado = await _sesionService.EliminarSesionAsync(id);
            return NoContent();
        }

    }
}
