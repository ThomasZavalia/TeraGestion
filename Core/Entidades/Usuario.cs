using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entidades
{
    public class Usuario
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string Rol { get; set; }

        public int DuracionTurnoDefault { get; set; } = 60;

      public string? ResetToken { get; set; }
       public DateTime? ResetTokenExpiry { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}
