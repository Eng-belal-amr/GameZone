using GameZone.Services;
using GameZone.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGamesService _gamesService;
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;

        public GamesController(
            IGamesService gamesService,
            ICategoriesService categoriesService,
            IDevicesService devicesService)
        {
            _gamesService = gamesService;
            _categoriesService = categoriesService;
            _devicesService = devicesService;
        }

        // ---------------- Index ----------------
        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        // ---------------- Details ----------------
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game == null) return NotFound();
            return View(game);
        }

        // ---------------- Create GET ----------------
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateGameFormViewModel
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList()
            };

            return View(viewModel);
        }

        // ---------------- Create POST ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            await _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        // ---------------- Edit GET ----------------
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();

            var viewModel = new EditGameFormViewModel
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CurrentCover = game.Cover
            };

            return View(viewModel);
        }

        // ---------------- Edit POST ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            var game = await _gamesService.Update(model);
            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // ---------------- Delete ----------------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool deleted = _gamesService.Delete(id);
            return Json(new { success = deleted });
        }


    }
}
