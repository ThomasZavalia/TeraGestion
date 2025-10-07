using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {

        private readonly Core.Interfaces.ISesionService _sesionService;
        public SesionController(Core.Interfaces.ISesionService sesionService)
        {
            _sesionService = sesionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSesion(int id)
        {
            try
            {
                var sesion = await _sesionService.GetSesionByIdAsync(id);
                return Ok(sesion);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetSesiones()
        {
            try
            {
                var sesiones = await _sesionService.GetSesionesAsync();
                return Ok(sesiones);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> CrearSesion([FromBody] Core.DTOs.CrearSesionDTO sesion)
        {
            try
            {
                var nuevaSesion = await _sesionService.CrearSesionAsync(sesion);
                
                return Ok(nuevaSesion);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSesion(int id, [FromBody] Core.DTOs.SesionDTO sesionDTO)
        {
            try
            {
                var sesionActualizada = await _sesionService.ActualizarSesionAsync(id, sesionDTO);
                return Ok(sesionActualizada);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarSesion(int id)
        {
            try
            {
                var resultado = await _sesionService.EliminarSesionAsync(id);
                return Ok(new { mensaje = "Sesion eliminada exitosamente", exito = resultado });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
