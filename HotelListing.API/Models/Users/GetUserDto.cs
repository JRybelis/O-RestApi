namespace HotelListing.API.Models.Users;
public class GetUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NormalizedEmail { get; set; }
    public string PhoneNumber { get; set; }
}