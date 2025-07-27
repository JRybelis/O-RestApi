using HotelListing.Data;
using HotelListing.Net9.API.Core.Models.Country;

namespace HotelListing.Net9.API.Core.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<CountryDto?> GetCountryDetailed(int id);
}

