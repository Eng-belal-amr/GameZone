using Microsoft.AspNetCore.Mvc;
using GameZone.Data;
using System.Linq;

namespace GameZone.Controllers.Api
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("games-by-category")]
        public IActionResult GamesByCategory()
        {
            var data = _context.Categories
                .Select(c => new
                {
                    label = c.Name,
                    value = c.Games.Count
                }).ToList();

            return Ok(new
            {
                labels = data.Select(d => d.label),
                values = data.Select(d => d.value)
            });
        }
    }
}
