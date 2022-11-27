using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Address
{
    
    public Address (){}

    public Address(string street, string city, string ziptCode )
    {
        Street = street;
        City = city;
        ZiptCode = ziptCode;
        
    }
    

    public int Id {get; set;}
    
    [Required]
    [DisplayName("Street")]
    public string Street {get; set;}
    
    
    [Required]
    [DisplayName("Zip code")]
    public string ZiptCode {get; set;}
    
    [Required]
    [DisplayName("City")]
    public string City {get; set;}
    
    
    
   
    //public string ApplicationUserId { get; set;  }
    public ICollection<ApplicationUser?> User { get; set; }
    public Location Location { get; set;} 
    
    
    

}