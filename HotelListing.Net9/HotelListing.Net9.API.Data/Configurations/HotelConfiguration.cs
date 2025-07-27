using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
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
            }
        );
    }
}