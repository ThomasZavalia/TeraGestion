using AutoMapper;
using Core.DTOs.Usuario.Input;
using Core.DTOs.Usuario.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class UsuarioProfile:Profile
    {

        public UsuarioProfile()
        {
           
            CreateMap<Usuario, UsuarioDto>();

           
            CreateMap<UsuarioPerfilDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignora Id
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Ignora Hash
                .ForMember(dest => dest.Rol, opt => opt.Ignore()); // Ignora Rol

            // Si tienes UsuarioActualizarDto (para Admin), mapea aquí también
            CreateMap<UsuarioActualizarDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); 
        }
    }
}
