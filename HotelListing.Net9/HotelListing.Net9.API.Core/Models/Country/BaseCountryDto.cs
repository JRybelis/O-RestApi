using System.ComponentModel.DataAnnotations;

namespace HotelListing.Net9.API.Core.Models.Country;

public abstract class BaseCountryDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string ISOCode { get; set; }
}