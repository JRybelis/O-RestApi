using HotelListing.Net9.API.Core.Models.Hotel;

namespace HotelListing.Net9.API.Core.Models.Country;

public class CountryDto : BaseCountryDto
{
    public int Id { get; set; }
    public List<HotelDto> Hotels { get; set; }
}