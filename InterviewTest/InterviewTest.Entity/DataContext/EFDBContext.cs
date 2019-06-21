using InterviewTest.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InterviewTest.Entity.DataContext
{
    public class EFDBContext : DbContext
    {
        public EFDBContext()
        {
        }
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Image> Image { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var con = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build().GetConnectionString("DefaultDBConnection").ToString();
                optionsBuilder.UseSqlServer(con);
                base.OnConfiguring(optionsBuilder);
            }
        }


    }
}
