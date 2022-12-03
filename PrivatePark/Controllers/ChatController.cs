using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mnacr22.Models;
using mnacr22.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        var user = _userManager.GetUserAsync(User).Result;
        
        ViewData["User1Id"] = user.Id;
        ViewData["User1Name"] = user.Nickname;
        ViewData["User1Email"] = user.Email;

        if (ViewData["User2Id"] != null)
        {
            var user2 = _userManager.FindByIdAsync(ViewData["User2Id"].ToString()).Result;

            ViewData["User2Id"] = user2.Id;
            ViewData["User2Name"] = user2.Nickname;
            ViewData["User2Email"] = user2.Email;

            return View("Index");
        }
        
        return View();
    }

    [HttpPost]
    public IActionResult Index(string username)
    {
        var user = _userManager.GetUserAsync(User).Result;
        var user2 = _userManager.FindByNameAsync(username).Result;

        if (user2 == user)
        {
            return View("Error");
        }

        if (user2 != null)
        {
            ViewData["User1Id"] = user.Id;
            ViewData["User1Name"] = user.Nickname;
            ViewData["User1Email"] = user.Email;
            
            ViewData["User2Id"] = user2.Id;
            ViewData["User2Name"] = user2.Nickname;
            ViewData["User2Email"] = user2.Email;

            return View("Index");
        }

        return View("Error");
    }
}