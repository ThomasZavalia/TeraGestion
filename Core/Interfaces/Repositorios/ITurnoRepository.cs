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
        Task<bool> ExisteTurnoPorPacienteYFecha(int pacienteId, DateTime fecha);

       Task<IEnumerable<Turno>> GetTurnosByDayAndTerapeutaAsync(DateTime fecha, int terapeutaId);

        Task<IEnumerable<Turno>> GetTurnosByTerapeutaAsync(int terapeutaId);

    }
}
