using HotelListing.API.Core.Models.Hotel;
using HotelListing.LIB;

namespace HotelListing.API.Core.Contracts;
public interface IHotelsRepository : IGenericRepository<Hotel>
{
    Task<HotelDto> GetHotel(int? id);
}