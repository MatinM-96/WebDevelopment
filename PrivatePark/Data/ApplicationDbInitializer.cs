
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
        string city = "Grimstad";
        const string zicode = "4879 ";
        string street = "jon lilletuns vei 2A";
        
        
        
        string city2 = "Grimstad";
        const string zicode2 = "4879";
        string street2 = "Jon Lilletuns vei 9";

        var address = new[]
        {
            new Address(street, city, zicode),
            new Address(street2,city2,zicode2),
        };
        await  db.Addresses.AddRangeAsync(address);
         
        address[0].User = new[] {user[0], admin};
        address[1].User = new[] {user[0]};

        //address[0].User.Add(user[0]); 
        //address[1].User.Add(admin);


        
      
        
        
        
        
        
        //locatino 
        Rootobject oRootObject;
        
        Task<string> jason_taks_string=  FindLocation.GetTheLatitudeAndLongitude(city, street, zicode);
        string jason = jason_taks_string.Result;
        
        oRootObject = JsonConvert.DeserializeObject<Rootobject>(jason);

        Location loc = new Location(); 
        
        if (oRootObject != null)
        {
            loc = oRootObject.results[0].Geometry.location;
            db.Locations.AddRange(loc);
            //loc.Address = address[0];
            
        }
        
        
        
        
        Rootobject oRootObject2;
        
        Task<string> jason_taks_string2=  FindLocation.GetTheLatitudeAndLongitude(city2, street2, zicode2);
        string jason2 = jason_taks_string2.Result;
        
        oRootObject2 = JsonConvert.DeserializeObject<Rootobject>(jason2);

        Location loc2 = new Location(); 
        
        if (oRootObject2 != null)
        {
            loc2 = oRootObject2.results[0].Geometry.location;
            db.Locations.AddRange(loc2);
            //loc.Address = address[0];
            
        }

        address[0].Location = loc;
        address[1].Location = loc2; 




        //Car 
        var car = new[]
       {
           new Car("aj57220", "PersonBil"),
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
           
           new Parkering(1,
               "Varebil",
               false ,
               20,
               new DateTime(2000,12,2)),
         
       };
       
       
       await db.Parkerings.AddRangeAsync(parkering); 
      
       
       
       
       // assigning relationships between the entities  
       //address[0].Parkering = new List<Parkering> {parkering[0]};
       
       loc.Parkering = new List<Parkering> {parkering[0]};
       loc2.Parkering = new List<Parkering> {parkering[1]};
       user[0].Addresses = address;
       
       parkering[0].Location = loc;
       parkering[0].User = user[0];
       parkering[0].car = car[0];

       parkering[1].Location = loc2;
       parkering[1].User = user[0];
       
       
       
       
       
       
       
       
       
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


