using Microsoft.EntityFrameworkCore;

namespace Adalyn.API
{
    public class AppDbContext : DbContext
    {
        public DbSet<Urun> Urunler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Veritabanı dosyamızın adını belirliyoruz
            optionsBuilder.UseSqlite("Data Source=adalyn.db");
        }
    }
}