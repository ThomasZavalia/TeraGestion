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
        }
    }
}
