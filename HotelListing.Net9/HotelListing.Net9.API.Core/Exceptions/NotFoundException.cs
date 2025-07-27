namespace HotelListing.Net9.API.Core.Exceptions;

public class NotFoundException(string name, object? key)
    : ApplicationException(string.Format("{0} ({1}) was not found.", name, key))
{
    
}