using Core.DTOs.Paciente;
using Core.Entidades;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
             
                var actualizado = await _pacienteService.ActualizarPacienteAsync(id,paciente);
                return Ok(actualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
              var eliminado = await _pacienteService.EliminarPacienteAsync(id);
             
              return NoContent();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<PacienteSimpleDto>>> BuscarPacientes([FromQuery] string query)
        {
            var pacientes = await _pacienteService.BuscarPacientesAsync(query);
            return Ok(pacientes);
        }
        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<PacienteDetalleDTO>> GetPacienteDetallesAsync(int id)
        {
            try
            {
                var pacienteDetalle = await _pacienteService.GetPacienteDetallesAsync(id);

                return Ok(pacienteDetalle);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los detalles del paciente. {ex.Message}");
            }
        }

        [HttpGet("check-dni")]
        public async Task<IActionResult> CheckDni([FromQuery] string dni)
        {
            if (string.IsNullOrEmpty(dni))
            {
                return BadRequest(new { message = "DNI no provisto." });
            }
            var exists = await _pacienteService.CheckDniExistsAsync(dni);

            // Devolver un objeto es más claro para el frontend
            return Ok(new { exists = exists });
        }
    }
}

