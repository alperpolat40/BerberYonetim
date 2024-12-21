using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BerberYonetim.Data;
using BerberYonetim.Models;

namespace BerberYonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CalisanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalisanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Çalışanları listeleme
        public IActionResult Index()
        {
            var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        // Yeni çalışan ekleme (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Yeni çalışan ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(calisan);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(calisan);
        }

        // Çalışan düzenleme (GET)
        public IActionResult Edit(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        // Çalışan düzenleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Update(calisan);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(calisan);
        }

        // Çalışan silme
        public IActionResult Delete(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan == null)
            {
                return NotFound();
            }
            _context.Calisanlar.Remove(calisan);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}