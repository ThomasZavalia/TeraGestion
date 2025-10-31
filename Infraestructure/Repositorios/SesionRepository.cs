using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infraestructure;

namespace Infrastructure.Repositorios
{
    public class SesionRepository : ISesionRepository
    {
        private readonly TeraDbContext _context;

        public SesionRepository(TeraDbContext context)
        {
            _context = context;
        }

        public async Task<Sesion> Actualizar(Sesion sesion)
        {
            
            var sesionEncontrada = await _context.Sesiones.FindAsync(sesion.Id);
            if (sesionEncontrada == null)
            {
                
                throw new KeyNotFoundException($"No se encontró la sesión con ID {sesion.Id} para actualizar.");
            }

          
            sesionEncontrada.Asistencia = sesion.Asistencia;
            sesionEncontrada.Notas = sesion.Notas;
           

            await _context.SaveChangesAsync();
            return sesionEncontrada; 
        }



        public async Task<Sesion> Agregar(Sesion sesion)
        {

           await _context.Sesiones.AddAsync(sesion);
            await _context.SaveChangesAsync();


            return sesion;
        }



        public async Task<bool> Eliminar(int id)
        {
            var sesionEncontrada = await _context.Sesiones.FindAsync(id);
            if (sesionEncontrada == null)
            {
                return false; 
            }


            _context.Sesiones.Remove(sesionEncontrada);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Sesion>? GetById(int id)
        {
            var sesionExistente = await _context.Sesiones.FindAsync(id);
            if (sesionExistente == null)
            {
                return null;
            }
            return sesionExistente;
        }

        public async Task<IEnumerable<Sesion>> ObtenerTodos()
        {
            return await _context.Sesiones.ToListAsync();
        }

        public async Task<Sesion?> GetByTurnoIdAsync(int turnoId)
        {
            
            return await _context.Sesiones
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(s => s.TurnoId == turnoId);
        }

    }
}
