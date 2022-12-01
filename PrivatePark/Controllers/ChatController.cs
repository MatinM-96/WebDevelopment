using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Newtonsoft.Json;

namespace mnacr22.Controllers;

public class ChatController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    // GET
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public JsonResult GetUser()
    {
        var user = _userManager.GetUserAsync(User).Result;
        
        return Json(user);
    }
}