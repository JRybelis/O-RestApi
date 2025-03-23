using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.LIB;
using Microsoft.Extensions.Logging;

namespace HotelListing.API.Core.Repository;
public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HotelsRepository> _logger;

    public HotelsRepository(HotelListingDbContext context, IMapper mapper,
    ILogger<HotelsRepository> logger) 
    : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
    }
}