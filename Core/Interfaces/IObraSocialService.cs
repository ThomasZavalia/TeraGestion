using Core.DTOs.Paciente;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IObraSocialService
    {
        Task<ObraSocial> GetByIdAsync(int id);
        Task<decimal> CalcularPrecioTurnoAsync(PacienteDTO paciente);
    }
}
