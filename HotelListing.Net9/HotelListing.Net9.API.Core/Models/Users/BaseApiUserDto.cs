using System.ComponentModel.DataAnnotations;

namespace HotelListing.Net9.API.Core.Models.Users;

public class BaseApiUserDto
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public string[] Roles { get; set; }
}