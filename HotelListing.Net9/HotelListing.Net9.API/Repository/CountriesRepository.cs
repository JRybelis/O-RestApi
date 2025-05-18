using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Repository;

public class CountriesRepository(HotelListingDbContext context)
    : GenericRepository<Country>(context), ICountriesRepository
{
    public async Task<Country> GetDetails(int id) =>
        await context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);
}