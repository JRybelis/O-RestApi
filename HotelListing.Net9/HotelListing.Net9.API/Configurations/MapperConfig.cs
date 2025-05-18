using AutoMapper;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Country;
using HotelListing.Net9.Models.Hotel;

namespace HotelListing.Net9.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<Country, UpdateCountryDto>().ReverseMap();
        
    }
}