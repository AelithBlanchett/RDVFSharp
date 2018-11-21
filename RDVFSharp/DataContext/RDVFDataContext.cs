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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Program.Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
