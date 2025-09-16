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
        public async Task<Usuario> Actualizar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> Agregar(Usuario entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Usuario>? GetById(int id)

        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Usuario>> ObtenerTodos()

        {
            throw new NotImplementedException();
        }

    }  
 
}
 