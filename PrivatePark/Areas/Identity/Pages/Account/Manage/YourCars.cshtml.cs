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

public class YourCarsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public YourCarsModel(UserManager<ApplicationUser> um, ApplicationDbContext db)
    {
        _userManager = um;
        _db = db;
    }
    

    public IEnumerable<Car> DisplayCars { get; set; }
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }
            
        [Required]
        [Display(Name = "Car type")]
        public string CarType { get; set; }
    }


    public void OnGetAsync()
    {
        var user = _userManager.GetUserAsync(User).Result;
        DisplayCars = _db.Cars.Where(x => x.User == user).ToList();
    }
    
    
    public async Task<IActionResult> OnPostAsync(string buttonType)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        int id = Input.Id;
        if (id == null)
        {
            Console.WriteLine("Id is null");
            return Redirect("./SomeError");
        }
        
        Car car = _db.Cars.Find(id);

        car.Id = id;
        car.RegistrationNumber = Input.RegistrationNumber;
        car.CarType = Input.CarType;
        car.User = user;

        if (buttonType == "Update")
        {
            Console.WriteLine("\n\nUpdating information...\n");
            _db.Entry(car).State = EntityState.Modified;
            await _db.SaveChangesAsync();  
        }
        else if (buttonType == "Delete")
        {
            Console.WriteLine("\n\nDeleting from database...\n");
            _db.Entry(car).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }
        
        return RedirectToPage();
    }
}