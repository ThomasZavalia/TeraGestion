using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaciente(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] Core.Entidades.Paciente paciente)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] Core.Entidades.Paciente paciente)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            throw new NotImplementedException();
        }


    }
}
