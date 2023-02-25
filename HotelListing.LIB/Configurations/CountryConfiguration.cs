using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.LIB.Configurations;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
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
            }
        );
    }
}