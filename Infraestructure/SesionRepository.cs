using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            sesionEncontada.Asistencia = sesion.Asistencia;
            sesionEncontada.Notas = sesion.Notas;
            
            _context.Sesiones.Update(sesionEncontada);
            await _context.SaveChangesAsync();

            return sesionEncontada;
        }



        public async Task<Sesion> Agregar(Sesion sesion)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }




        public async Task<Sesion>? GetById(int id)
        {
            return await _context.Sesiones.FindAsync(id);
        }





        public async Task<IEnumerable<Sesion>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
