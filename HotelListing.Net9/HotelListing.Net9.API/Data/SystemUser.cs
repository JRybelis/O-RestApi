using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.Data;

public class SystemUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}