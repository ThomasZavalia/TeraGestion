using Core.DTOs.Paciente;
using Core.Entidades;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ObraSocialService : IObraSocialService
    {

        private readonly IObraSocialRepository _obraSocialRepository;
        private const decimal PrecioBaseSinObraSocial = 5000m; // puedes cambiar o parametrizar

        public ObraSocialService(IObraSocialRepository obraSocialRepository)
        {
            _obraSocialRepository = obraSocialRepository;
        }

        public async Task<ObraSocial> GetByIdAsync(int id)
        {
            return await _obraSocialRepository.GetById(id);
        }

        public async Task<decimal> CalcularPrecioTurnoAsync(PacienteDTO paciente)
        {
            if (paciente.ObraSocialId.HasValue)
            {
                var obraSocial = await _obraSocialRepository.GetById(paciente.ObraSocialId.Value);
                if (obraSocial != null)
                    return obraSocial.PrecioTurno;
            }

            // Si no tiene obra social o no se encuentra, usar precio base
            return PrecioBaseSinObraSocial;
        }
    }
}

