using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BerberYonetim.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context; // _context alanı

        // Constructor
        public AdminController(AppDbContext context)
        {
            _context = context; // Dependency Injection ile _context başlatılıyor
        }

        // Admin giriş sayfasını göster
        public IActionResult Giris()
        {
            return View();
        }

        // Admin giriş işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Giris(string email, string sifre)
        {
            // Sabit admin kullanıcı bilgileri
            var adminEmail = "admin@gmail.com";
            var adminSifre = "admin12345";

            if (email == adminEmail && sifre == adminSifre)
            {
                // Admin oturumu başlat
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Panel");
            }

            // Hata mesajı
            ViewBag.Hata = "Geçersiz e-posta veya şifre!";
            return View();
        }

        // Admin paneli
        public IActionResult Panel()
        {
            // Admin oturum kontrolü
            if (HttpContext.Session.GetString("Admin") != "true")
            {
                return Unauthorized(); // Yetkisiz erişim
            }

            return View();
        }

        // Admin çıkış işlemi
        public IActionResult Cikis()
        {
            HttpContext.Session.Remove("Admin"); // Admin oturumunu kapat
            return RedirectToAction("Giris");
        }

        // Kuaför Yönetimi
        public IActionResult KuaforYonetimi()
        {
            var kuaforler = _context.Kuaforler.ToList(); // Tüm kuaförleri getir
            return View(kuaforler);
        }


        // İşlem Yönetimi
        public IActionResult IslemYonetimi()
        {
            var islemler = _context.Islemler.ToList();
            return View(islemler); // İşlemleri listele
        }

        // Randevu Yönetimi
        public IActionResult RandevuYonetimi()
        {
            var randevular = _context.Randevular
                .Include(r => r.Kuafor)
                .Include(r => r.Islem)
                .Include(r => r.Kullanici) // Kullanıcı bilgilerini de ekliyoruz
                .ToList();

            return View(randevular);
        }

        // Kuaför Silme
        public IActionResult SilKuafor(int id)
        {
            var kuafor = _context.Kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor != null)
            {
                _context.Kuaforler.Remove(kuafor);
                _context.SaveChanges();
                TempData["Mesaj"] = "Kuaför başarıyla silindi.";
            }
            return RedirectToAction("KuaforYonetimi");
        }

        // İşlem Silme
        public IActionResult SilIslem(int id)
        {
            var islem = _context.Islemler.FirstOrDefault(i => i.Id == id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem);
                _context.SaveChanges();
                TempData["Mesaj"] = "İşlem başarıyla silindi.";
            }
            return RedirectToAction("IslemYonetimi");
        }

        // Randevu Silme
        public IActionResult SilRandevu(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null)
            {
                return NotFound();
            }

            _context.Randevular.Remove(randevu);
            _context.SaveChanges();

            TempData["Mesaj"] = "Randevu başarıyla silindi.";
            return RedirectToAction("RandevuYonetimi");
        }

        // Randevu Düzenleme Sayfasını Göster
        public IActionResult Duzenle(int id)
        {
            var randevu = _context.Randevular.Find(id);

            if (randevu == null)
                return NotFound();

            ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad", randevu.IslemId);
            ViewBag.Kuaforler = new SelectList(_context.Kuaforler, "Id", "Ad", randevu.KuaforId);
            ViewBag.Saatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00" };

            return View(randevu);
        }



        // Randevuyu Düzenle (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Duzenle(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                var randevuDb = _context.Randevular.FirstOrDefault(r => r.Id == randevu.Id);
                if (randevuDb == null)
                {
                    return NotFound();
                }

                randevuDb.IslemId = randevu.IslemId;
                randevuDb.KuaforId = randevu.KuaforId;
                randevuDb.Tarih = randevu.Tarih;
                randevuDb.Saat = randevu.Saat;

                _context.SaveChanges();
                TempData["Basari"] = "Randevu başarıyla güncellendi.";
                return RedirectToAction("RandevuYonetimi");
            }

            ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad", randevu.IslemId);
            ViewBag.Kuaforler = new SelectList(_context.Kuaforler, "Id", "Ad", randevu.KuaforId);
            ViewBag.Saatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00" };

            return View(randevu);
        }


        public IActionResult KuaforEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KuaforEkle(Kuafor kuafor)
        {
            if (ModelState.IsValid) // Model doğrulama kontrolü
            {
                _context.Kuaforler.Add(kuafor); // Kuaförü ekle
                _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
                TempData["Basari"] = "Kuaför başarıyla eklendi!"; // Başarı mesajı
                return RedirectToAction("KuaforYonetimi"); // Yönlendirme
            }

            // ModelState hataları varsa aynı sayfayı göster
            return View(kuafor);
        }






    }
}
