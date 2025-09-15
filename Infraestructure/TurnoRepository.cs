using Core.Entidades;
using Core.Interfaces.Repositorios;
using SQLitePCL;
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

        public async Task<Turno> Actualizar(Turno turno)
        {
            var turnoExistente = _context.Turnos.Find(turno.Id);
            var turnoActualizado = _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();
            return turnoActualizado.Entity;
        }

        public async Task<Turno> Agregar(Turno turno)
        {
          await _context.Turnos.AddAsync(turno);
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<bool> Eliminar(int id)
        {
            var turnoExistente = _context.Turnos.Find(id);
             _context.Turnos.Remove(turnoExistente);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Turno>? GetById(int id)
        {
           var turnoExistente = _context.Turnos.Find(id);
            return turnoExistente;
        }

        public IEnumerable<Turno> ObtenerTodos()
        { 
            return _context.Turnos.ToList();

        }
    }
}
