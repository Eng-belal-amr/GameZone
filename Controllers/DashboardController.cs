using Microsoft.AspNetCore.Mvc;
using GameZone.Data;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalGames = _context.Games.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.TotalDevices = _context.Devices.Count();
            return View();
        }
    }
}
