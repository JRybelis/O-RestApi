using AutoMapper;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Exceptions;
using HotelListing.Net9.API.Core.Models.Hotel;

namespace HotelListing.Net9.API.Core.Repository;

public class HotelsRepository(HotelListingDbContext context, IMapper mapper)
    : GenericRepository<Hotel>(context, mapper), IHotelsRepository
{
    public async Task<HotelDto> GetHotelByIdAsync(int? id)
    {
        if (id is null)
            throw new NotFoundException(nameof(GetHotelByIdAsync), id);

        var hotel = await context.Hotels.FindAsync(id);
        
        return mapper.Map<HotelDto>(hotel);
    }
}