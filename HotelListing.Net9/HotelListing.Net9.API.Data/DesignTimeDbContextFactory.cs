using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext> 
{
    public HotelListingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var builder = new DbContextOptionsBuilder<HotelListingDbContext>();
        var connectionString = configuration.GetConnectionString("HotelListingDbConnectionString");
        
        builder.UseSqlServer(connectionString);
        
        return new HotelListingDbContext(builder.Options);
    }
}