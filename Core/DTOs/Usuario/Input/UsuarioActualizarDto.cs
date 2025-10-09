using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Usuario.Input
{
    public class UsuarioActualizarDto
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }  // Solo admin puede cambiarlo
    }
}
