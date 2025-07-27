using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Models.Country;
using HotelListing.Net9.API.Core.Models.Hotel;
using HotelListing.Net9.API.Core.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HotelListingApiUnitTests;

public class HotelsRepositoryUnitTests : IDisposable, IAsyncDisposable
{
    private readonly HotelListingDbContext _context;
    private readonly SqliteConnection _connection;
    private IMapper _mapper;
    private readonly GenericRepository<Hotel> _repository;

    public HotelsRepositoryUnitTests()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        
        var options = new DbContextOptionsBuilder<HotelListingDbContext>()
            .UseSqlite(connection)
            .Options;
        
        _context = new HotelListingDbContext(options);
        _context.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Country, CreateCountryDto>().ReverseMap();
            cfg.CreateMap<Country, GetCountryDto>().ReverseMap();
            cfg.CreateMap<Country, CountryDto>().ReverseMap();
            cfg.CreateMap<Country, UpdateCountryDto>().ReverseMap();
            cfg.CreateMap<Hotel, HotelDto>().ReverseMap();
            cfg.CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        });
        _mapper = config.CreateMapper();
    
        _repository = new GenericRepository<Hotel>(_context, _mapper);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    [Theory]
    [InlineData(1, true, "Mariott", 1)]
    [InlineData(2, true, "NH", 2)]
    [InlineData(999, false, null, null)] // returns null when Id does not exist
    [InlineData(-1, false, null, null)] // handles edge cases - negative id
    [InlineData(0, false, null, null)] // handles edge cases - id zero
    public async Task GetAsync_WithVariousIds_ReturnsExpectedResult(int id, bool shouldExist, string? expectedName, int expectedCountryId)
    {
        // Act 
        var result = await _repository.GetAsync(id);
        
        // Assert
        if (shouldExist)
        {
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Name.Should().Be(expectedName);
            result.CountryId.Should().Be(expectedCountryId);
        }
        else
        {
            result.Should().BeNull();
        }
    }
}