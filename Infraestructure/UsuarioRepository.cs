using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class UsuarioRepository : IUsuariosRepository
    {
        public void Actualizar(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Agregar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public Usuario? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
