using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace mnacr22.Models;

public class ApplicationUser : IdentityUser
{
    [Required] 
    [DisplayName("First Name")] 
    public string Firstname { get; set; }

    [Required] 
    [DisplayName("Last Name")] 
    public string Lastname { get; set; }

    [Required]
    [MinimumAge(18)]
    [DisplayName("Date of Birth")]
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }
    
    public ICollection<Address>? Addresses { get; set; }
    public ICollection<Car>? Cars { get; set; } 
    //public List<Parkering> Parkerings { get; set; }
}



public class MinimumAgeAttribute: ValidationAttribute
{
    int _minimumAge;

    public MinimumAgeAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    public override bool IsValid(object value)
    {
        DateTime date;
        if (DateTime.TryParse(value.ToString(),out date))
        {
            return date.AddYears(_minimumAge) < DateTime.Now;
        }

        return false;
    }
}