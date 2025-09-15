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
        public Task<Sesion> ActualizarSesionAsync(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public Task<Sesion> CrearSesionAsync(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarSesionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Sesion> GetSesionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sesion>> GetSesionesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
