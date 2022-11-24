
using Microsoft.AspNetCore.Identity;
using mnacr22.Models;
using mnacr22.Services;
using Newtonsoft.Json;

namespace mnacr22.Data;

public class ApplicationDbInitializer
{
    
    public static async Task Initializer(ApplicationDbContext db, UserManager<ApplicationUser> um,
        RoleManager<IdentityRole> rm)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var adminRole = new IdentityRole("Admin");
        var renterRole = new IdentityRole("Renter");
        var renteeRole = new IdentityRole("Rentee");
        var bothRoles = new IdentityRole("Both");
        rm.CreateAsync(adminRole).Wait();
        rm.CreateAsync(renterRole).Wait();
        rm.CreateAsync(renteeRole).Wait();
        rm.CreateAsync(bothRoles).Wait();


        var user = new[]
        {
            new ApplicationUser
            {
                Firstname = "Test",
                Lastname = "User",
                UserName = "user@uia.no",
                Email = "user@uia.no",
                EmailConfirmed = true,
                PersonNummer = 13259696951,
                DateOfBirth = new DateTime(1996, 08, 12)
            }
        };
        um.CreateAsync(user[0], "Password1.").Wait();
        await um.AddToRoleAsync(user[0], "Both");


        // Admin
        var admin = new ApplicationUser()
        {
            Firstname = "Admin",
            Lastname = "Admin",
            UserName = "admin@uia.no",
            Email = "admin@uia.no",
            EmailConfirmed = true,
            PersonNummer = 123456,
            DateOfBirth = new DateTime(1950, 01, 01)
        };
        um.CreateAsync(admin, "Password1.").Wait();
        await um.AddToRoleAsync(admin, "Admin");

        
        
        //city  
        string city = "Mo i Rana";
        const string zicode = "8622";
        string street = "Vikabakken 13 ";

        var address = new[]
        {
            new Address(street, city, zicode)
        };
        await db.Addresses.AddRangeAsync(address);
         
        address[0].User = user;

        
      
        
        
        
        
        
        //location 
        Rootobject oRootObject;
        
        Task<string> jason_taks_string = FindLocation.GetTheLatitudeAndLongitude(city, street, zicode);
        string jason = jason_taks_string.Result;
        
        oRootObject = JsonConvert.DeserializeObject<Rootobject>(jason);

        if (oRootObject != null)
        {
            Location loc = oRootObject.results[0].Geometry.location;
            db.Locations.AddRange(loc);
            loc.Address = address[0];
            
        }

         
     
        
        
        //Car 
        var car = new[]
       {
           new Car("aj57220", "PersonBil", user[0])
       };
       await db.Cars.AddRangeAsync(car);
        

        //Parking  
       var parkering = new[]
       {
           new Parkering(2,
               "personbil",
               true,
               20,
               new DateTime(2000,12,2)),
         
       };
       
       
       await db.Parkerings.AddRangeAsync(parkering); 
      
       
       
       
       // assigning relationships between the entities  
       address[0].Parkering = new List<Parkering> {parkering[0]};
       
       user[0].Addresses = address;
       
       parkering[0].Address = address[0];
       parkering[0].User = user[0];
       parkering[0].car = car[0];
       
       
        //Check if the parking is available 
       for (int i = 0; i < parkering.Length; i++)
       {
           if (parkering[i].car == null)
           {
               parkering[i].Availability = true;
           }
           else
           {
               parkering[i].Availability = false;
           }
       }
       

       await db.SaveChangesAsync();
       
    }
}


