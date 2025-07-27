namespace HotelListing.Net9.API.Core.Models.Users;

public class AuthResponseDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}