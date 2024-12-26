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


app.Run();
