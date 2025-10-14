using Core.DTOs.Usuario.Input;
using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
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
        private readonly PasswordHasher<string> _passwordHasher = new();
        public UsuarioService(IUsuariosRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Usuario> ActualizarUsuario(int id,UsuarioActualizarDto dto)
        {
            var usuarioExistente = await _usuarioRepository.GetById(id);
            if (usuarioExistente == null) throw new KeyNotFoundException("Usuario no encontrado");
            usuarioExistente.Username = dto.Username;
            usuarioExistente.Email = dto.Email;
            usuarioExistente.Rol = dto.Rol;

            return await _usuarioRepository.Actualizar(usuarioExistente);
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
            if(!resultado)throw new KeyNotFoundException("Usuario no encontrado");
            return resultado;
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
           var usuario = await _usuarioRepository.GetById(id);
            if (usuario == null) { throw new KeyNotFoundException("Usuario no encontrado"); }
            return usuario;
        }

        public async Task<Usuario> GetByName(string username)
        {
            var usuarios = await _usuarioRepository.ObtenerTodos();
            var usuario = usuarios.FirstOrDefault(u => u.Username == username);
            if (usuario == null) { return null; }
            return usuario;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.ObtenerTodos();
            return usuarios;
        }

        public async Task<Usuario> ValidarCredenciales(string username, string password)
        {
            var usuario = await GetByName(username);
            if (usuario == null) { throw new KeyNotFoundException("Usuario no encontrado"); }
            var resultado = _passwordHasher.VerifyHashedPassword(null, usuario.PasswordHash, password);
            if (resultado == PasswordVerificationResult.Failed) { return null; }
            return usuario;
        }
    }
}
