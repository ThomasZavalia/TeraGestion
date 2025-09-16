using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _repo;
        public SesionService(ISesionRepository repo)
        {
            _repo= repo;
        }
        public async Task<Sesion> ActualizarSesionAsync(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public async Task<Sesion> CrearSesionAsync(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarSesionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Sesion> GetSesionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sesion>> GetSesionesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
