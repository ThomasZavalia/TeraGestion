using Core.Entidades;
using Core.Interfaces.Repositorios;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ObraSocialRepository : IObraSocialRepository
    {
        private readonly TeraDbContext _context;

        public ObraSocialRepository(TeraDbContext context)
        {
            _context = context;
        }

        public Task<ObraSocial> Actualizar(ObraSocial entity)
        {
            throw new NotImplementedException();
        }

        public Task<ObraSocial> Agregar(ObraSocial entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ObraSocial> GetById(int id)
        {
            return await _context.ObrasSociales
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

      

        public Task<IEnumerable<ObraSocial>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}

