using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {

        private readonly Core.Interfaces.IPagoService _pagoService;
        public PagoController(Core.Interfaces.IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPago(int id)
        {
            try
            {
                var pagoEncontrado = await _pagoService.GetPago(id);

                return Ok(pagoEncontrado);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            try
            {
                var pagos = await _pagoService.GetPagos();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] Core.DTOs.CrearPagoDTO pago)
        {
            try
            {
                var nuevoPago = await _pagoService.CrearPago(pago);
                return CreatedAtAction(nameof(GetPago), new { id = nuevoPago.Id }, nuevoPago);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPago(int id, [FromBody] Core.Entidades.Pago pago)
        {
            throw new NotImplementedException();
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPago(int id)
        {
            try
            {
                var eliminado = await _pagoService.EliminarPago(id);
                if (eliminado)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"No se encontró un pago con ID {id}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
