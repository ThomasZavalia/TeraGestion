using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Core.Interfaces.IUsuarioService _usuarioService;
        public UsuarioController(Core.Interfaces.IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Core.Entidades.Usuario usuario)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Core.Entidades.Usuario usuario)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            throw new NotImplementedException();
        }
    }
}
