using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;

namespace mnacr22.Areas.Identity.Pages.Account.Manage;

public class AddParking : PageModel
{
   
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AddParking(UserManager<ApplicationUser> um, ApplicationDbContext db)
        {
            _userManager = um;
            _db = db;
        }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [DisplayName("Parking Number")]
            public int ParkingNumber { get; set; }

            [Required]
            [DisplayName("Parking Type")]
            public string ParkingType { get; set; }

            [Required]
            [DisplayName("Availability")]
            public bool Availability { get; set; }

            [Required] [DisplayName("Price")] public int Price { get; set; }

        }
        
        
        
        public async Task<IActionResult> OnPostAsync(Parkering parkering)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            
                

            parkering.Price = Input.Price;
            parkering.ParkingNumber = Input.ParkingNumber;
            parkering.ParkingType = Input.ParkingType;
           


            
            
    
            return RedirectToPage();
        }

}


