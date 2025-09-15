using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class PagoRepository : IPagosRepository
    {

        private readonly TeraDbContext _context;
        public PagoRepository( TeraDbContext context) 
        {
            _context = context;    
        }
        public Task<Pago> Actualizar(Pago entity)
        {
            throw new NotImplementedException();
        }

        public Task<Pago> Agregar(Pago entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pago>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pago>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
