using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BerberYonetim.Controllers
{
    public class RandevuController : Controller
    {
        private readonly AppDbContext _context;

        public RandevuController(AppDbContext context)
        {
            _context = context;
        }

        // Randevu Al Sayfası
        public IActionResult Al()
        {
            ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad");
            ViewBag.Saatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00" };
            return View();
        }

        // Uzman Kuaförleri Listele
        [HttpPost]
        public IActionResult UzmanlariGetir(int islemId)
        {
            var uzmanKuaforler = _context.KuaforIslemler
                .Where(ki => ki.IslemId == islemId)
                .Select(ki => ki.Kuafor)
                .ToList();

            return PartialView("_UzmanKuaforler", uzmanKuaforler);
        }

        // Randevu Al (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Al(Randevu randevu)
        {
            // Oturumdan KullaniciId'yi al
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                ModelState.AddModelError("", "Randevu almak için giriş yapmanız gerekiyor.");
                ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad");
                ViewBag.Saatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00" };
                return View(randevu);
            }

            // KullaniciId'yi randevuya ata
            randevu.KullaniciId = kullaniciId.Value;

            // Çakışma kontrolü
            bool cakisma = _context.Randevular.Any(r =>
                r.KuaforId == randevu.KuaforId &&
                r.Tarih == randevu.Tarih &&
                r.Saat == randevu.Saat);

            if (cakisma)
            {
                ModelState.AddModelError("", "Seçilen saat için bu kuaförde randevu zaten mevcut!");
                ViewBag.Islemler = new SelectList(_context.Islemler, "Id", "Ad");
                ViewBag.Saatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00" };
                return View(randevu);
            }

            // Randevuyu kaydet
            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            TempData["Basari"] = "Randevunuz başarıyla alındı!";
            return RedirectToAction("Al");
        }

        public IActionResult Randevularim()
        {
            // Oturumdan KullaniciId'yi al
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
            {
                return RedirectToAction("GirisYap", "Kullanici");
            }

            // Kullanıcının randevularını getir
            var randevular = _context.Randevular
                .Where(r => r.KullaniciId == kullaniciId)
                .Include(r => r.Islem)  // İşlem bilgilerini dahil et
                .Include(r => r.Kuafor) // Kuaför bilgilerini dahil et
                .ToList();

            return View(randevular);
        }
        public IActionResult Sil(int id)
        {
            // Randevuyu bul
            var randevu = _context.Randevular.FirstOrDefault(r => r.Id == id);

            if (randevu == null)
            {
                return NotFound();
            }

            // Oturumdaki kullanıcı bu randevuyu silebilir mi?
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId != randevu.KullaniciId)
            {
                return Unauthorized();
            }

            // Randevuyu sil
            _context.Randevular.Remove(randevu);
            _context.SaveChanges();

            TempData["Basari"] = "Randevunuz başarıyla iptal edildi.";
            return RedirectToAction("Randevularim");
        }
        

       





    }
}
