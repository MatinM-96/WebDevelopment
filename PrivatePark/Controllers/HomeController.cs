using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;

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
        return View();
    }

    [HttpGet]
    public IActionResult Rent(int addressId)
    {
        var rentee = _userManager.GetUserAsync(User).Result;
        var address = _db.Addresses.Find(addressId);
        var renter = address.User;
        
        Parkering park = new Parkering()
        {
            StartTime = DateTime.Now,
            Address = address,
            Renter = renter,
            Rentee = rentee
        };
        
        _db.Parkerings.AddRange(park);
        _db.SaveChanges();
        
        return View();
    }
    
    [HttpPost]
    public IActionResult Rent()
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