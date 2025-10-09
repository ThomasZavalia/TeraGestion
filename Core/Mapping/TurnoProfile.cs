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
        }
    }
}
