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
            throw new NotImplementedException();
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
          throw new NotImplementedException();


        }

        public async Task<bool> EliminarUsuario(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            throw new NotImplementedException();
        }
    }
}
