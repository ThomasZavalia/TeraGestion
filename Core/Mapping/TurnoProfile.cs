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
               
                .ForMember(dest => dest.Fecha, 
                           opt => opt.MapFrom(src => src.FechaHora)) 

            
                .ForMember(dest => dest.PacienteNombre, opt => opt.MapFrom(src => src.Paciente.Nombre))
                .ForMember(dest => dest.PacienteApellido, opt => opt.MapFrom(src => src.Paciente.Apellido));

            CreateMap<Turno, TurnoDetalleDto>()
            .ForMember(dest => dest.PacienteNombre, opt => opt.MapFrom(src => $"{src.Paciente.Nombre} {src.Paciente.Apellido}"));
        }
    }
}
