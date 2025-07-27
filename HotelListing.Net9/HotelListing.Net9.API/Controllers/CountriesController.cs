using AutoMapper;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Models;
using HotelListing.Net9.API.Core.Models.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(
    IMapper mapper,
    ICountriesRepository countriesRepository,
    ILogger<CountriesController> logger) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        logger.LogInformation("Looking country {0} up", id);
        var countryDto = await countriesRepository.GetCountryDetailed(id);
        
        return Ok(countryDto);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            return BadRequest("Invalid record id.");

        try
        {
            await countriesRepository.UpdateAsync<UpdateCountryDto, Country>(id, updateCountryDto);
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!await CountryExists(id))
                return NotFound();
            
            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = await countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);

        return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCountry(int id)
    {
        await countriesRepository.DeleteAsync<Country>(id);
        
        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await countriesRepository.ExistsAsync<GetCountryDto>(id); 
    }
}