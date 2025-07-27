using HotelListing.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.Data;

public class HotelListingDbContext(DbContextOptions<HotelListingDbContext> options)
    : IdentityDbContext<ApiUser>(options)
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country?> Countries { get; set; }
    public new DbSet<ApiUser> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
    }
}

public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
{
    public HotelListingDbContext CreateDbContext(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<HotelListingDbContext>();
        var conn = config.GetConnectionString("HotelListingDbConnectionString");
        optionsBuilder.UseSqlServer(conn);
        
        return new HotelListingDbContext(optionsBuilder.Options);
    }
}