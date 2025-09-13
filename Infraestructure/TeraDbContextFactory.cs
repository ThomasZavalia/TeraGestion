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
    public class TeraDbContextFactory:IDesignTimeDbContextFactory<TeraDbContext>
    {
        public TeraDbContext CreateDbContext(string[] args)
        {
            

            var optionsBuilder = new DbContextOptionsBuilder<TeraDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=consultorio_DB;Username=postgres;Password=pongansuclave");
            return new TeraDbContext(optionsBuilder.Options);

        }
    }


}

