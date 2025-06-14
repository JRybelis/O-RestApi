using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Hotel;

namespace HotelListing.Net9.Contracts;

public interface IHotelsRepository : IGenericRepository<Hotel>
{
    Task<HotelDto> GetHotelByIdAsync(int? id);
}