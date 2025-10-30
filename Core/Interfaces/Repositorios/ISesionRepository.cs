using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositorios
{
    public interface ISesionRepository : IRepository<Entidades.Sesion>
    {
        Task<Sesion?> GetByTurnoIdAsync(int turnoId);
    }
}
