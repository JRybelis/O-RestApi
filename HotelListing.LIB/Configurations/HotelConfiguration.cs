using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.LIB.Configurations;
public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
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
            }
        );
    }
}