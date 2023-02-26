namespace HotelListing.API.Core.Models.Users;
public class GetDetailedUserDto : GetUserDto
{
    public string MiddleName { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool EmailConfirmed { get; set; }
    public string NormalizedUserName { get; set; }
}