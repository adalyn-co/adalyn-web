using Microsoft.EntityFrameworkCore;

namespace Adalyn.API
{
    public class AppDbContext : DbContext
    {
        public DbSet<Urun> Urunler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // C#'ın tam olarak anladığı formatta Neon bağlantısı
            optionsBuilder.UseNpgsql("Host=ep-muddy-hill-b1y4evjn-pooler.c-5.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_2pKPkC5uZcjO;SslMode=Require;");
        }
    }
}