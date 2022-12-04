using System.ComponentModel.DataAnnotations;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;
using mnacr22.Services;
using Newtonsoft.Json;

namespace mnacr22.Areas.Identity.Pages.Account.Manage;

public class AddAddress : PageModel
{
    private readonly UserManager<ApplicationUser> _um;
    private readonly ApplicationDbContext _db;

    public AddAddress(UserManager<ApplicationUser> um, ApplicationDbContext db)
    {
        _um= um;
        _db = db;
    }
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }
            
        [Required]
        [Display(Name = "Zip code")]
        public string ZiptCode { get; set; } 
       
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required] 
        [Display(Name = "Price")] 
        public float Price { get; set; }
        
        [Required]
        [Display(Name = "Active")]
        public bool Active { get; set; }
        
        [Required]
        [Display(Name = "Suitability")]
        public string Suitability { get; set; }
    }
    
    public async Task<IActionResult> OnPostAsync(Address address)
    {
        var user = _um.GetUserAsync(User).Result;
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_um.GetUserId(User)}'.");
        }

        var checkDb = _db.Addresses
            .Where(x => x.Street == Input.Street && x.ZiptCode == Input.ZiptCode && x.City == Input.City)
            .ToList();

        var count = checkDb.Count;

        if (count > 0)
        {
            address = _db.Addresses.Find(checkDb[0].Id);
            address.User = user;
            _db.Entry(address).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        else
        {
            address.City = Input.City;
            address.Street = Input.Street;
            address.ZiptCode = Input.ZiptCode;
            address.Price = Input.Price;
            address.Active = Input.Active;
            address.Suitability = Input.Suitability;

            Location loc = coordinates(address);  
        
            address.Location = loc;
            address.User = user;
        
            _db.Addresses.AddRange(address);
            await _db.SaveChangesAsync();
        }
        
        return RedirectToPage();
    }



    public static Location coordinates(Address address)
    {
        Rootobject? oRootObject;

        Task<string> jason_taks_string =
            FindLocation.GetTheLatitudeAndLongitude(address.City, address.Street, address.ZiptCode);
        string jason = jason_taks_string.Result;

        oRootObject = JsonConvert.DeserializeObject<Rootobject>(jason);

        Location loc = new Location();

        if (oRootObject != null)
        {
            loc = oRootObject.results[0].Geometry.location;
        }

        return loc; 
    }


      

    
    
}