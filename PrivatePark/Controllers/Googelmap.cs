using Microsoft.AspNetCore.Mvc;
using mnacr22.Data;

namespace mnacr22.Controllers;

public class Googelmap : Controller
{
    private readonly ApplicationDbContext _db;

    public Googelmap(ApplicationDbContext db)
    {
        _db = db; 
    }
    
    
    // GET
    public IActionResult map()
    {
        return View();
    }

    public JsonResult GetAllLocation()
    {
        var location = _db.Locations.ToList();
        return Json(location);
    }
}