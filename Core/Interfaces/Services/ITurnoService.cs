using Core.DTOs.Public;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ITurnoService
    {
        public Task<TurnoDto> GetTurnoAsync(int id);
        public Task<IEnumerable<TurnoCalendarioDto>> GetTurnosAsync();
        public Task<TurnoCalendarioDto> CrearTurnoAsync(TurnoDtoCreacion turnoDto);
        public Task<TurnoCalendarioDto> ActualizarTurnoAsync(int id, TurnoDtoActualizar turno);
        public Task<bool> EliminarTurnoAsync(int id);
        public Task<IEnumerable<Turno>> GetTurnosSinDto();

        public Task MarcarComoPagadoAsync(int turnoId, string metodo);

        Task<IEnumerable<string>> GetAvailableSlotsAsync(DateTime date);

        Task<IEnumerable<TurnoCalendarioDto>> GetTurnosDelDiaAsync(DateTime date);
        Task<TurnoDetalleDto> GetTurnoDetalleAsync(int id);

        Task<TurnoCalendarioDto> ReprogramarTurnoAsync(int id, DateTime nuevaFecha);
        Task<TurnoCalendarioDto> ReservarTurnoPublicoAsync(ReservaDto dto);


    }
}
