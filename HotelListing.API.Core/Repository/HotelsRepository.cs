using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.LIB;

namespace HotelListing.API.Core.Repository;
public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public HotelsRepository(HotelListingDbContext context, IMapper mapper) 
    : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HotelDto> GetHotel(int? id)
    {
        if (id is null)
            throw new BadRequestException(nameof(GetAsync), id);

        var hotel = await _context.Hotels.FindAsync(id);

        return _mapper.Map<HotelDto>(hotel);
    }
}