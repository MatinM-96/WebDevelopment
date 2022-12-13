using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mnacr22.Services;
using Newtonsoft.Json;
using NuGet.Versioning;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;
using Microsoft.Extensions.Options;

namespace mnacr22.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager, IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
        Options = optionsAccessor.Value;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var check = _db.Parkerings.Include(a => a.Address);
        foreach (var park in check)
        {
            if (park.EndTime < DateTime.Now && !park.CheckFinished)
            {
                var address = _db.Addresses.FirstOrDefault(x => x.Id == park.Address.Id);

                if (address.Quantity < address.MaxQuantity)
                {
                    address.Quantity++;
                    park.CheckFinished = true;

                    _db.Addresses.Update(address);
                    _db.SaveChanges();
                }
            }
        }
        
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Renter, Admin, Both")]
    public IActionResult RenterHistory()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Rentee, Admin, Both")]
    public IActionResult RenteeHistory()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public AuthMessageSenderOptions Options { get; }
    
    [HttpPost]
    [Authorize]
    public IActionResult CreatePayment(int addressId, string car, DateTime time)
    {
        Console.WriteLine("\n\n" + addressId + "\n\n");
        
        var address = _db.Addresses.FirstOrDefault(a => a.Id == addressId);
        var user = _userManager.GetUserAsync(User).Result;

        if (time < DateTime.Now || user == null)
        {
            return RedirectToAction("Error");
        }

        TimeSpan totalTime = time - DateTime.Now;

        var priceToPay = (long)(totalTime.TotalHours * address.Price);
        
        StripeConfiguration.ApiKey = Options.StripeKey;
        
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

        TempData["addressId"] = addressId;
        string endTime = time.ToString("yy-MM-dd HH:mm:ss");
        TempData["time"] = endTime;
        TempData["pricePaid"] = (int)priceToPay;
        TempData["car"] = car;
        
        return new StatusCodeResult(303);
    }

    [HttpGet]
    public IActionResult Success()
    {
        int addressId = (int)TempData["addressId"];
        string timeString = (string)TempData["time"];
        int pricePaid = (int)TempData["pricePaid"];
        string car = (string) TempData["car"];

        DateTime time = DateTime.ParseExact(timeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        
        var rentee = _userManager.GetUserAsync(User).Result;
        var address = _db.Addresses.FirstOrDefault(a => a.Id == addressId);
        var test = _db.Addresses.Where(b => b.Id == addressId).Include(c => c.User).FirstOrDefault();
        var renter = test.User;

        Parkering park = new Parkering
        {
            StartTime = DateTime.Now,
            Address = address,
            Renter = renter,
            Rentee = rentee,
            Car = car,
            EndTime = time, 
            TotalTime = time - DateTime.Now,
            PricePaid = pricePaid
        };
        
        test.Quantity -= 1;
        
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