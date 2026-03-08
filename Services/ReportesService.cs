using ClosedXML.Excel;
using Core.DTOs.Reportes;
using Core.Interfaces.Repositorios;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly IUsuariosRepository _usuarioRepository;


        public ReportesService(IPacienteService pacienteService, ITurnoService turnoService, IPagoService pagoService, IUsuariosRepository usuariosRepository)

        {
            _pacienteService = pacienteService;
            _turnoService = turnoService;
            _pagoService = pagoService;
            _usuarioRepository = usuariosRepository;

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



        public async Task<byte[]> GenerarExcelReporteCompleto(DateTime? fechaDesde, DateTime? fechaHasta)
        {

            var ingresosPorMes = await GetIngresosPorMes(fechaDesde, fechaHasta);
            var turnosPorMes = await GetTurnosPorMes(fechaDesde, fechaHasta);
            var topPacientes = await GetTopPacientes();
            var estadosTurnos = await GetTurnoPorEstado();
            var metodosPago = await GetMetodosPagoDto();
            var obrasSociales = await GetTurnosPorObraSocial();


            using (var workbook = new XLWorkbook())
            {
                var hojaFinanzas = workbook.Worksheets.Add("Finanzas");


                hojaFinanzas.Cell(1, 1).Value = "REPORTE DE INGRESOS";
                hojaFinanzas.Range(1, 1, 1, 2).Merge().Style.Font.Bold = true;

                hojaFinanzas.Cell(3, 1).Value = "Mes";
                hojaFinanzas.Cell(3, 2).Value = "Total Facturado";
                hojaFinanzas.Range(3, 1, 3, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                hojaFinanzas.Range(3, 1, 3, 2).Style.Font.Bold = true;


                int fila = 4;
                foreach (var item in ingresosPorMes)
                {
                    hojaFinanzas.Cell(fila, 1).Value = item.Mes;
                    hojaFinanzas.Cell(fila, 2).Value = item.Valor;
                    hojaFinanzas.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0.00";
                    fila++;
                }
                hojaFinanzas.Columns().AdjustToContents();

                var hojaTurnos = workbook.Worksheets.Add("Turnos y Actividad");

                hojaTurnos.Cell(1, 1).Value = "TURNOS POR MES";
                hojaTurnos.Cell(1, 1).Style.Font.Bold = true;
                hojaTurnos.Cell(2, 1).Value = "Mes";
                hojaTurnos.Cell(2, 2).Value = "Cantidad";
                hojaTurnos.Range(2, 1, 2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                fila = 3;
                foreach (var item in turnosPorMes)
                {
                    hojaTurnos.Cell(fila, 1).Value = item.Mes;
                    hojaTurnos.Cell(fila, 2).Value = item.Valor;
                    fila++;
                }

                hojaTurnos.Cell(1, 4).Value = "ESTADOS DE TURNOS";
                hojaTurnos.Cell(1, 4).Style.Font.Bold = true;
                hojaTurnos.Cell(2, 4).Value = "Estado";
                hojaTurnos.Cell(2, 5).Value = "Cantidad";
                hojaTurnos.Range(2, 4, 2, 5).Style.Fill.BackgroundColor = XLColor.LightBlue;

                fila = 3;
                foreach (var item in estadosTurnos)
                {
                    hojaTurnos.Cell(fila, 4).Value = item.Estado;
                    hojaTurnos.Cell(fila, 5).Value = item.Cantidad;
                    fila++;
                }
                hojaTurnos.Columns().AdjustToContents();

                var hojaRankings = workbook.Worksheets.Add("Rankings");

                hojaRankings.Cell(1, 1).Value = "TOP 5 PACIENTES";
                hojaRankings.Range(1, 1, 1, 2).Style.Font.Bold = true;
                hojaRankings.Cell(2, 1).Value = "Paciente";
                hojaRankings.Cell(2, 2).Value = "Turnos";

                fila = 3;
                foreach (var p in topPacientes)
                {
                    hojaRankings.Cell(fila, 1).Value = p.Paciente;
                    hojaRankings.Cell(fila, 2).Value = p.Turnos;
                    fila++;
                }

                hojaRankings.Cell(1, 4).Value = "MÉTODOS DE PAGO";
                hojaRankings.Range(1, 4, 1, 5).Style.Font.Bold = true;
                hojaRankings.Cell(2, 4).Value = "Método";
                hojaRankings.Cell(2, 5).Value = "Uso";

                fila = 3;
                foreach (var m in metodosPago)
                {
                    hojaRankings.Cell(fila, 4).Value = m.MetodoPago;
                    hojaRankings.Cell(fila, 5).Value = m.Cantidad;
                    fila++;
                }

                hojaRankings.Cell(1, 7).Value = "OBRAS SOCIALES";
                hojaRankings.Range(1, 7, 1, 8).Style.Font.Bold = true;
                hojaRankings.Cell(2, 7).Value = "Obra Social";
                hojaRankings.Cell(2, 8).Value = "Turnos";

                fila = 3;
                foreach (var o in obrasSociales)
                {
                    hojaRankings.Cell(fila, 7).Value = o.Estado;
                    hojaRankings.Cell(fila, 8).Value = o.Cantidad;
                    fila++;
                }

                hojaRankings.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<ReporteTerapeutaDto> GetRendimientoTerapeutaAsync(int terapeutaId)
        {
            var fechaInicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var turnosMes = await _usuarioRepository.GetTurnosRendimientoAsync(terapeutaId, fechaInicioMes);

            int turnosAtendidos = turnosMes.Count(t => t.Sesion != null && t.Sesion.Asistencia == "Presente");

            int pacientesUnicos = turnosMes
                .Where(t => t.Sesion != null && t.Sesion.Asistencia == "Presente")
                .Select(t => t.PacienteId)
                .Distinct()
                .Count();
 
            var turnosFinalizados = turnosMes.Count(t => t.Sesion != null && (t.Sesion.Asistencia == "Presente" || t.Sesion.Asistencia == "Ausente"));

            double tasaAsistencia = turnosFinalizados > 0
                ? Math.Round((double)turnosAtendidos / turnosFinalizados * 100, 2)
                : 0;

            var topPacientes = turnosMes
                .Where(t => t.Sesion != null && t.Sesion.Asistencia == "Presente")
                .GroupBy(t => new { t.Paciente.Nombre, t.Paciente.Apellido })
                .Select(g => new TopPacienteDto
                {
                    NombreCompleto = $"{g.Key.Nombre} {g.Key.Apellido}",
                    CantidadTurnos = g.Count()
                })
                .OrderByDescending(x => x.CantidadTurnos)
                .Take(5)
                .ToList();

            return new ReporteTerapeutaDto
            {
                TurnosAtendidosMes = turnosAtendidos,
                PacientesUnicosMes = pacientesUnicos,
                TasaAsistencia = tasaAsistencia,
                TopPacientes = topPacientes
            };
        }
    }


    }


