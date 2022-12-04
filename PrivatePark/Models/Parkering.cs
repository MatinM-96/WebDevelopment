using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Parkering
{
    public Parkering (){}

    public Parkering(DateTime startTime, Address address, Car car)
    {
        StartTime = startTime;
        Address = address;
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
    
    
    //Den som leier ut, eller den som leier?
    public ApplicationUser Renter { get; set; }
    
    public ApplicationUser Rentee { get; set; }
    
    public Car? Car { get; set;}

    

}