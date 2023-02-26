using AutoMapper;
using HotelListing.API.Core.Contracts;
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
}