using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Net9.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
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
            }
        );
    }
}