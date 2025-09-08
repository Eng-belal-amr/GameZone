public class DevicesController : Controller
{
    private readonly ApplicationDbContext _context;

    public DevicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var devices = _context.Devices.ToList();
        return View(devices);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Device device)
    {
        _context.Devices.Add(device);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var device = _context.Devices.Find(id);
        if (device == null) return NotFound();
        return View(device);
    }

    [HttpPost]
    public IActionResult Edit(Device device)
    {
        _context.Devices.Update(device);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var device = _context.Devices.Find(id);
        if (device == null) return NotFound();
        _context.Devices.Remove(device);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
