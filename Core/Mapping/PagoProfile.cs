using AutoMapper;
using Core.DTOs.Pago.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class PagoProfile:Profile
    {

        public PagoProfile()
        {
            CreateMap<PagoDto, Pago>();
            CreateMap<Pago, PagoDto>();

            CreateMap<Pago, PagoDetallesDto>()
               
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.Turno.Paciente.Id))
                .ForMember(dest => dest.PacienteNombre, opt => opt.MapFrom(src => src.Turno.Paciente.Nombre))
                .ForMember(dest => dest.PacienteApellido, opt => opt.MapFrom(src => src.Turno.Paciente.Apellido));
        }
    }
}
