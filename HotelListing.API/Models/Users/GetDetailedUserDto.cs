namespace HotelListing.API.Models.Users;
public class GetDetailedUserDto : GetUserDto
{
    public string MiddleName { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool EmailConfirmed { get; set; }
    public string NormalizedUsername { get; set; }
}