using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mnacr22.Models;

namespace mnacr22.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Car> Cars => Set<Car>();
    
    
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Parkering> Parkerings => Set<Parkering>();
    
    
    
    
}