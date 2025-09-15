using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /*  public class TeraDbContextFactory:IDesignTimeDbContextFactory<TeraDbContext>
      {






          public TeraDbContext CreateDbContext(string[] args)
          {


              var optionsBuilder = new DbContextOptionsBuilder<TeraDbContext>();
              optionsBuilder.UseNpgsql("Host=localhost;Database=consultorio_db;Username=postgres;Password=dkdemicorazon");
              return new TeraDbContext(optionsBuilder.Options);

          }
      }
    */


    public class TeraDbContextFactory : IDesignTimeDbContextFactory<TeraDbContext>
    {
        public TeraDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Controllers")) 
                .Build();

            var provider = configuration["Database:Provider"];
            var connectionString = configuration["Database:ConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<TeraDbContext>();

            switch (provider)
            {
                case "Postgres":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                case "MySQL":
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    break;
                case "SQLite":
                    optionsBuilder.UseSqlite(connectionString);
                    break;
                default:
                    throw new Exception("Proveedor de base de datos no soportado.");
            }

            return new TeraDbContext(optionsBuilder.Options);
        }
    }


}

