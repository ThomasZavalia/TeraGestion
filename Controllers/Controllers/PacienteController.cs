using Core.DTOs.Paciente;
using Core.Entidades;
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
              var paciente = await _pacienteService.GetPacienteAsync(id);
              if (paciente == null)
                 return NotFound();
              return Ok(paciente);
        }

        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
              var pacientes = await _pacienteService.GetPacientesAsync();
              return Ok(pacientes);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] PacienteDTO pacienteDto)
        {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var creado = await _pacienteService.CrearPacienteAsync(pacienteDto);
                if (creado == null)
                    return BadRequest("No se pudo crear el paciente.");
               return Ok(creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, [FromBody] PacienteDTO paciente)
        {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var existente = await _pacienteService.GetPacienteAsync(id);
                if (existente == null)
                    return NotFound();
                paciente.Id = id;
                var actualizado = await _pacienteService.ActualizarPacienteAsync(paciente);
                return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
              var eliminado = await _pacienteService.EliminarPacienteAsync(id);
              if (!eliminado)
                 return NotFound();
              return NoContent();
        }


    }
}

