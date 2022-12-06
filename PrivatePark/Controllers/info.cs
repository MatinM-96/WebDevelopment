using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;

namespace mnacr22.Controllers;

public class info : Controller
{
    private readonly ApplicationDbContext _db;

    public info(ApplicationDbContext db)
    {
        _db = db;
    }


    // GET


    public JsonResult GetAllParkings()
    {

        var location = _db.Addresses.Where(x => x.Active == true).Include(a => a.Location)
            .ToList();
        return Json(location);
    }
}