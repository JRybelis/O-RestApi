using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.LIB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelListing.API.Core.Repository;
public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CountriesRepository> _logger;

    public CountriesRepository(HotelListingDbContext context, IMapper mapper,
    ILogger<CountriesRepository> logger) 
    : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CountryDto> GetCountryDetailed(int id)
    {
        _logger.LogInformation($"Looking {nameof(Country)} {id} up.");
        
        var country = await _context.Countries.Include(q => q.Hotels)
        .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(q => q.Id == id);

        if (country is null)
            throw new NotFoundException(nameof(GetCountryDetailed), id);

        return country;
    }
}