using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.DataContext
{
    public class RDVFDataContext : DbContext
    {
        public DbSet<BaseFighter> Fighters { get; set; }
        public DbSet<BaseFight> Fights { get; set; }
        public IConfigurationRoot Configuration { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false)
                                .Build();

            optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
