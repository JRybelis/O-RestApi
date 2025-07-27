namespace HotelListing.Net9.API.Core.Helpers;

public class JwtSettings
{
    public string Key { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string DurationInMinutes { get; init; }
}