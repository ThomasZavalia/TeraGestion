using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class SesionRepository : ISesionRepository
    {
        private readonly TeraDbContext _context;

        public  SesionRepository(TeraDbContext context)
        {
            _context = context;
        }

        public async Task<Sesion> Actualizar(Sesion sesion)
        {
            var sesionEncontada = await _context.Sesiones.FindAsync(sesion.Id);

            if (sesionEncontada == null)
            {
                throw new ArgumentException("Sesion no encontrada");
            }
            sesionEncontada.PacienteId = sesion.PacienteId;
            sesionEncontada.TurnoId = sesion.TurnoId;
            sesionEncontada.FechaHoraInicio = sesion.FechaHoraInicio;
            sesionEncontada.Asistencia = sesion.Asistencia;
            sesionEncontada.Notas = sesion.Notas;
            
            await _context.SaveChangesAsync();

            return sesionEncontada;
        }



        public async Task<Sesion> Agregar(Sesion sesion)
        {

            _context.Sesiones.Add(sesion);
            await _context.SaveChangesAsync();

            
            return sesion;
        }



        public async Task<bool> Eliminar(int id)
        {
            var sesionEncontrada = await _context.Sesiones.FindAsync(id);

            if (sesionEncontrada == null)
            {
                throw new ArgumentException("Sesion no encontrada");
            }

            _context.Sesiones.Remove(sesionEncontrada);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Sesion>? GetById(int id)
        {
            return await _context.Sesiones.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sesion>> ObtenerTodos()
        {
            return await _context.Sesiones.ToListAsync();
        }

    }
}
