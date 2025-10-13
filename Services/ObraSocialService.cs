using AutoMapper;
using Core.DTOs.ObraSocial;
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
        private const decimal PrecioBaseSinObraSocial = 5m;
       private readonly IMapper _mapper;

        public ObraSocialService(IObraSocialRepository obraSocialRepository,IMapper mapper)
        {
            _obraSocialRepository = obraSocialRepository;
            _mapper = mapper;
        }

        public async Task<ObraSocial> GetByIdAsync(int id)
        {
            return await _obraSocialRepository.GetById(id);
        }

        public async Task<decimal> CalcularPrecioTurnoAsync(int? obraSocialId)
        {
            if (obraSocialId.HasValue)
            {
                var obraSocial = await _obraSocialRepository.GetById(obraSocialId.Value);
                if (obraSocial != null)
                    return obraSocial.PrecioTurno;
            }

            // Si no tiene obra social o no se encuentra, usar precio base
            return PrecioBaseSinObraSocial;
        }

        public Task<IEnumerable<ObraSocialDto>> GetObrasSocialesAsync()
        {
            var obrasSociales = _obraSocialRepository.ObtenerTodos();
            return _mapper.Map<Task<IEnumerable<ObraSocialDto>>>(obrasSociales);

        }

        public Task<ObraSocialDto> CrearObraSocialAsync(ObraSocialDto obraSocialDto)
        {

            var nuevaObraSocial = _mapper.Map<ObraSocial>(obraSocialDto);
            var obraSocialCreada = _obraSocialRepository.Agregar(nuevaObraSocial);
            return _mapper.Map<Task<ObraSocialDto>>(obraSocialCreada);

        }

        public Task<ObraSocialDto> ActualizarObraSocialAsync(int id,ObraSocialDto obraSocialDto)
        {

            var obraSocialExistente = _obraSocialRepository.GetById(id).Result;

            if (obraSocialExistente == null)
            {
                return null;
            }

            // Mapear los valores del DTO sobre la entidad existente
            _mapper.Map(obraSocialDto, obraSocialExistente);

            // Actualizar directamente la obra social existente
            var obraSocialActualizada = _obraSocialRepository.Actualizar(obraSocialExistente);

            return _mapper.Map<Task<ObraSocialDto>>(obraSocialActualizada);
        }

        public async Task<bool> EliminarObraSocialAsync(int id)
        {
            var obraSocial = await _obraSocialRepository.GetById(id);
            if(obraSocial == null)
            {
                return false;
            }
            await _obraSocialRepository.Eliminar(id);
            return true;

        }
    }
}

