using Core.DTOs;
using Core.DTOs.Usuario.Input;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUsuarioService
    {
        public Task<Usuario> GetUsuarioById(int id);
        public Task<IEnumerable<Usuario>> GetUsuarios();
        public Task<Usuario> CrearUsuario(Usuario usuario);
        public Task<Usuario> ActualizarUsuario(int id,UsuarioActualizarDto usuario);
        public Task<bool> EliminarUsuario(int id);
        public Task<Usuario> GetByName(string username);

        public Task<Usuario> ValidarCredenciales(string username, string password);

    }
}
