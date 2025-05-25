using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Exceptions;
using HotelListing.Net9.Models.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await countriesRepository.GetAllAsync();
        var records = mapper.Map<List<GetCountryDto>>(countries);
        
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await countriesRepository.GetDetails(id);

        if (country is null)
            throw new NotFoundException(nameof(GetCountry), id);
        
        var record = mapper.Map<CountryDto>(country);
        
        return Ok(record);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            return BadRequest("Invalid record id.");

        var country = await countriesRepository.GetAsync(id);
        var countryExists = await CountryExists(id);

        if (!countryExists)
            throw new NotFoundException(nameof(PutCountry), id);
        
        mapper.Map(updateCountryDto, country); // sets country state to modified

        try
        {
            await countriesRepository.UpdateAsync(country);
        }
        catch (Exception e)
        {
            if (!countryExists)
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = mapper.Map<Country>(createCountryDto);
        await countriesRepository.AddAsync(country);
        
        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Country>> DeleteCountry(int id)
    {
        var country = await countriesRepository.GetAsync(id);
        
        if (country is null)
            throw new NotFoundException(nameof(DeleteCountry), id);
        
        await countriesRepository.DeleteAsync(id);
        
        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await countriesRepository.ExistsAsync(id); 
    }
}