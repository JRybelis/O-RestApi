using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Users;
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 15, MinimumLength = 6,
    ErrorMessage = "Your password is limited to {2} to {1} characters.")]
    public string Password { get; set; }
}