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
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<IActionResult> GetSesiones()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> CrearSesion([FromBody] Core.Entidades.Sesion sesion)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSesion(int id, [FromBody] Core.Entidades.Sesion sesion)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarSesion(int id)
        {
            throw new NotImplementedException();
        }

    }
}
