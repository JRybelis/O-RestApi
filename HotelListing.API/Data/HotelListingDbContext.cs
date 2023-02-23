using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data;

public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public  DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "New Zealand",
                ShortName = "NZ"
            },
            new Country
            {
                Id = 2,
                Name = "Colombia",
                ShortName = "CO"
            },
            new Country
            {
                Id = 3,
                Name = "Jamaica",
                ShortName = "JM"
            },
            new Country
            {
                Id = 4,
                Name = "Italy",
                ShortName = "IT"
            },
            new Country
            {
                Id = 5,
                Name = "India",
                ShortName = "ID"
            });

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Comfort Suites",
                Address = "Bogota DC",
                CountryId = 2,
                Rating = 3.9
            },
            new Hotel
            {
                Id = 2,
                Name = "Mercure",
                Address = "Wellington",
                CountryId = 1,
                Rating = 4.3
            },
            new Hotel
            {
                Id = 3,
                Name = "Anantara Rasananda",
                Address = "Mumbai",
                CountryId = 5,
                Rating = 4.9
            },
            new Hotel
            {
                Id = 4,
                Name = "Sandals Resort and Spa",
                Address = "Negril",
                CountryId = 3,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 5,
                Name = "NH",
                Address = "Rome",
                CountryId = 4,
                Rating = 4.0
            });
    }
}