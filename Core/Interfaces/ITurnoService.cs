using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITurnoService
    {
        public Task<TurnoDto> GetTurnoAsync(int id);
        public Task<IEnumerable<TurnoDto>> GetTurnosAsync();
        public Task<TurnoDto> CrearTurnoAsync(TurnoDtoCreacion turnoDto);
        public Task<TurnoDto> ActualizarTurnoAsync(TurnoDto turno);
        public Task<bool> EliminarTurnoAsync(int id);

        public Task MarcarComoPagadoAsync(int turnoId, string metodo);

    }
}
