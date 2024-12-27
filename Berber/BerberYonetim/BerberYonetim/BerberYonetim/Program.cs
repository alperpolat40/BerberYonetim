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


app.Run();
