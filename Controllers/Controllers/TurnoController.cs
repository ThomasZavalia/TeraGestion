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
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<IActionResult> GetTurnos()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> CrearTurno([FromBody] Core.Entidades.Turno turno)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTurno(int id, [FromBody] Core.Entidades.Turno turno)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTurno(int id)
        {
            throw new NotImplementedException();
        }
    }
}
