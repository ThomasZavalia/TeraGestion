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
        public Task<Turno> Actualizar(Turno entity)
        {
            throw new NotImplementedException();
        }

        public Task<Turno> Agregar(Turno entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Turno> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Turno>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
 
 } 