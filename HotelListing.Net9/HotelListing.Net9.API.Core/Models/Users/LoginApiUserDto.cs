using System.ComponentModel.DataAnnotations;

namespace HotelListing.Net9.API.Core.Models.Users;

public class LoginApiUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(25, MinimumLength = 14, ErrorMessage = "Your password must be between {2} and {1} characters long.")]
    public string Password { get; set; }
}