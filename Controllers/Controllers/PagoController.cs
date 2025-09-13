using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<IActionResult> GetPagos()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] Core.Entidades.Pago pago)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPago(int id, [FromBody] Core.Entidades.Pago pago)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPago(int id)
        {
            throw new NotImplementedException();
        }
    }
}
