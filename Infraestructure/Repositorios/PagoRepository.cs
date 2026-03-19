using Core.Entidades;
using Core.Interfaces.Repositorios;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorios
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
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
            return pago;
        }

        public async Task<Pago> Agregar(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return pago;
        }

        public async Task<bool> Eliminar(int id)
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

        public async Task<IEnumerable<Pago>> ObtenerTodos()
        {
            return await _context.Pagos
                          .Include(p => p.Turno)
                          .ThenInclude(t => t.Terapeuta)
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

        public async Task<(IEnumerable<Pago> pagos, int total)> GetPagosPaginadosYFiltradosAsync(
    int pagina, int tamanio, string? busqueda, DateTime? fechaDesde, DateTime? fechaHasta, string? metodoPago)
        {
            var query = _context.Pagos
                .Include(p => p.Turno)
                    .ThenInclude(t => t.Paciente)
                .AsQueryable();

            query = query.Where(p => p.Anulado != true);

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var b = busqueda.ToLower();
                query = query.Where(p => p.Turno.Paciente.Nombre.ToLower().Contains(b) ||
                                         p.Turno.Paciente.Apellido.ToLower().Contains(b) ||
                                         p.Turno.Paciente.DNI.Contains(b));
            }

            if (fechaDesde.HasValue)
            {
                var desdeUtc = DateTime.SpecifyKind(fechaDesde.Value, DateTimeKind.Utc);
                query = query.Where(p => p.Fecha >= desdeUtc);
            }

            if (fechaHasta.HasValue)
            {

                var hastaUtc = DateTime.SpecifyKind(fechaHasta.Value, DateTimeKind.Utc).AddDays(1).AddTicks(-1);
                query = query.Where(p => p.Fecha <= hastaUtc);
            }

            if (!string.IsNullOrWhiteSpace(metodoPago))
            {
                query = query.Where(p => p.MetodoPago == metodoPago);
            }
        
            int totalItems = await query.CountAsync();

            var pagosFiltrados = await query
                .OrderByDescending(p => p.Fecha)
                .Skip((pagina - 1) * tamanio)
                .Take(tamanio)
                .ToListAsync();

            return (pagosFiltrados, totalItems);
        }

    }
}
