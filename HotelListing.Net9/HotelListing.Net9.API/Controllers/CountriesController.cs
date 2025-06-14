using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Exceptions;
using HotelListing.Net9.Models;
using HotelListing.Net9.Models.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(
    IMapper mapper,
    ICountriesRepository countriesRepository,
    ILogger<CountriesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var getCountryDtos = await countriesRepository.GetAllAsync<GetCountryDto>();
        
        return Ok(getCountryDtos);
    }
    
    // GET: api/GetAllCountriesPaged/?StartIndex=0&pagesize=25&pagenumber=1
    [HttpGet("GetAllCountriesPaged")]
    public async Task<ActionResult<PagedResult<GetCountryDto>>> GetCountriesPaged(
        [FromQuery] QueryParameters queryParameters)
    {
        var pagedCountriesResult = await countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
        
        return Ok(pagedCountriesResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        logger.LogInformation("Looking country {0} up", id);
        var countryDto = await countriesRepository.GetCountryDetailed(id);

        if (countryDto is null)
            throw new NotFoundException(nameof(GetCountry), id);
        
        return Ok(countryDto);
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
        country = await countriesRepository.AddAsync(country);
        
        return CreatedAtAction("GetCountry", new { id = country.Id }, 
            mapper.Map<CountryDto>(country));
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