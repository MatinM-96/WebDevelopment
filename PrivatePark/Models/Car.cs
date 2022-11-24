using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NuGet.Packaging;

namespace mnacr22.Models;

public class Car
{
    
    public Car (){}

    public Car (string registrationNumber, string carType, ApplicationUser user)
    {
        RegistrationNumber = registrationNumber;
        CarType = carType;
        User = user;
    }
    
    public int Id { get; set; }

    [Required]
    [DisplayName("Registration Number")]
    public string RegistrationNumber { get; set; }
    
    [Required]
    [DisplayName("Car Type")]
    public string CarType { get; set; }
    
    //public string ApplicationUserId { get; set;  }
    public ApplicationUser User { get; set; }
    //Nødvendig med ICollection?? Må i såfall ikke være mulig å opprette to like biler + mulighet for å søke etter bil i databasen
    
}