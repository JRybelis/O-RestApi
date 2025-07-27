using HotelListing.Data;
using HotelListing.Net9.API.Core.Models.Hotel;

namespace HotelListing.Net9.API.Core.Contracts;

public interface IHotelsRepository : IGenericRepository<Hotel>
{
    Task<HotelDto> GetHotelByIdAsync(int? id);
}