using System.ComponentModel.DataAnnotations;

namespace HotelListing.Net9.Models.Hotel;

public abstract class BaseHotelDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    public double? Rating { get; set; }
    
    [Required]
    [Range (1, 200)]
    public int CountryId { get; set; }
}