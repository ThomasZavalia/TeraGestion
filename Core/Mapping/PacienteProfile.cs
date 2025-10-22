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
            CreateMap<PacienteDTO, Paciente>();
            CreateMap<Paciente, PacienteDTO>();

            CreateMap<Paciente, PacienteSimpleDto>();
        }
    }
}
