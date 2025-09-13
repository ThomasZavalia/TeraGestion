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
        public Task<Usuario> ActualizarUsuario(Usuario usuario);
        public Task<bool> EliminarUsuario(int id);

    }
}
