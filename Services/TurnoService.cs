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
    public class TurnoService : ITurnoService


    {

        private readonly ITurnoRepository _turnoRepository;
        public TurnoService(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }


        public async Task<Turno> ActualizarTurnoAsync(Turno turno)
        {
            throw new NotImplementedException();
        }

      
        public async Task<Turno> CrearTurnoAsync(Turno turno)
        {
          
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarTurnoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Turno> GetTurnoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Turno>> GetTurnosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
