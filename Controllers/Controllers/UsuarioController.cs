using Core.DTOs.Usuario.Input;
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        public UsuarioController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
          
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
          var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null) { return NotFound(); }
            return Ok(usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {

            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(usuarios);
        }
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Core.Entidades.Usuario usuario)
        {

            var nuevoUsuario = await _usuarioService.CrearUsuario(usuario);
            if (nuevoUsuario == null) { return BadRequest("No se pudo crear el usuario"); }
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioActualizarDto dto)
        {
            var usuarioActualizado = await _usuarioService.ActualizarUsuario(id, dto);
            if (usuarioActualizado == null) return NotFound();

            return Ok(usuarioActualizado); // Devuelve solo los campos del usuario (sin PasswordHash)
        }

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {

            var resultado = await _usuarioService.EliminarUsuario(id);
            if (!resultado) { return NotFound(); }
            return NoContent();

        }



    }
}
