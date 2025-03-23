using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Data;

public class HotelListingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Japan",
                ISOCode = "JPN"
            },
            new Country
            {
                Id = 2,
                Name = "Slovakia",
                ISOCode = "SVK"
            });

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Mariott",
                Address = "96 Shinto street, Osaka-ku",
                Rating = 4.5,
                CountryId = 1,
            },
            new Hotel
            {
                Id = 2,
                Name = "NH",
                Address = "97 Slava ul., Bratislava",
                Rating = 3.5,
                CountryId = 2
            });
    }
}

