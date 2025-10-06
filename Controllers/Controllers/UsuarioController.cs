using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Core.Interfaces.IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        public UsuarioController(Core.Interfaces.IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
          
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
          var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null) { return NotFound(); }
            return Ok(usuario);
        }
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
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Core.Entidades.Usuario usuario)
        {
            if (id != usuario.Id) { return BadRequest("El ID del usuario no coincide"); }
            var usuarioActualizado = await _usuarioService.ActualizarUsuario(usuario);
            if (usuarioActualizado == null) { return NotFound(); }
            return Ok(usuarioActualizado);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {

            var resultado = await _usuarioService.EliminarUsuario(id);
            if (!resultado) { return NotFound(); }
            return NoContent();

        }



    }
}
