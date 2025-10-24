using Core.Entidades;
using Core.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
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
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<Turno> Agregar(Turno turno)
        {
          await _context.Turnos.AddAsync(turno);
           
           
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task<bool> Eliminar(int id)
        {
            var turnoExistente = await _context.Turnos.FindAsync(id);
            if (turnoExistente == null)
            {
                return false;
            }
                _context.Turnos.Remove(turnoExistente);
            await _context.SaveChangesAsync();
            return true;
            
        }

        

        public async Task<Turno?> GetById(int id)
        {
           var turnoExistente = await _context.Turnos.FindAsync(id);
            if (turnoExistente == null)
            {
                return null;
        }
            return turnoExistente;
        }

        public async Task<IEnumerable<Turno>> ObtenerTodos()
        {
            return await _context.Turnos
                           .Include(t => t.Paciente)
                           .ToListAsync();

        }

        public async Task<Turno?> GetByIdConPaciente(int id)
        {
          
            return await _context.Turnos
                                 .Include(t => t.Paciente) 
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

       
        public async Task<IEnumerable<Turno>> GetTurnosByDayAsync(DateTime date)
        {
            
            return await _context.Turnos
                                 .Where(t => t.FechaHora.Date == date.Date)
                                 .ToListAsync();
        }


    }
 
 } 