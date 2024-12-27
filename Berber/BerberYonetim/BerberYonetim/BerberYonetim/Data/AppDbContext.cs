using BerberYonetim.Models;
using Microsoft.EntityFrameworkCore;

namespace BerberYonetim.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Kullanıcı tablosu tanımı
        public DbSet<Kullanici> Kullanicilar { get; set; }

        // Diğer tablolar
        public DbSet<Kuafor> Kuaforler { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<KuaforIslem> KuaforIslemler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kuaför-İşlem ilişkisi tanımı
            modelBuilder.Entity<KuaforIslem>()
                .HasKey(ki => new { ki.KuaforId, ki.IslemId });

            modelBuilder.Entity<KuaforIslem>()
                .HasOne(ki => ki.Kuafor)
                .WithMany(k => k.Uzmanliklar)
                .HasForeignKey(ki => ki.KuaforId);

            modelBuilder.Entity<KuaforIslem>()
                .HasOne(ki => ki.Islem)
                .WithMany(i => i.Kuaforler)
                .HasForeignKey(ki => ki.IslemId);
        }
    }
}
