using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Country;

namespace HotelListing.Net9.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<CountryDto?> GetCountryDetailed(int id);
}

