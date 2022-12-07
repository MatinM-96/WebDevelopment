using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Versioning;
using Stripe;
using Stripe.Checkout;

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
            EndTime = time, 
            TotalTime = time - DateTime.Now
        };

        if (park.EndTime < park.StartTime)
        {
            return RedirectToPage("Error");
        }

        var price = (long)(park.TotalTime.TotalHours * address.Price);

        address.Rented = true;

        _db.Addresses.Update(address);
        _db.Parkerings.AddRange(park);
        _db.SaveChanges(); // Endre slik at endringene blir bekreftet etter at betalingen er bekreftet
        
        return RedirectToAction("CreatePayment", new {priceToPay = price});
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

    public IActionResult CreatePayment(long priceToPay)
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

        return new StatusCodeResult(303);
    }

    [HttpGet]
    public IActionResult Success()
    {
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