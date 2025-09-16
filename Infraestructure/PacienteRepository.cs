using Core.Entidades;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public async Task<Paciente> Agregar(Paciente entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Paciente>? GetById(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<IEnumerable<Paciente>> ObtenerTodos()

        {
            throw new NotImplementedException();
        }
    }
}
