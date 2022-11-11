using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Address
{
    
    public Address (){}

    public Address(string street, string city, int ziptCode )
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
    [DisplayName("City")]
    public string City {get; set;}
    
    [Required]
    [DisplayName("Zip code")]
    public int ZiptCode {get; set;}
    
   
    //public string ApplicationUserId { get; set;  }
    public ICollection<ApplicationUser?> User { get; set; }
    
    public List<Parkering> Parkering { get; set; }
    
    

}