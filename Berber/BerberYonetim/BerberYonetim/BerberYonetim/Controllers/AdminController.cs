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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KuaforSil(int id)
        {
            var kuafor = _context.Kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor == null)
            {
                TempData["Hata"] = "Kuaför bulunamadı.";
                return RedirectToAction("KuaforYonetimi");
            }

            _context.Kuaforler.Remove(kuafor);
            _context.SaveChanges();
            TempData["Basari"] = "Kuaför başarıyla silindi.";
            return RedirectToAction("KuaforYonetimi");
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




        public IActionResult KuaforEkle()
        {
            ViewBag.Islemler = new SelectList(_context.Islemler.ToList(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KuaforEkle(Kuafor kuafor, List<int> islemIds)
        {
            if (ModelState.IsValid)
            {
                // Kuaförü kaydet
                _context.Kuaforler.Add(kuafor);
                _context.SaveChanges(); // Kuaför kaydedildikten sonra Id'si oluşacak

                // Uzmanlık alanlarını ilişkilendir
                if (islemIds != null && islemIds.Any())
                {
                    foreach (var islemId in islemIds)
                    {
                        _context.KuaforIslemler.Add(new KuaforIslem
                        {
                            KuaforId = kuafor.Id,
                            IslemId = islemId
                        });
                    }
                    _context.SaveChanges();
                }

                TempData["Basari"] = "Kuaför ve uzmanlık alanları başarıyla eklendi!";
                return RedirectToAction("KuaforYonetimi");
            }

            // ModelState hatası durumunda ViewBag'i tekrar doldur
            ViewBag.Islemler = _context.Islemler.ToList();
            return View(kuafor);
        }
        public IActionResult IslemYonetimi()
        {
            var islemler = _context.Islemler.ToList(); // Tüm işlemleri getir
            return View(islemler);
        }
        public IActionResult IslemEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IslemEkle(Islem islem)
        {
            if (ModelState.IsValid) // Model doğrulama kontrolü
            {
                _context.Islemler.Add(islem); // Yeni işlemi ekle
                _context.SaveChanges(); // Veritabanına kaydet
                TempData["Basari"] = "İşlem başarıyla eklendi!"; // Başarı mesajı
                return RedirectToAction("IslemYonetimi"); // Yönlendirme
            }

            // ModelState hataları varsa aynı sayfayı göster
            TempData["Hata"] = "Formda eksik veya yanlış bilgiler var!";
            return View(islem); // Hata varsa formu geri döndür
        }

        public IActionResult SilIslem(int id)
        {
            var islem = _context.Islemler.FirstOrDefault(i => i.Id == id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem); // İşlemi sil
                _context.SaveChanges(); // Değişiklikleri kaydet
                TempData["Mesaj"] = "İşlem başarıyla silindi."; // Silme mesajı
            }
            return RedirectToAction("IslemYonetimi"); // İşlem yönetimine yönlendir
        }

    }
}
