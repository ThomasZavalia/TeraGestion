using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositorios
{
    public interface ITurnoRepository : IRepository<Entidades.Turno>
    {
        Task<Turno?> GetByIdConPaciente(int id);
       
            Task<IEnumerable<Turno>> GetTurnosByDayAsync(DateTime date);
        
    }
}
