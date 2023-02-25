using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Country;
using HotelListing.LIB;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository;
public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    public CountriesRepository(HotelListingDbContext context, IMapper mapper) 
    : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountryDto> GetCountryDetailed(int id)
    {
        return await _context.Countries.Include(q => q.Hotels)
        .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(q => q.Id == id);
    }
}