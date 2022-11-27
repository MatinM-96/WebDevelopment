using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mnacr22.Data;
using mnacr22.Models;
using mnacr22.Services;
using Newtonsoft.Json;

namespace mnacr22.Areas.Identity.Pages.Account.Manage;

public class Youraddresses: PageModel
{
    private readonly UserManager<ApplicationUser> _um;
    private readonly ApplicationDbContext _db;

    public Youraddresses(UserManager<ApplicationUser> um, ApplicationDbContext db)
    {
        _um = um;
        _db = db;
    }

    
    public IEnumerable<Address> DisplayAddresses { get; set; }
    [BindProperty] 
    public InputModel Input { get; set; }

    public class InputModel
    {
        public int Id {get; set;}
        
        
        [Required] 
        [Display(Name = "Street")] 
        public string Street { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public string ZiptCode { get; set; }

        [Required] 
        [Display(Name = "City")] 
        public string City { get; set; }
    }
    
    public void OnGetAsync()
    {
        var user = _um.GetUserAsync(User).Result;
        DisplayAddresses = _db.Addresses.ToList();
        
    }
   
    
    
    public async Task<IActionResult> OnPostAsync(string buttonType)
    {
        var user = await _um.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_um.GetUserId(User)}'.");
        }

        int id = Input.Id;
        if (id == null)
        {
            Console.WriteLine("Id is null");
            return Redirect("./SomeError");
        }
        
        Address address = _db.Addresses.Find(id);

        address.Id = id;
        address.Street = Input.Street;
        address.ZiptCode = Input.ZiptCode;
        address.City = Input.City;
        address.User = new[] {user};
        Location loc = coordinates(address);
        
        
        

        address.Location = loc;
        
        
        
        

        if (buttonType == "Update")
        {
            Console.WriteLine("\n\nUpdating information...\n");
            _db.Entry(address).State = EntityState.Modified;
            await _db.SaveChangesAsync();  
        }
        else if (buttonType == "Delete")
        {
            Console.WriteLine("\n\nDeleting from database...\n");
            _db.Entry(address).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }
        
        return RedirectToPage();
    }
    
    
    
    public Location coordinates(Address address)
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
