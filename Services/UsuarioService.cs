using AutoMapper;
using Core.DTOs.Usuario.Input;
using Core.DTOs.Usuario.Output;
using Core.Entidades;
using Core.Interfaces.Email;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuariosRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        public UsuarioService(IUsuariosRepository usuarioRepository, IMapper mapper, IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<UsuarioDto> ActualizarUsuario(int id, UsuarioActualizarDto dto)
        {
            var usuarioExistente = await _usuarioRepository.GetById(id);
            if (usuarioExistente == null) throw new KeyNotFoundException("Usuario no encontrado");


            _mapper.Map(dto, usuarioExistente);

            var usuarioActualizado = await _usuarioRepository.Actualizar(usuarioExistente);

            return _mapper.Map<UsuarioDto>(usuarioActualizado);
        }




        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {

            usuario.PasswordHash = _passwordHasher.HashPassword(null, usuario.PasswordHash);
            var nuevoUsuario = await _usuarioRepository.Agregar(usuario);
            return nuevoUsuario;


        }

        public async Task<bool> EliminarUsuario(int id)
        {
            var resultado = await _usuarioRepository.Eliminar(id);
            if (!resultado) throw new KeyNotFoundException("Usuario no encontrado");
            return resultado;
        }

        public async Task<UsuarioDto> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepository.GetById(id);
            if (usuario == null) { throw new KeyNotFoundException("Usuario no encontrado"); }

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<Usuario> GetByName(string username)
        {
            var usuarios = await _usuarioRepository.ObtenerTodos();
            var usuario = usuarios.FirstOrDefault(u => u.Username == username);
            if (usuario == null) { return null; }
            return usuario;
        }

        public async Task<IEnumerable<UsuarioDto>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.ObtenerTodos();

            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }


        public async Task<Usuario> ValidarCredenciales(string username, string password)
        {
            var usuario = await GetByName(username);

            if (usuario == null)
            {
                return null;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(null, usuario.PasswordHash, password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return usuario;
        }



        public async Task<UsuarioDto> ActualizarPerfilUsuario(int id, UsuarioPerfilDto dto)
        {
            var usuarioExistente = await _usuarioRepository.GetById(id);
            if (usuarioExistente == null) throw new KeyNotFoundException("Usuario no encontrado");


            var otroUsuario = await GetByName(dto.Username);
            if (otroUsuario != null && otroUsuario.Id != id) throw new ArgumentException("El nombre de usuario ya está en uso.");



            _mapper.Map(dto, usuarioExistente);

            var usuarioActualizado = await _usuarioRepository.Actualizar(usuarioExistente);
            return _mapper.Map<UsuarioDto>(usuarioActualizado);
        }

        public async Task<bool> CambiarContraseña(int id, string contraseñaActual, string contraseñaNueva)
        {
            var usuario = await _usuarioRepository.GetById(id);
            if (usuario == null) { throw new KeyNotFoundException("Usuario no encontrado"); }


            var resultadoVerificacion = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, contraseñaActual);
            if (resultadoVerificacion == PasswordVerificationResult.Failed)
            {
                return false;
            }


            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, contraseñaNueva);


            await _usuarioRepository.Actualizar(usuario);
            return true;
        }


        public async Task<bool> SolicitarRecuperacionClave(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null)
            {
                return true;
            }

            var token = Guid.NewGuid().ToString();
            usuario.ResetToken = token;
            usuario.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            await _usuarioRepository.Actualizar(usuario);

            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            var cuerpoMail = $@"
        <h1>Recuperación de Contraseña</h1>
        <p>Hiciste una solicitud para restablecer tu contraseña en TeraGestión.</p>
        <p>Hacé clic en el siguiente enlace para continuar:</p>
        <a href='{resetLink}'>Restablecer Contraseña</a>
        <p>Este enlace vence en 1 hora.</p>";

            await _emailService.SendEmailAsync(usuario.Email, "Recuperación de Contraseña", cuerpoMail);

            return true;
        }

        public async Task<bool> RestablecerClave(string token, string nuevaClave)
        {
            var usuario = await _usuarioRepository.GetByResetTokenAsync(token);

            if (usuario == null || usuario.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false;
            }

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, nuevaClave);

            usuario.ResetToken = null;
            usuario.ResetTokenExpiry = null;

            await _usuarioRepository.Actualizar(usuario);
            return true;
        }

        public async Task<IEnumerable<TerapeutaListaDto>> GetTerapeutasDisponibles()
        {
          var terapeutas = await _usuarioRepository.GetTerapeutasDisponibles();
            return terapeutas.Select(t => new TerapeutaListaDto
            {
                Id = t.Id,
              
                NombreCompleto = string.IsNullOrEmpty(t.Nombre) ? t.Username : $"{t.Nombre} {t.Apellido}"
            });
        }

        
    } 
}
