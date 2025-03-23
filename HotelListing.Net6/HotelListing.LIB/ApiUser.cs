using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.LIB;
public class ApiUser : IdentityUser
{
    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string LastName { get; set; }

    [MaxLength(256)]
    public string MiddleName { get; set; }
}