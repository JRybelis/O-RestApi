using HotelListing.Net9.Data;

namespace HotelListing.Net9.Contracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<Country> GetDetails(int id);
}

