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

        public async Task<ObraSocial> Actualizar(ObraSocial entity)
        {
            var existente = await _context.ObrasSociales.FindAsync(entity.Id);
            if (existente == null) return null;

            existente.Nombre = entity.Nombre;
            existente.PrecioTurno = entity.PrecioTurno;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<ObraSocial> Agregar(ObraSocial entity)
        {
            await _context.ObrasSociales.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Eliminar(int id)
        {
            var existente = await _context.ObrasSociales.FindAsync(id);
            if (existente == null) return false;
            _context.ObrasSociales.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ObraSocial> GetById(int id)
        {
            return await _context.ObrasSociales
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<ObraSocial>> ObtenerTodos()
        {
            return await _context.ObrasSociales.AsNoTracking().ToListAsync();
        }
    }
}

