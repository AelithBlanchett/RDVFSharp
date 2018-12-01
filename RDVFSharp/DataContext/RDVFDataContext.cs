using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RDVFSharp.Entities;

namespace RDVFSharp.DataContext
{
    public class RDVFDataContext : DbContext
    {
        public DbSet<BaseFighter> Fighters { get; set; }
        public DbSet<BaseFight> Fights { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        public RDVFDataContext()
        { }

        public RDVFDataContext(DbContextOptions<RDVFDataContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                Configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: false)
                                .Build();

                optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
