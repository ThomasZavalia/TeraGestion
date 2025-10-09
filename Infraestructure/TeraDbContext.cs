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

        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Paciente>().ToTable("Pacientes");
            modelBuilder.Entity<Turno>().ToTable("Turnos");
            modelBuilder.Entity<Pago>().ToTable("Pagos");
            modelBuilder.Entity<Sesion>().ToTable("Sesiones");
            modelBuilder.Entity<ObraSocial>().ToTable("ObrasSociales");

            modelBuilder.Entity<ObraSocial>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(o => o.PrecioTurno).HasColumnType("decimal(10,2)");
            });

            // Paciente
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.FechaNacimiento).HasColumnType("date");
                entity.HasOne(p => p.ObraSocial)
                      .WithMany(o => o.Pacientes)
                      .HasForeignKey(p => p.ObraSocialId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Turno
            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.FechaHora).HasColumnType("timestamp");
                entity.Property(t => t.Precio).HasColumnType("decimal(10,2)");
                entity.HasOne(t => t.Paciente)
                      .WithMany(p => p.Turnos)
                      .HasForeignKey(t => t.PacienteId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(t => t.ObraSocial)
                      .WithMany(o => o.Turnos)
                      .HasForeignKey(t => t.ObraSocialId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Sesion
            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.FechaHoraInicio).HasColumnType("timestamp");
                entity.Property(s => s.Notas).HasMaxLength(1000);
                entity.HasOne(s => s.Turno)
                      .WithMany(t => t.Sesiones)
                      .HasForeignKey(s => s.TurnoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Pago
            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Fecha).HasColumnType("timestamp");
                entity.Property(p => p.Monto).HasColumnType("decimal(10,2)");
                entity.Property(p => p.MetodoPago).HasMaxLength(50);
                entity.HasOne(p => p.Turno)
                      .WithMany(t => t.Pagos)
                      .HasForeignKey(p => p.TurnoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
