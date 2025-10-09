using Core.Entidades;
using Core.Interfaces.Repositorios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class UsuarioRepository : IUsuariosRepository
    {
        private readonly TeraDbContext _context;

        public UsuarioRepository(TeraDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario> Actualizar(Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            if (usuarioExistente == null) { return null; }

                usuarioExistente.Username = usuario.Username;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Rol = usuario.Rol;
            await _context.SaveChangesAsync();
            return usuarioExistente;


        }

        
        public async Task<Usuario> Agregar(Usuario usuario)
        {
            if (usuario == null) { return null; }
        
          await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> Eliminar(int id)
        {
            var usuario =  await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) { return false; }
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Usuario>? GetById(int id)

        {
         var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) { return null; }
            return usuario;
        }


        public async Task<IEnumerable<Usuario>> ObtenerTodos()

        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return usuarios;
        }

    }  
 
}
 