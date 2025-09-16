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
        public Task<Paciente> Actualizar(Paciente entity)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> Agregar(Paciente entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable <Paciente>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
