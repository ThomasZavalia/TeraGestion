using Core.DTOs;
using Core.DTOs.Usuario.Input;
using Core.DTOs.Usuario.Output;
using Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IUsuarioService
    {
       Task<Usuario> ValidarCredenciales(string username, string password); // Devuelve Entidad solo para Auth
        Task<Usuario> CrearUsuario(Usuario usuario); // Devuelve Entidad solo al crear? O DTO? Considerar.

        
        Task<UsuarioDto> GetUsuarioById(int id); 
        Task<IEnumerable<UsuarioDto>> GetUsuarios(); 
        Task<UsuarioDto> ActualizarUsuario(int id, UsuarioActualizarDto dto); 
        Task<UsuarioDto> ActualizarPerfilUsuario(int id, UsuarioPerfilDto dto);
        Task<bool> CambiarContraseña(int id, string contraseñaActual, string contraseñaNueva); // ¡Nuevo!
        Task<bool> EliminarUsuario(int id);
        Task<Usuario> GetByName(string username);

    }
}
