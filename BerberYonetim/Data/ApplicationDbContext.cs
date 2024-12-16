using Microsoft.EntityFrameworkCore;
using BerberYonetim.Models;

namespace BerberYonetim.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Kuafor> Kuaforler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
    }
}
