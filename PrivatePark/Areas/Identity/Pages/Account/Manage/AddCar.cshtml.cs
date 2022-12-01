using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;
using NuGet.Packaging;

namespace mnacr22.Areas.Identity.Pages.Account.Manage;

public class AddCarModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public AddCarModel(UserManager<ApplicationUser> um, ApplicationDbContext db)
    {
        _userManager = um;
        _db = db;
    }
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }
            
        [Required]
        [Display(Name = "Car type")]
        public string CarType { get; set; }
    }
    
    
    public async Task<IActionResult> OnPostAsync(Car car)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var checkDb = _db.Cars.Where(x => x.RegistrationNumber == Input.RegistrationNumber).ToList();
        var test = checkDb.Count;
        
        if (test > 0)
        {
            car = _db.Cars.Find(checkDb[0].Id);

            car.User = new[] {user};
            _db.Entry(car).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        else
        {
            car.RegistrationNumber = Input.RegistrationNumber;
            car.CarType = Input.CarType;
            car.User = new[] {user};

            _db.Cars.AddRange(car);
            await _db.SaveChangesAsync();
        }
        
        return RedirectToPage();
    }
}