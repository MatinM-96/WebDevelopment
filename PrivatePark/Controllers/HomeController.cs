using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Versioning;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;

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
        var check = _db.Parkerings.Include(a => a.Address);
        foreach (var park in check)
        {
            if (park.EndTime < DateTime.Now)
            {
                var address = _db.Addresses.FirstOrDefault(x => x.Id == park.Address.Id);

                if (address.Quantity < address.MaxQuantity)
                {
                    address.Quantity++;

                    _db.Addresses.Update(address);
                    _db.SaveChanges();   
                }
            }
        }
        
        return View();
    }
   


    [HttpPost]
    [Authorize]
    public IActionResult Index(int addressId, DateTime time)
    {
        Console.WriteLine("\n\n" + addressId + "\n\n");
        
        var address = _db.Addresses.FirstOrDefault(a => a.Id == addressId);
        var user = _userManager.GetUserAsync(User).Result;

        if (time < DateTime.Now || user == null)
        {
            return RedirectToAction("Error");
        }

        TimeSpan tTime = time - DateTime.Now;

        var price = (long)(tTime.TotalHours * address.Price);

        return RedirectToAction("CreatePayment", new {priceToPay = price, aId = addressId, t = time});
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult RenterHistory()
    {
        var user = _userManager.GetUserAsync(User).Result;
        
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult RenteeHistory()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public IActionResult CreatePayment(long priceToPay, int aId, DateTime t)
    {
        StripeConfiguration.ApiKey =
            "sk_test_51M1RCrH7GdmlJf4XKjrBTbzb9OZnX8gGIV28NeUUDcJ50Exbw4iJTEjy5LUTIhInLuACZcJg7vT3qZoJ5EkA9QEI00MJJSOwuC";
        
        var domain = "https://localhost:7034";

        if (priceToPay < 3)
        {
            priceToPay = 3;
        }
        
        var pOptions = new PriceCreateOptions
        {
            UnitAmount = priceToPay * 100,
            Currency = "nok",
            Product = "prod_MvELkaTH4SLlUO"
        };

        var pService = new PriceService();
        Price price = pService.Create(pOptions);

        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = price.Id,
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = domain + "/Home/Success",
            CancelUrl = domain + "/Home/Cancel"
        };

        var service = new SessionService();
        Session session = service.Create(options);
        
        Response.Headers.Add("Location", session.Url);

        TempData["addressId"] = aId;
        string time = t.ToString("yy-MM-dd HH:mm:ss");
        TempData["time"] = time;
        TempData["pricePaid"] = (int)priceToPay;
        
        return new StatusCodeResult(303);
    }

    [HttpGet]
    public IActionResult Success()
    {
        int addressId = (int)TempData["addressId"];
        string timeString = (string)TempData["time"];
        int pricePaid = (int)TempData["pricePaid"];

        DateTime time = DateTime.ParseExact(timeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        
        var rentee = _userManager.GetUserAsync(User).Result;
        var address = _db.Addresses.FirstOrDefault(a => a.Id == addressId);
        var user = _db.Addresses.Where(b => b.Id == addressId).Include(c => c.User).FirstOrDefault();
        var renter = user.User;

        Parkering park = new Parkering
        {
            StartTime = DateTime.Now,
            Address = address,
            Renter = renter,
            Rentee = rentee,
            //Car = car,
            EndTime = time, 
            TotalTime = time - DateTime.Now,
            PricePaid = pricePaid
        };
        
        address.Quantity -= 1;
        
        _db.Addresses.Update(address);
        _db.Parkerings.AddRange(park);
        _db.SaveChanges();

        ViewBag.ParkId = park.Id;
        
        return View();
    }

    [HttpGet]
    public IActionResult Cancel()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}