using GameZone.Data;
using GameZone.Models;
using GameZone.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;

        public GamesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET ALL
        // =========================================================
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                    .ThenInclude(d => d.Device)
                .ToList();
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        public Game? GetById(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                    .ThenInclude(d => d.Device)
                .FirstOrDefault(g => g.Id == id);
        }

        // =========================================================
        // CREATE
        // =========================================================
        public async Task Create(CreateGameFormViewModel model)
        {
            // 1. تحديد مسار الصور
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            // 2. التأكد من رفع صورة
            if (model.Cover != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(model.Cover.FileName)
                             + "_" + Guid.NewGuid()
                             + Path.GetExtension(model.Cover.FileName);

                var filePath = Path.Combine(imagesPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Cover.CopyToAsync(stream);
                }

                var game = new Game
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Cover = fileName
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                foreach (var deviceId in model.SelectedDevices)
                {
                    _context.GameDevices.Add(new GameDevice
                    {
                        GameId = game.Id,
                        DeviceId = deviceId
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        // =========================================================
        // UPDATE
        // =========================================================
        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .FirstOrDefault(g => g.Id == model.Id);

            if (game == null) return null;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;

            // تعديل الصورة لو موجودة
            if (model.Cover != null)
            {
                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var fileName = Path.GetFileNameWithoutExtension(model.Cover.FileName)
                             + "_" + Guid.NewGuid()
                             + Path.GetExtension(model.Cover.FileName);

                var filePath = Path.Combine(imagesPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Cover.CopyToAsync(stream);
                }

                game.Cover = fileName;
            }

            // تحديث الأجهزة
            _context.GameDevices.RemoveRange(game.Devices);

            foreach (var deviceId in model.SelectedDevices)
            {
                _context.GameDevices.Add(new GameDevice
                {
                    GameId = game.Id,
                    DeviceId = deviceId
                });
            }

            await _context.SaveChangesAsync();
            return game;
        }

        // =========================================================
        // DELETE
        // =========================================================
        public bool Delete(int id)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .FirstOrDefault(g => g.Id == id);

            if (game == null) return false;

            _context.GameDevices.RemoveRange(game.Devices);
            _context.Games.Remove(game);
            _context.SaveChanges();

            return true;
        }
    }
}
