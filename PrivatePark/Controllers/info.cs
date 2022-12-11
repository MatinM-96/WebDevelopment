using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;
using Newtonsoft.Json;

namespace mnacr22.Controllers;

public class Info : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;

    public Info(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um; 
    }
    
    public JsonResult GetAddressUser()
    {
        var location = _db.Addresses.Where(x => x.Active == true).Include(a => a.User)
            .ToList();
        return Json(location);
    }
    

    public JsonResult GetAllParkings()
    {

        var location = _db.Addresses.Where(x => x.Active == true).Include(a => a.Location)
            .ToList();
        return Json(location);
    }
    
    
    
    [Authorize]
    public string GetAllcarForEachUser()
    {
        
        var user = _um.GetUserAsync(User).Result;
        var cars = _db.Cars.Where(x => x.User.Contains(user));
        string carsJson = JsonConvert.SerializeObject(cars);

        return (carsJson);
    }
}