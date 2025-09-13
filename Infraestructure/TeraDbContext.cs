using Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class TeraDbContext : DbContext
    {
        public TeraDbContext(DbContextOptions<TeraDbContext> options) : base(options)
        {
        }

        public DbSet<Core.Entidades.Usuario> Usuarios { get; set; }
        public DbSet<Core.Entidades.Pago> Pagos { get; set; }
        public DbSet<Core.Entidades.Sesion> Sesiones { get; set; }
        public DbSet<Core.Entidades.Turno> Turnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Paciente>().ToTable("Pacientes");
            modelBuilder.Entity<Turno>().ToTable("Turnos");
            modelBuilder.Entity<Pago>().ToTable("Pagos");
            modelBuilder.Entity<Sesion>().ToTable("Sesiones");
        }


    }
}
