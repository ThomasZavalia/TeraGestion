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
        private readonly TeraDbContext _context;

        public UsuarioRepository(TeraDbContext context)
        {
            _context = context;
        }
        public Task<Usuario> Actualizar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> Agregar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

    }  
 
}
 