using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Contracts;
public interface IHotelsRepository : IGenericRepository<Hotel>
{
    Task<HotelDto> GetHotel(int? id);
}