using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
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
        public UsuarioService(IUsuariosRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Usuario> ActualizarUsuario(Usuario usuario)
        {
            var usuarioExistente = await _usuarioRepository.GetById(usuario.Id);
            if (usuarioExistente == null) { return null; }
            usuarioExistente.Username = usuario.Username;
            usuarioExistente.Rol = usuario.Rol;
            var usuarioActualizado = await _usuarioRepository.Actualizar(usuarioExistente);
            return usuarioActualizado;
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
          if (usuario == null) { return null; }
            var nuevoUsuario = await _usuarioRepository.Agregar(usuario);
            return nuevoUsuario;


        }

        public async Task<bool> EliminarUsuario(int id)
        {
          var resultado = await _usuarioRepository.Eliminar(id);
            return resultado;
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
           var usuario = await _usuarioRepository.GetById(id);
            if (usuario == null) { return null; }
            return usuario;
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.ObtenerTodos();
            return usuarios;
        }
    }
}
