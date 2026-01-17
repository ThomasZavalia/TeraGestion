using AutoMapper;
using Core.DTOs.Paciente;
using Core.DTOs.Turno.Input;
using Core.DTOs.Turno.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class TurnoProfile:Profile
    {

        public TurnoProfile()
        {
            CreateMap<TurnoDto, Turno>();
            CreateMap<Turno, TurnoDto>();


            CreateMap<Turno, TurnoCalendarioDto>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))

     .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.FechaHora))
     .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora)) 
    .ForMember(dest => dest.End, opt => opt.MapFrom(src =>
        src.FechaHora.AddMinutes(src.Duracion > 0 ? src.Duracion : 40)))

    .ForMember(dest => dest.Duracion, opt => opt.MapFrom(src => src.Duracion))


     .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
         $"{src.Paciente.Nombre} {src.Paciente.Apellido}"))

     .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado)) 
     .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
     .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
     .ForMember(dest => dest.ObraSocialId, opt => opt.MapFrom(src => src.ObraSocialId))
   
     ;


            CreateMap<Turno, TurnoDetalleDto>()
                .ForMember(dest => dest.PacienteNombre, opt => opt.MapFrom(src => $"{src.Paciente.Nombre} {src.Paciente.Apellido}"));
        }
    }
}
