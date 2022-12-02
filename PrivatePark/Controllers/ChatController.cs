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
        return View();
    }

    [HttpPost]
    public IActionResult Action(string username)
    {
        var user = _userManager.GetUserAsync(User).Result;
        var user2 = _userManager.FindByNameAsync(username).Result;

        if (user == user2)
        {
            return View("Error");
        }

        if (user != null && user2 != null)
        {
            return RedirectToAction("CreateChat", new {user1Id = user.Id, user2Id = user2.Id});
        }

        return View("Error");
    }

    public async Task<IActionResult> CreateChat(string user1Id, string user2Id)
    {
        var existingChat = await _db.Chats
            .SingleOrDefaultAsync(x => 
                (x.User1Id == user1Id && x.User2Id == user2Id) 
                || (x.User1Id == user2Id && x.User2Id == user1Id));

        if (existingChat != null)
        {
            return RedirectToAction("Messages", new {id = existingChat.Id});
        }

        var chat = new Chat
        {
            User1Id = user1Id,
            User2Id = user2Id
        };

        _db.Chats.Add(chat);
        await _db.SaveChangesAsync();
        return RedirectToAction("Messages", new {id = chat.Id});
    }

    public IActionResult Messages(int id)
    {
        var chat = _db.Chats.Find(id);
        
        var user1 = _userManager.GetUserAsync(User).Result;
        var user2 = _userManager.FindByIdAsync(chat.User2Id).Result;
        
        ViewData["User1Id"] = chat.User1Id;
        ViewData["User2Id"] = chat.User2Id;

        ViewData["User1Name"] = user1.Nickname;
        ViewData["User2Name"] = user2.Nickname;
        
        ViewData["User1Email"] = user1.Email;
        ViewData["User2Email"] = user2.Email;
        
        return View();
    }
}