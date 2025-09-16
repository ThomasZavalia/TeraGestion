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

        public  SesionRepository(TeraDbContext context)
        {
            _context = context;
        }

        public async Task<Sesion> Actualizar(Sesion entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Sesion> Agregar(Sesion entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Sesion>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sesion>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

    }
}
