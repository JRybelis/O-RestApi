using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Exceptions;
using HotelListing.Net9.API.Core.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.API.Core.Repository;

public class CountriesRepository(HotelListingDbContext context, IMapper mapper)
    : GenericRepository<Country>(context, mapper), ICountriesRepository
{
    public async Task<CountryDto?> GetCountryDetailed(int id)
    {
        var country = await context.Countries.Include(q => q.Hotels)
            .ProjectTo<CountryDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (country is null)
            throw new NotFoundException(nameof(GetCountryDetailed), id);

        return country;
    }
}