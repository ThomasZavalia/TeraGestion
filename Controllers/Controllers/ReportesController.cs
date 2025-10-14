using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {

        private readonly IReportesService _reportesService;
        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("top-pacientes")]
        public async Task<IActionResult> GetTopPacientes()
        {
            var topPacientes = await _reportesService.GetTopPacientes();
            return Ok(topPacientes);
        }

        [HttpGet("metodos-pago")]
        public async Task<IActionResult> GetMetodosPago()
        {
            var metodosPago = await _reportesService.GetMetodosPagoDto();
            return Ok(metodosPago);
        }
        [HttpGet("turnos-por-estado")]
        public async Task<IActionResult> GetTurnosPorEstado()
        {
            var turnosPorEstado = await _reportesService.GetTurnoPorEstado();
            return Ok(turnosPorEstado);
        }

        [HttpGet("turnos-por-mes")]
        public async Task<IActionResult> GetTurnosPorMes()
        {
            var turnosPorMes = await _reportesService.GetTurnosPorMes();
            return Ok(turnosPorMes);
        }

        [HttpGet("ingresos-por-mes")]
        public async Task<IActionResult> GetIngresosPorMes()
        {
            var ingresosPorMes = await _reportesService.GetIngresosPorMes();
            return Ok(ingresosPorMes);
        }


    }
}
