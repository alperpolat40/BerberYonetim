using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�n� yap�land�r
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BerberYonetimDb")));

// Oturum y�netimi (Session) yap�land�rmas�
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman a��m� s�resi
    options.Cookie.HttpOnly = true; // Cookie'nin yaln�zca HTTP �zerinden eri�ilebilir olmas�
    options.Cookie.IsEssential = true; // Cookie'nin zorunlu oldu�unu belirtir
});

// MVC deste�ini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata y�netimi ve HTTPS y�nlendirmesi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Orta katman (middleware) yap�land�rmas�
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Oturum deste�ini etkinle�tir
app.UseAuthorization();

// Varsay�lan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Veritaban�n� ba�lang�� verileriyle doldur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // ��lemleri kontrol edip ekle
    if (!context.Islemler.Any())
    {
        context.Islemler.AddRange(
            new Islem { Ad = "Sa� Kesimi", Sure = 30, Ucret = 50 },
            new Islem { Ad = "Sa� Boyama", Sure = 60, Ucret = 150 },
            new Islem { Ad = "Sakal T�ra��", Sure = 20, Ucret = 30 },
            new Islem { Ad = "Bak�m ve Maske", Sure = 45, Ucret = 80 },
            new Islem { Ad = "Keratin Bak�m�", Sure = 90, Ucret = 200 }
        );
        context.SaveChanges();
    }

    // Kuaf�rleri kontrol edip ekle
    if (!context.Kuaforler.Any())
    {
        var kuaforler = new List<Kuafor>
        {
            new Kuafor { Ad = "Ali Kuaf�r", Telefon = "05555555555", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Ay�e Kuaf�r", Telefon = "05444444444", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Mehmet Kuaf�r", Telefon = "05333333333", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Fatma Kuaf�r", Telefon = "05222222222", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Hakan Kuaf�r", Telefon = "05111111111", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Elif Kuaf�r", Telefon = "05666666666", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Cem Kuaf�r", Telefon = "05777777777", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Zeynep Kuaf�r", Telefon = "05888888888", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Can Kuaf�r", Telefon = "05999999999", CalismaSaatleri = "09:00 - 18:00" },
            new Kuafor { Ad = "Seda Kuaf�r", Telefon = "05000000000", CalismaSaatleri = "09:00 - 18:00" }
        };

        context.Kuaforler.AddRange(kuaforler);
        context.SaveChanges();

        // ��lem ve kuaf�rleri ili�kilendir
        var islemler = context.Islemler.ToList();
        var kuaforIndex = 0;

        foreach (var islem in islemler)
        {
            context.KuaforIslemler.AddRange(
                new KuaforIslem { KuaforId = kuaforler[kuaforIndex].Id, IslemId = islem.Id },
                new KuaforIslem { KuaforId = kuaforler[kuaforIndex + 1].Id, IslemId = islem.Id }
            );

            kuaforIndex += 2; // �ki kuaf�r� bir i�leme at�yoruz
        }

        context.SaveChanges();
    }
}

app.Run();
