using AutoMapper;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Core.Models.Users;
using HotelListing.LIB;

namespace HotelListing.API.Core.Configurations;
public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        CreateMap<Hotel, HotelDto>().ReverseMap();

        CreateMap<ApiUser, ApiUserDto>().ReverseMap();
        CreateMap<ApiUser, GetUserDto>().ReverseMap();
        CreateMap<ApiUser, GetDetailedUserDto>().ReverseMap();
    }
}