using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class SesionRepository : ISesionRepository
    {
        private readonly TeraDbContext _context;

        public SesionRepository(TeraDbContext context)
        {
            _context = context;
        }

        public Task<Sesion> Actualizar(Sesion entity)
        {
            throw new NotImplementedException();
        }

        public Task<Sesion> Agregar(Sesion entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Sesion>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sesion> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
