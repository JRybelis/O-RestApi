using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;
public class LookupUsersByNameDto
{
    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(256)]
    public string LastName { get; set; }
}