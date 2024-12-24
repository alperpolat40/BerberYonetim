using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.AspNetCore.Mvc;

namespace BerberYonetim.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly AppDbContext _context;

        // Veritabanı bağlantısını almak için constructor
        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }

        // Üye Ol Sayfası
        public IActionResult UyeOl()
        {
            return View();
        }

        // Üye Olma İşlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UyeOl(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                _context.Kullanicilar.Add(kullanici);
                _context.SaveChanges();
                TempData["UyeOlundu"] = "Tebrikler! Üyeliğiniz başarıyla oluşturuldu.";
                return RedirectToAction("UyeOl");
            }
            return View(kullanici);
        }


        // Giriş Yap Sayfası
        public IActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GirisYap(string email, string sifre)
        {
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(k => k.Email == email && k.Sifre == sifre);

            if (kullanici != null)
            {
                HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                HttpContext.Session.SetString("KullaniciAdi", kullanici.AdSoyad);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = "Geçersiz e-posta veya şifre!";
            return View();
        }



        // Çıkış Yap İşlemi
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear(); // Tüm oturum verilerini temizle
            return RedirectToAction("GirisYap");
        }
    }
}
