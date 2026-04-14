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
        private readonly ITurnoRepository _turnoRepository;


        public ReportesService(IPacienteService pacienteService, ITurnoService turnoService, IPagoService pagoService, IUsuariosRepository usuariosRepository, ITurnoRepository turnoRepository)

        {
            _pacienteService = pacienteService;
            _turnoService = turnoService;
            _pagoService = pagoService;
            _usuarioRepository = usuariosRepository;
            _turnoRepository = turnoRepository;
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
            var pagosQuery = (await _pagoService.GetPagosSinDto())
                             .Where(p => p.Turno != null && p.Anulado != true && p.Turno.Terapeuta != null);

            if (fechaDesde.HasValue) pagosQuery = pagosQuery.Where(p => p.Fecha.Date >= fechaDesde.Value.Date);
            if (fechaHasta.HasValue) pagosQuery = pagosQuery.Where(p => p.Fecha.Date <= fechaHasta.Value.Date);

            var query = pagosQuery
                .GroupBy(p => new { p.Fecha.Year, p.Fecha.Month })
                .Select(g => {
                   
                    decimal totalFacturado = g.Sum(p => p.Monto ?? 0);

                    decimal pagoTerapeutas = g.Sum(p => (p.Monto ?? 0) * (p.Turno.Terapeuta.PorcentajeGanancia / 100m));

                    decimal gananciaClinica = totalFacturado - pagoTerapeutas;

                    return new ReporteMesDto
                    {
                        Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                        Valor = totalFacturado, 
                        TotalFacturado = totalFacturado,
                        PagoTerapeutas = pagoTerapeutas,
                        GananciaClinica = gananciaClinica
                    };
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
            var topPacientesTupla = await _turnoRepository.GetTopPacientesReporteAsync();

            return topPacientesTupla.Select(p => new ReporteTopPacienteDto
            {
                Paciente = p.Paciente,
                Turnos = p.Turnos
            }).ToList();
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

                hojaFinanzas.Cell("A1").Value = "REPORTE FINANCIERO - TERAGESTIÓN";
                hojaFinanzas.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 16;
                hojaFinanzas.Cell("A2").Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
                hojaFinanzas.Range("A2:D2").Merge().Style.Font.SetItalic();

                hojaFinanzas.Cell("A4").Value = "Mes";
                hojaFinanzas.Cell("B4").Value = "Facturación Bruta";
                hojaFinanzas.Cell("C4").Value = "Honorarios Profesionales";
                hojaFinanzas.Cell("D4").Value = "Ganancia Neta Clínica";

                int fila = 5;
                foreach (var item in ingresosPorMes)
                {
                    hojaFinanzas.Cell(fila, 1).Value = item.Mes;

                    hojaFinanzas.Cell(fila, 2).Value = item.TotalFacturado;
                    hojaFinanzas.Cell(fila, 2).Style.NumberFormat.Format = "$ #,##0.00";

                    hojaFinanzas.Cell(fila, 3).Value = item.PagoTerapeutas;
                    hojaFinanzas.Cell(fila, 3).Style.NumberFormat.Format = "$ #,##0.00";

                    hojaFinanzas.Cell(fila, 4).Value = item.GananciaClinica;
                    hojaFinanzas.Cell(fila, 4).Style.NumberFormat.Format = "$ #,##0.00";

                    fila++;
                }

                if (ingresosPorMes.Any())
                {
                    var tblFinanzas = hojaFinanzas.Range(4, 1, fila - 1, 4).CreateTable("TablaFinanzas");
                    tblFinanzas.Theme = XLTableTheme.TableStyleMedium2; 
                    tblFinanzas.ShowTotalsRow = true; 
                }
                hojaFinanzas.Columns().AdjustToContents();

                var hojaTurnos = workbook.Worksheets.Add("Actividad y Estados");

                hojaTurnos.Cell("A1").Value = "REPORTE DE ACTIVIDAD MENSUAL";
                hojaTurnos.Range("A1:E1").Merge().Style.Font.SetBold().Font.FontSize = 14;

                hojaTurnos.Cell("A3").Value = "Mes";
                hojaTurnos.Cell("B3").Value = "Cantidad de Turnos";

                fila = 4;
                foreach (var item in turnosPorMes)
                {
                    hojaTurnos.Cell(fila, 1).Value = item.Mes;
                    hojaTurnos.Cell(fila, 2).Value = item.Valor;
                    fila++;
                }
                if (turnosPorMes.Any())
                {
                    var tblTurnos = hojaTurnos.Range(3, 1, fila - 1, 2).CreateTable("TablaTurnos");
                    tblTurnos.Theme = XLTableTheme.TableStyleMedium14; 
                }

                hojaTurnos.Cell("D3").Value = "Estado del Turno";
                hojaTurnos.Cell("E3").Value = "Cantidad";

                fila = 4;
                foreach (var item in estadosTurnos)
                {
                    hojaTurnos.Cell(fila, 4).Value = item.Estado;
                    hojaTurnos.Cell(fila, 5).Value = item.Cantidad;
                    fila++;
                }
                if (estadosTurnos.Any())
                {
                    var tblEstados = hojaTurnos.Range(3, 4, fila - 1, 5).CreateTable("TablaEstados");
                    tblEstados.Theme = XLTableTheme.TableStyleMedium14;
                }
                hojaTurnos.Columns().AdjustToContents();

                var hojaRankings = workbook.Worksheets.Add("Métricas Clave");

                hojaRankings.Cell("A1").Value = "RANKINGS Y DISTRIBUCIÓN";
                hojaRankings.Range("A1:H1").Merge().Style.Font.SetBold().Font.FontSize = 14;

                hojaRankings.Cell("A3").Value = "Top Pacientes";
                hojaRankings.Cell("B3").Value = "Turnos";
                fila = 4;
                foreach (var p in topPacientes)
                {
                    hojaRankings.Cell(fila, 1).Value = p.Paciente;
                    hojaRankings.Cell(fila, 2).Value = p.Turnos;
                    fila++;
                }
                if (topPacientes.Any())
                {
                    var tblPac = hojaRankings.Range(3, 1, fila - 1, 2).CreateTable("TablaTopPacientes");
                    tblPac.Theme = XLTableTheme.TableStyleMedium10; 
                }

                hojaRankings.Cell("D3").Value = "Método de Pago";
                hojaRankings.Cell("E3").Value = "Uso";
                fila = 4;
                foreach (var m in metodosPago)
                {
                    hojaRankings.Cell(fila, 4).Value = m.MetodoPago;
                    hojaRankings.Cell(fila, 5).Value = m.Cantidad;
                    fila++;
                }
                if (metodosPago.Any())
                {
                    var tblMet = hojaRankings.Range(3, 4, fila - 1, 5).CreateTable("TablaMetodos");
                    tblMet.Theme = XLTableTheme.TableStyleMedium10;
                }

                hojaRankings.Cell("G3").Value = "Obra Social";
                hojaRankings.Cell("H3").Value = "Turnos";
                fila = 4;
                foreach (var o in obrasSociales)
                {
                    hojaRankings.Cell(fila, 7).Value = o.Estado;
                    hojaRankings.Cell(fila, 8).Value = o.Cantidad;
                    fila++;
                }
                if (obrasSociales.Any())
                {
                    var tblObras = hojaRankings.Range(3, 7, fila - 1, 8).CreateTable("TablaObrasSociales");
                    tblObras.Theme = XLTableTheme.TableStyleMedium10;
                }
                hojaRankings.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<ReporteTerapeutaDto> GetRendimientoTerapeutaAsync(int terapeutaId, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var inicioPeriodo = fechaDesde ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var finPeriodo = fechaHasta ?? DateTime.MaxValue;

            var todosLosTurnos = await _turnoRepository.GetTurnosHistoricoTerapeutaAsync(terapeutaId);

            var terapeuta = await _usuarioRepository.GetById(terapeutaId);
            decimal porcentaje = terapeuta.PorcentajeGanancia / 100m;

            var turnosPeriodo = todosLosTurnos
                .Where(t => t.FechaHora.Date >= inicioPeriodo.Date && t.FechaHora.Date <= finPeriodo.Date)
                .ToList();

            int turnosAtendidos = turnosPeriodo.Count(t => t.Estado == "Atendido");

            int pacientesUnicos = turnosPeriodo
                .Where(t => t.Estado == "Atendido")
                .Select(t => t.PacienteId)
                .Distinct()
                .Count();

            int turnosFinalizados = turnosPeriodo.Count(t => t.Estado == "Atendido" || t.Estado == "Ausente");

            double tasaAsistencia = turnosFinalizados > 0
                ? Math.Round((double)turnosAtendidos / turnosFinalizados * 100, 2)
                : 0;

            var turnosPagados = turnosPeriodo.Where(t =>
                (t.Estado == "Atendido" || t.Estado == "Ausente") &&
                 t.Pagos != null &&
                 t.Pagos.Any(p => p.Anulado != true));
            decimal ganancias = turnosPagados.Sum(t => t.Precio) * porcentaje;

            var topPacientes = turnosPeriodo
                .Where(t => t.Estado == "Atendido")
                .GroupBy(t => new { t.Paciente.Nombre, t.Paciente.Apellido })
                .Select(g => new TopPacienteDto
                {
                    NombreCompleto = $"{g.Key.Nombre} {g.Key.Apellido}",
                    CantidadTurnos = g.Count()
                })
                .OrderByDescending(x => x.CantidadTurnos).Take(5).ToList();


            var evolucionGanancias = todosLosTurnos
                .Where(t =>
                (t.Estado == "Atendido" || t.Estado == "Ausente") &&
                 t.Pagos != null &&
                 t.Pagos.Any(p => p.Anulado != true))

                .Where(t => (!fechaDesde.HasValue || t.FechaHora.Date >= fechaDesde.Value.Date) &&
                            (!fechaHasta.HasValue || t.FechaHora.Date <= fechaHasta.Value.Date))
                .GroupBy(t => new { t.FechaHora.Year, t.FechaHora.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new ReporteMesDto
                {
                    Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                    Valor = g.Sum(t => t.Precio) * porcentaje
                }).ToList();

            var distribucionEstados = turnosPeriodo
                .GroupBy(t => t.Estado)
                .Select(g => new ReporteEstadoDto
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                }).ToList();

            return new ReporteTerapeutaDto
            {
                TurnosAtendidosMes = turnosAtendidos,
                PacientesUnicosMes = pacientesUnicos,
                TasaAsistencia = tasaAsistencia,
                TopPacientes = topPacientes,
                GananciasEstimadasMes = ganancias,
                EvolucionGanancias = evolucionGanancias, 
                DistribucionEstados = distribucionEstados 
            };
        }

    }
}


