using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsýný yapýlandýr
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BerberYonetimDb")));

// Oturum yönetimi (Session) yapýlandýrmasý
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþýmý süresi
    options.Cookie.HttpOnly = true; // Cookie'nin yalnýzca HTTP üzerinden eriþilebilir olmasý
    options.Cookie.IsEssential = true; // Cookie'nin zorunlu olduðunu belirtir
});

// MVC desteðini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata yönetimi ve HTTPS yönlendirmesi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Orta katman (middleware) yapýlandýrmasý
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Oturum desteðini etkinleþtir
app.UseAuthorization();

// Varsayýlan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Veritabanýný baþlangýç verileriyle doldur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ýþlemleri kontrol edip ekle
    if (!context.Islemler.Any())
    {
        context.Islemler.AddRange(
            new Islem { Ad = "Saç Kesimi", Sure = 30, Ucret = 50 },
            new Islem { Ad = "Saç Boyama", Sure = 60, Ucret = 150 },
            new Islem { Ad = "Sakal Týraþý", Sure = 20, Ucret = 30 },
            new Islem { Ad = "Bakým ve Maske", Sure = 45, Ucret = 80 },
            new Islem { Ad = "Keratin Bakýmý", Sure = 90, Ucret = 200 }
        );
        context.SaveChanges();
    }

    // Kuaförleri kontrol edip ekle
    if (!context.Kuaforler.Any())
    {
        var kuaforler = new List<Kuafor>
        {
            new Kuafor { Ad = "Ali Kuaför", Telefon = "05555555555", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Ayþe Kuaför", Telefon = "05444444444", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Mehmet Kuaför", Telefon = "05333333333", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Fatma Kuaför", Telefon = "05222222222", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Hakan Kuaför", Telefon = "05111111111", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Elif Kuaför", Telefon = "05666666666", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Cem Kuaför", Telefon = "05777777777", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Zeynep Kuaför", Telefon = "05888888888", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Can Kuaför", Telefon = "05999999999", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Seda Kuaför", Telefon = "05000000000", CalismaSaatleri = "09:00 - 18:00" }
        };

        context.Kuaforler.AddRange(kuaforler);
        context.SaveChanges();

        // Ýþlem ve kuaförleri iliþkilendir
        var islemler = context.Islemler.ToList();
        var kuaforIndex = 0;

        foreach (var islem in islemler)
        {
            context.KuaforIslemler.AddRange(
                new KuaforIslem { KuaforId = kuaforler[kuaforIndex].Id, IslemId = islem.Id },
                new KuaforIslem { KuaforId = kuaforler[kuaforIndex + 1].Id, IslemId = islem.Id }
            );

            kuaforIndex += 2; // Ýki kuaförü bir iþleme atýyoruz
        }

        context.SaveChanges();
    }
}

app.Run();
