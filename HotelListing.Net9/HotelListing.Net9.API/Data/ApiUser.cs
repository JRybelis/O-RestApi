using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.Data;

public class ApiUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string[] Roles { get; set; }
}