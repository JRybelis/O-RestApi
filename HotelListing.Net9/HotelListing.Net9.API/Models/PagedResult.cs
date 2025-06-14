namespace HotelListing.Net9.Models;

public class PagedResult<T>
{
    public long TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int RecordNumber { get; set; }
    public List<T> Items { get; set; }
}