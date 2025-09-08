using GameZone.Services; // ضروري علشان IGamesService
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesService _gamesService;

        public HomeController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            // بدل _context.Games.ToList()
            var games = _gamesService.GetAll();
            return View(games);
        }
    }
}
