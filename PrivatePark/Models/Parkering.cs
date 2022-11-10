using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mnacr22.Models;

public class Parkering
{
    public Parkering (){}

    public Parkering(int parkingNumber, string parkingType, bool availability, int price, DateTime date)
    {
        ParkingNumber = parkingNumber;
        ParkingType = parkingType;
        Availability = availability;
        Price = price;
        Date = date;
        
    }
    
    
    public int Id { get; set; }
    
    [Required]
    [DisplayName("Parking Number")]
    public int ParkingNumber {get; set;}
    
    [Required]
    [DisplayName("Parking Type")]
    public string ParkingType {get; set;}
    
    [Required]
    [DisplayName("Availability")]
    public bool Availability {get; set;}
    
    [Required]
    [DisplayName("Price")]
    public int Price {get; set;}
    
    [Required]
    [DisplayName("Time")]
    public DateTime Date {get; set;}
    
    //public int AddressId { get; set;}
    public Address Address { get; set; }
    public ApplicationUser User { get; set; }
    
    
    public Car? car { get; set;}

    

}