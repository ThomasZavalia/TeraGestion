using Core.DTOs;
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
            var usuarios = await _usuarioService.GetUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.Username == loginDto.Username && u.PasswordHash == loginDto.Password);
            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            var token = GenerateJwtToken(usuario.Rol); // 🔹 Pasás el rol al generar el token
            return Ok(new { token });
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

