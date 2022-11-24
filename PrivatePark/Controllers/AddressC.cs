using Microsoft.AspNetCore.Mvc;
using mnacr22.Data;
using mnacr22.Models;

namespace mnacr22.Controllers;

public class AddressC : Controller
{
    // GET
    private readonly ApplicationDbContext _db;

    public AddressC(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult AddressV()
    {
        
        return View();
        
    }

    
}