using HotelListing.API.Core.Models.Country;
using HotelListing.LIB;

namespace HotelListing.API.Core.Contracts;
public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<CountryDto> GetCountryDetailed(int id);
}