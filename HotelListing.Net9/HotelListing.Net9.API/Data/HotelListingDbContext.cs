using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Data;

public class HotelListingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }
}

