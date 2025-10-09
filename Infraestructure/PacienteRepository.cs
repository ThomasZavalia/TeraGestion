
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
    public class PacienteRepository : IPacienteRepository
    {
        private readonly TeraDbContext _context;

        public PacienteRepository(TeraDbContext context)
        {
            _context = context;
        }
        public async Task<Paciente> Actualizar(Paciente entity)
        {
            var pacienteEncontrado = await _context.Pacientes.FindAsync(entity.Id);
            if (pacienteEncontrado == null)
            { return null; }
            pacienteEncontrado.Nombre = entity.Nombre;
            pacienteEncontrado.Apellido = entity.Apellido;
            pacienteEncontrado.FechaNacimiento = entity.FechaNacimiento.Date;
           pacienteEncontrado.ObraSocialId = entity.ObraSocialId;
            pacienteEncontrado.Telefono = entity.Telefono;
            pacienteEncontrado.Email = entity.Email;
            pacienteEncontrado.DNI = entity.DNI;

            await _context.SaveChangesAsync();
              return entity;
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
              return await _context.Pacientes.ToListAsync();
        }
    }
}
