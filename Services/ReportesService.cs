using Core.DTOs.Reportes;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportesService : IReportesService
    {
        private readonly IPacienteService _pacienteService;
        private readonly IPagoService _pagoService;
        private readonly ITurnoService _turnoService;


        public ReportesService(IPacienteService pacienteService, ITurnoService turnoService, IPagoService pagoService)

        {
            _pacienteService = pacienteService;
            _turnoService = turnoService;
            _pagoService = pagoService;

        }

        public async Task<IEnumerable<ReporteMesDto>> GetTurnosPorMes()
        {
            var turnos = await _turnoService.GetTurnosSinDto();
            var query = turnos.GroupBy(t => t.FechaHora.Month)
                .Select(g => new ReporteMesDto
                {
                    Mes = new DateTime(1, g.Key, 1).ToString("MMMM"),
                    Valor = g.Count()
                })
                .ToList();

            return query;
        }

        public async Task<IEnumerable<ReporteMesDto>> GetIngresosPorMes()
        {
            var pagos = await _pagoService.GetPagosSinDto();
            var query = pagos.Where(p => p.Turno != null && p.Turno.Estado.ToLower() == "pagado")
                .GroupBy(p => p.Fecha.Month)
                .Select(g => new ReporteMesDto
                {
                    Mes = new DateTime(1, g.Key, 1).ToString("MMMM"),
                    Valor = (int)g.Sum(p => p.Monto)

                }).ToList();
            return query;
        }

        public async Task<IEnumerable<ReporteEstadoDto>> GetTurnoPorEstado()
        {
            var turnos = await _turnoService.GetTurnosSinDto();

            var query = turnos.GroupBy(t => t.Estado)
                .Select(g => new ReporteEstadoDto
                {
                    Estado = g.Key,
                    Cantidad = g.Count()

                }).ToList();
            return query;


        }

        public async Task<IEnumerable<ReporteMetodoPagoDto>> GetMetodosPagoDto()
        {
            var pagos = await _pagoService.GetPagosSinDto();
            var query = pagos.GroupBy(p => p.MetodoPago)
                .Select(g => new ReporteMetodoPagoDto
                {
                    MetodoPago = g.Key,
                    Cantidad = g.Count()
                }).ToList();
            return query;
        }

        public async Task<IEnumerable<ReporteTopPacienteDto>> GetTopPacientes()
        {

            var turnos = await _turnoService.GetTurnosSinDto();
            var query = turnos.Where(t => t.Paciente != null)
                .GroupBy(t => t.Paciente.Nombre)
                .Select(g => new ReporteTopPacienteDto
                {
                    Paciente=g.Key,
                    Turnos = g.Count()

                }).OrderByDescending(x=>x.Turnos).Take(5).ToList();
            return query;
        }






    }


}

