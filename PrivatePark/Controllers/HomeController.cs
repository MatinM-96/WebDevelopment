using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Versioning;

namespace mnacr22.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var cars = _db.Cars.Where(x => x.User.Contains(user));

        
        
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult Index(int addressId, DateTime time)
    {
        Console.WriteLine("\n\n" + addressId + "\n\n");
        
        var rentee = _userManager.GetUserAsync(User).Result;
        var address = _db.Addresses.FirstOrDefault(a => a.Id == addressId);
        var renter = address.User;

        Parkering park = new Parkering()
        {
            StartTime = DateTime.Now,
            Address = address,
            Renter = renter,
            //Car = car, Legge til at man kan velge hvilken bil i formen
            EndTime = time 
        };

        address.Rented = true;

        _db.Addresses.Update(address);
        _db.Parkerings.AddRange(park);
        _db.SaveChanges();
        
        return View();
    }
    
    [HttpGet]
    public IActionResult RenterHistory()
    {
        var user = _userManager.GetUserAsync(User).Result;
        
        return View();
    }

    [HttpGet]
    public IActionResult RenteeHistory()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    
    
    
    
    
    
    
    
    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}