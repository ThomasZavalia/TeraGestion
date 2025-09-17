using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly TeraDbContext _context;

        public TurnoRepository(TeraDbContext context)
        {
            _context = context;
        }
        public async Task<Turno> Actualizar(Turno entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Turno> Agregar(Turno entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Turno>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
