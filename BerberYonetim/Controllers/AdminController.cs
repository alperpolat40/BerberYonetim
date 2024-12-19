using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BerberYonetim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Çalışan Yönetimi
        public IActionResult ManageEmployees()
        {
            return View();
        }

        // İşlem Yönetimi
        public IActionResult ManageServices()
        {
            return View();
        }
    }
}
