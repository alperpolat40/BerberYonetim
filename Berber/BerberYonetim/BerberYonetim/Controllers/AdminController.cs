using BerberYonetim.Data;
using BerberYonetim.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BerberYonetim.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
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
            var adminEmail = "g191210039@sakarya.edu.tr";
            var adminSifre = "38377758514-Alper";

            if (email == adminEmail && sifre == adminSifre)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Panel");
            }

            ViewBag.Hata = "Geçersiz e-posta veya şifre!";
            return View();
        }

        // Admin paneli
        [Authorize(Roles = "Admin")]
        public IActionResult Panel()
        {
            return View();
        }

        // Admin çıkış işlemi
        public IActionResult Cikis()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris");
        }

        // Kuaför Yönetimi
        [Authorize(Roles = "Admin")]
        public IActionResult KuaforYonetimi()
        {
            var kuaforler = _context.Kuaforler.ToList();
            return View(kuaforler);
        }

        // Kuaför Ekleme
        [Authorize(Roles = "Admin")]
        public IActionResult KuaforEkle()
        {
            ViewBag.Islemler = new SelectList(_context.Islemler.ToList(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult KuaforEkle(Kuafor kuafor, List<int> islemIds)
        {
            if (ModelState.IsValid)
            {
                _context.Kuaforler.Add(kuafor);
                _context.SaveChanges();

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

            ViewBag.Islemler = _context.Islemler.ToList();
            return View(kuafor);
        }

        // Kuaför Silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // Randevu Yönetimi
        [Authorize(Roles = "Admin")]
        public IActionResult RandevuYonetimi()
        {
            var randevular = _context.Randevular
                .Include(r => r.Kuafor)
                .Include(r => r.Islem)
                .Include(r => r.Kullanici)
                .ToList();

            return View(randevular);
        }

        // Randevu Silme
        [Authorize(Roles = "Admin")]
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

        // İşlem Yönetimi
        [Authorize(Roles = "Admin")]
        public IActionResult IslemYonetimi()
        {
            var islemler = _context.Islemler.ToList();
            return View(islemler);
        }

        // İşlem Ekleme
        [Authorize(Roles = "Admin")]
        public IActionResult IslemEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult IslemEkle(Islem islem)
        {
            if (ModelState.IsValid)
            {
                _context.Islemler.Add(islem);
                _context.SaveChanges();
                TempData["Basari"] = "İşlem başarıyla eklendi!";
                return RedirectToAction("IslemYonetimi");
            }

            TempData["Hata"] = "Formda eksik veya yanlış bilgiler var!";
            return View(islem);
        }

        // İşlem Silme
        [Authorize(Roles = "Admin")]
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
    }
}
