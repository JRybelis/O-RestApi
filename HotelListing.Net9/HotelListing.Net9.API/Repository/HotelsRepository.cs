using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;

namespace HotelListing.Net9.Repository;

public class HotelsRepository(HotelListingDbContext context) : GenericRepository<Hotel>(context), IHotelsRepository
{
    
}