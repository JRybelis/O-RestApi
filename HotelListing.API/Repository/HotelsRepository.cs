using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Exceptions;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Repository;
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