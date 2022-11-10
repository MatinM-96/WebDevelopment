using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Car
{
    
    public Car (){}

    public Car(string registrationNumber, string carType)
    {
        RegistrationNumber = registrationNumber;
        CarType = carType;
      
    }
    public int  id { get; set; }

    [Required]
    [DisplayName("Registration Number")]
    public string RegistrationNumber {get; set;}
    
    [Required]
    [DisplayName("Car Type")]
    public string CarType {get; set;}
    
    //public string ApplicationUserId { get; set;  }
    public ICollection<ApplicationUser> User { get; set; }
    
    
}