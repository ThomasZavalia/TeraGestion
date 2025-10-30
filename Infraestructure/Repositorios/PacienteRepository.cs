
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
    public class PacienteRepository : IPacienteRepository
    {
        private readonly TeraDbContext _context;

        public PacienteRepository(TeraDbContext context)
        {
            _context = context;
        }
     


       
            public async Task<Paciente> Actualizar(int id, Paciente entityConNuevosDatos)
{
    var pacienteEncontrado = await _context.Pacientes.FindAsync(id);
    
    if (pacienteEncontrado == null)
    { 
        return null; // El servicio manejará este null
    }
    
    // 2. Copia manualmente los valores
    pacienteEncontrado.Nombre = entityConNuevosDatos.Nombre;
    pacienteEncontrado.Apellido = entityConNuevosDatos.Apellido;
    pacienteEncontrado.FechaNacimiento = entityConNuevosDatos.FechaNacimiento.Date;
    pacienteEncontrado.ObraSocialId = entityConNuevosDatos.ObraSocialId;
    pacienteEncontrado.Telefono = entityConNuevosDatos.Telefono;
    pacienteEncontrado.Email = entityConNuevosDatos.Email;
    pacienteEncontrado.DNI = entityConNuevosDatos.DNI;

   
    _context.Entry(pacienteEncontrado).State = EntityState.Modified;

    // 4. EF ahora SÍ ve el estado 'Modified' y guarda.
    await _context.SaveChangesAsync();
    return pacienteEncontrado;
}
    
        public async Task<Paciente> Actualizar(Paciente entity)
        {
         
            return await Actualizar(entity.Id, entity);

            

        }
    
        

        public async Task<Paciente> Agregar(Paciente entity)
        {
            await _context.Pacientes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Eliminar(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return false;
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Paciente>? GetById(int id)
        {
            return await _context.Pacientes.FindAsync(id);
        }



        public async Task<IEnumerable<Paciente>> ObtenerTodos()

        {

            return await _context.Pacientes
                                 .Include(p => p.ObraSocial)
                                 .ToListAsync();

        }


        public async Task<IEnumerable<Paciente>> BuscarAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<Paciente>();
            }

            var normalizedQuery = query.ToLower().Trim();

            return await _context.Pacientes
                .Where(p =>
                    p.Nombre.ToLower().Contains(normalizedQuery) ||
                    p.Apellido.ToLower().Contains(normalizedQuery) ||
                    p.DNI.Contains(normalizedQuery)
                )
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Take(10) // Limitamos a 10 resultados para el dropdown
                .ToListAsync();
        }



        public async Task<Paciente> GetDetallesByIdAsync(int id)
        {
            var paciente = await _context.Pacientes.Include(p => p.ObraSocial)
                                                   .Include(p => p.Sesiones)
                                                   .ThenInclude(s => s.Turno)
                                                   .ThenInclude(t => t.Pagos)
                                                   .AsNoTracking()
                                                   .FirstOrDefaultAsync(p => p.Id == id);

            return paciente;
        }




    }
}
