using Core.Entidades;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Paciente;

namespace Core.Mapping
{
    public class PacienteProfile:Profile
    {
        public PacienteProfile()
        {
            CreateMap<PacienteDTO, Paciente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Paciente, PacienteDTO>();

            CreateMap<Paciente, PacienteSimpleDto>();





            // 1. Mapeo para Sesion -> SesionHistorialDto
            CreateMap<Sesion, SesionHistorialDTO>()
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaHoraInicio))
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));

            // 2. Mapeo para Pago -> PagoHistorialDto (este es simple)
            CreateMap<Pago, PagoHistorialDTO>();

            // 3. Mapeo principal para Paciente -> PacienteDetalleDto
            CreateMap<Paciente, PacienteDetalleDTO>()
                .ForMember(dest => dest.ObraSocial, opt => opt.MapFrom(src => src.ObraSocial.Nombre))
                .ForMember(dest => dest.Sesiones, opt => opt.MapFrom(src => src.Sesiones))
                .ForMember(dest => dest.Pagos, opt => opt.MapFrom(src =>
                    src.Sesiones
                       .Where(s => s.Turno != null && s.Turno.Pagos != null)
                       .SelectMany(s => s.Turno.Pagos)
                       .Distinct()
                ));
        }
    }
}
