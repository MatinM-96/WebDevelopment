using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using mnacr22.Models;

namespace mnacr22.Data;

public class ApplicationDbInitializer
{
    public static void Initializer(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
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
        um.AddToRoleAsync(user[0], "Both");


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
        um.AddToRoleAsync(admin, "Admin");
        
        

        var address = new []
              {
                  new Address ("Jon lilletuns vei 2A", "Grimstd", 123),
              };
       db.Addresses.AddRange(address);

       

       var car = new[]
       {
           new Car("aj57220", "PersonBil"),
       };
       db.Cars.AddRange(car);


       var parkering = new[]
       {
           new Parkering(2, "personbil", true, 20, new DateTime(2000,12,2)),
         
       };
       
       
       db.Parkerings.AddRange(parkering); 
      
       address[0].Parkering = new List<Parkering> {parkering[0]};
       
       user[0].Addresses = address;
       address[0].User = user; 
       parkering[0].Address = address[0];
       parkering[0].User = user[0];
       parkering[0].car = car[0];
       
       

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
       

       db.SaveChanges();
       
    }
}