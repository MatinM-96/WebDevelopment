using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Parkering
{
    public Parkering (){}

    public Parkering(DateTime startTime, Address address, ApplicationUser renter, Car car)
    {
        StartTime = startTime;
        Address = address;
        Renter = renter;
        Car = car;
    }
    
    
    public int Id { get; set; }

    [Required]
    [DisplayName("Start time")]
    public DateTime StartTime { get; set; }

    [Required] 
    [DisplayName("End time")] 
    public DateTime? EndTime { get; set; }
    
    public Address Address { get; set; }
    
    
    public ApplicationUser Renter { get; set; }
    
    
    public Car? Car { get; set;}

    

}