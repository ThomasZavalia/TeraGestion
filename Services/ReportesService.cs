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

        public async Task<IEnumerable<ReporteMesDto>> GetTurnosPorMes(DateTime? fechaDesde = null, DateTime? fechaHasta = null) 
        {
            var turnosQuery = (await _turnoService.GetTurnosSinDto()).AsQueryable(); 

           
            if (fechaDesde.HasValue) turnosQuery = turnosQuery.Where(t => t.FechaHora.Date >= fechaDesde.Value.Date);
            if (fechaHasta.HasValue) turnosQuery = turnosQuery.Where(t => t.FechaHora.Date <= fechaHasta.Value.Date);

            var query = turnosQuery
                .GroupBy(t => new { t.FechaHora.Year, t.FechaHora.Month })
                .Select(g => new ReporteMesDto
                {
                    
                    Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                    Valor = g.Count()
                })
                .OrderBy(r => r.Mes)
                .ToList();

            return query;
        }
        public async Task<IEnumerable<ReporteMesDto>> GetIngresosPorMes(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            
            var pagosQuery = (await _pagoService.GetPagosSinDto()).Where(p => p.Turno != null && p.Turno.Estado.ToLower() == "pagado").AsQueryable();

            if (fechaDesde.HasValue) pagosQuery = pagosQuery.Where(p => p.Fecha.Date >= fechaDesde.Value.Date);
            if (fechaHasta.HasValue) pagosQuery = pagosQuery.Where(p => p.Fecha.Date <= fechaHasta.Value.Date);

            var query = pagosQuery
                .GroupBy(p => new { p.Fecha.Year, p.Fecha.Month })
                .Select(g => new ReporteMesDto
                {
                    Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                   
                    Valor = g.Sum(p => p.Monto ?? 0) 
                })
                 .OrderBy(r => r.Mes)
                .ToList();


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
                .GroupBy(t => t.PacienteId) 
                .Select(g => new
                {
                    PacienteNombreCompleto = $"{g.First().Paciente.Nombre} {g.First().Paciente.Apellido}",
                    Turnos = g.Count()
                })
                .OrderByDescending(x => x.Turnos)
                .Take(5)
                .Select(g => new ReporteTopPacienteDto 
                {
                    Paciente = g.PacienteNombreCompleto,
                    Turnos = g.Turnos
                })
                .ToList();
            return query;
        }

        public async Task<IEnumerable<ReporteEstadoDto>> GetTurnosPorObraSocial()
        {
           
            var turnos = await _turnoService.GetTurnosSinDto();

            var query = turnos
               
                .Where(t => t.ObraSocialId != null && t.ObraSocial != null)
                .GroupBy(t => t.ObraSocial.Nombre)
                .Select(g => new ReporteEstadoDto
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .ToList();

            return query;
        }






    }


}

