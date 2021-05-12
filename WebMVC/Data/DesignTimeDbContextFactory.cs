using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityUserContext>
    {
        public IdentityUserContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder();

            builder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AuthApp;User=sa;password=pa$$w0rd;");
            return new IdentityUserContext(builder.Options);
        }
    }
}
