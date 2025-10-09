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
       var turnoExistente= await _context.Turnos.FindAsync(turno.Id);
            turnoExistente.FechaHora = turno.FechaHora;
            turnoExistente.Estado = turno.Estado;
            turnoExistente.Precio = turno.Precio;
            turnoExistente.PacienteId = turno.PacienteId;
            turnoExistente.ObraSocialId = turno.ObraSocialId;
            await _context.SaveChangesAsync();
            return turnoExistente;
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
          return await _context.Turnos.AsNoTracking().ToListAsync();

        }

       
    }
 
 } 