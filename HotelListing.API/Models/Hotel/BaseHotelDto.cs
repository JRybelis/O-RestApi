using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Hotel
{
    public class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Rating { get; set; }
    }
}