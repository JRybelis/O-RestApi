namespace HotelListing.API.Models.Hotel;
public class HotelDto : BaseHotelDto
{
    public int Id { get; set; }
    public string Address { get; set; }
    public int CountryId { get; set; }
}