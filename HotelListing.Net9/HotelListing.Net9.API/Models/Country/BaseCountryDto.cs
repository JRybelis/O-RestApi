using Microsoft.Build.Framework;

namespace HotelListing.Net9.Models.Country;

public abstract class BaseCountryDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string ISOCode { get; set; }
}