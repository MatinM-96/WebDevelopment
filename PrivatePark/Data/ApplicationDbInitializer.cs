
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                Nickname = "User1",
                Email = "user@uia.no",
                EmailConfirmed = true,
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
            Nickname = "Admin1",
            Email = "admin@uia.no",
            EmailConfirmed = true,
            DateOfBirth = new DateTime(1950, 01, 01)
        };
        um.CreateAsync(admin, "Password1.").Wait();
        await um.AddToRoleAsync(admin, "Admin");

        
        
        //city  
        string city = "Grimstad";
        const string zicode = "4879 ";
        string street = "jon lilletuns vei 2A";
        float price = 20;


        string city2 = "Grimstad";
        const string zicode2 = "4879";
        string street2 = "Jon Lilletuns vei 9";
        float price2 = 15;
        

        var address = new[]
        {
            new Address(street, city, zicode, price, false, "Car", 1),
            new Address(street2,city2,zicode2, price2, true, "Motorcycle", 2),
        };
        await db.Addresses.AddRangeAsync(address);


        address[0].User = admin;
        address[1].User = user[0];
        
        
        
        
        
        //location 
        Rootobject oRootObject;
        
        Task<string> jason_taks_string = FindLocation.GetTheLatitudeAndLongitude(city, street, zicode);
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

           new Car("AJ57220", "Car", user[0]),
           new Car("AJ57221", "motorcyscl", admin),
           new Car("AJ57222", "vareBil", user[0]),
           new Car("AJ57223", "Lastebil", user[0]),

           new Car("AJ57220", "Car", admin)

       };
       await db.Cars.AddRangeAsync(car);

       var park = new Parkering
       {
           StartTime = DateTime.Now,
           Address = address[0],
           Renter = user[0],
           Rentee = admin,
           Car = car[0],
           EndTime = DateTime.Now,
           TotalTime = DateTime.Now - DateTime.Now,
           PricePaid = 123
       };
       
       var park2 = new Parkering
       {
           StartTime = DateTime.Now,
           Address = address[0],
           Renter = user[0],
           Rentee = admin,
           Car = car[0],
           EndTime = DateTime.Now,
           TotalTime = DateTime.Now - DateTime.Now,
           PricePaid = 100
       };

       await db.Parkerings.AddRangeAsync(park);
       await db.Parkerings.AddRangeAsync(park2);
      

       await db.SaveChangesAsync();
       
    }
}


