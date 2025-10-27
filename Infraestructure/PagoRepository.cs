using Core.Entidades;
using Core.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
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
        public PagoRepository(TeraDbContext context)
        {
            _context = context;
        }
        public async Task<Pago> Actualizar(Pago pago)
        {
            throw new NotImplementedException();
        }

        public async Task<Pago> Agregar(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return pago;
        }

        public async Task <bool> Eliminar(int id)
        {
            var pagoEncontrado = await _context.Pagos.FindAsync(id);
            if (pagoEncontrado == null)
            {
                return false;
            }
            else
            {
                _context.Pagos.Remove(pagoEncontrado);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Pago>? GetById(int id)
        {
            return await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task <IEnumerable<Pago>> ObtenerTodos()
        {
            return await _context.Pagos
                          .Include(p => p.Turno) 
                          .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> GetPagosFiltradosAsync(DateTime? fechaDesde, DateTime? fechaHasta, int? pacienteId)
        {
            
            IQueryable<Pago> query = _context.Pagos
                                             .Include(p => p.Turno)
                                                .ThenInclude(t => t.Paciente) 
                                             .AsNoTracking(); 

           
            if (fechaDesde.HasValue)
            {
              
                query = query.Where(p => p.Fecha.Date >= fechaDesde.Value.Date);
            }
            if (fechaHasta.HasValue)
            {
               
                query = query.Where(p => p.Fecha.Date <= fechaHasta.Value.Date);
            }
            if (pacienteId.HasValue)
            {
                query = query.Where(p => p.Turno.PacienteId == pacienteId.Value);
            }

            
            query = query.OrderByDescending(p => p.Fecha);

            return await query.ToListAsync();
        }
    }
}
