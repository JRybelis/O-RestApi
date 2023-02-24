using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users;
public class ApiUserDto
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    public string MiddleName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 15, MinimumLength = 6, 
    ErrorMessage = "Your password is limited to {2} to {1} caracters.")]
    public string Password { get; set; }
}