using Core.DTOs.Pago.Output;
using Core.Entidades;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {

        private readonly IPagoService _pagoService;
        public PagoController(IPagoService pagoService)
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
        public async Task<IActionResult> CrearPago([FromBody] PagoDto pago)
        {
           
                var nuevoPago = await _pagoService.CrearPago(pago);
                return Ok(nuevoPago);
          
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPago(int id, [FromBody] Core.Entidades.Pago pago)
        {
            throw new NotImplementedException();
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPago(int id)
        {
            
                var eliminado = await _pagoService.EliminarPago(id);
            return NoContent();
        }
    }
}
