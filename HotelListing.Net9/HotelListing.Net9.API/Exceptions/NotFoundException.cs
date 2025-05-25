namespace HotelListing.Net9.Exceptions;

public class NotFoundException(string name, object key)
    : ApplicationException(string.Format("{1} ({2}) was not found.", name, key))
{
    
}