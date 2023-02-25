namespace HotelListing.API.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string name, object key) 
    : base($"{name} ({key}) was invalid for this operation.")
    {
        
    }
}