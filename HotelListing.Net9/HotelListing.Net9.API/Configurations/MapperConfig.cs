using AutoMapper;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Country;
using HotelListing.Net9.Models.Hotel;
using HotelListing.Net9.Models.Users;

namespace HotelListing.Net9.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();
        
        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        
        CreateMap<ApiUser, CreateApiUserDto>().ReverseMap();
        CreateMap<ApiUser, GetApiUserDto>().ReverseMap();
        CreateMap<ApiUser, LoginApiUserDto>().ReverseMap();
    }
}