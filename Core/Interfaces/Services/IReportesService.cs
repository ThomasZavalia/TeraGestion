using Core.DTOs.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IReportesService
    {
        public Task<IEnumerable<ReporteTopPacienteDto>> GetTopPacientes();
        public Task<IEnumerable<ReporteMetodoPagoDto>> GetMetodosPagoDto();
        public Task<IEnumerable<ReporteEstadoDto>> GetTurnoPorEstado();
        public Task<IEnumerable<ReporteMesDto>> GetIngresosPorMes(DateTime? fechaDesde = null, DateTime? fechaHasta = null);
        public Task<IEnumerable<ReporteMesDto>> GetTurnosPorMes(DateTime? fechaDesde = null, DateTime? fechaHasta = null);
        Task<IEnumerable<ReporteEstadoDto>> GetTurnosPorObraSocial(); 
                                                                      


    }
}
