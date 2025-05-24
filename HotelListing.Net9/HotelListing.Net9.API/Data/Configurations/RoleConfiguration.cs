using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Net9.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "6p5o4n3m-2l1k-0j9i-8h7g-6f5e4d3c2b1a",
                Name = "Super Admin",
                NormalizedName = "SUPERADMIN"
            },
            new IdentityRole
            {
                Id = "85d76b96-4c71-418d-a787-049eb8f0be79",
                Name = "Front Desk",
                NormalizedName = "FRONTDESK"
            }
        );
    }
}