using Core.DTOs;
using Core.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        public AuthController(UsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioService.ValidarCredenciales(loginDto.Username, loginDto.Password);
            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            var token = GenerateJwtToken(usuario.Rol);
            return Ok(new { token });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var existingUser = await _usuarioService.GetByName(registerDto.Username);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario ya existe");
            }

            var nuevoUsuario = new Usuario
            {
                Username = registerDto.Username,
                PasswordHash = registerDto.Password,
                Rol = "User" // Asignás un rol por defecto
            };

            var creadoUsuario = await _usuarioService.CrearUsuario(nuevoUsuario);
            if (creadoUsuario == null)
            {
                return StatusCode(500, "Error al crear el usuario");
            }

            return Ok("Usuario registrado exitosamente");
        }


        private string GenerateJwtToken(string rol)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "usuario_demo"),
            new Claim(ClaimTypes.Role, rol)  // 🔹 Guardás el rol
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

