using AutoMapper;
using Core.DTOs.Usuario.Input;
using Core.DTOs.Usuario.Output;
using Core.Entidades;
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
        
        private readonly PasswordHasher<Usuario> _passwordHasher = new();
        public UsuarioService(IUsuariosRepository usuarioRepository,IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
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
            if(!resultado)throw new KeyNotFoundException("Usuario no encontrado");
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
            if (usuario == null) { throw new KeyNotFoundException("Usuario no encontrado"); }
            var resultado = _passwordHasher.VerifyHashedPassword(null, usuario.PasswordHash, password);
            if (resultado == PasswordVerificationResult.Failed) { return null; }
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
    }
}
