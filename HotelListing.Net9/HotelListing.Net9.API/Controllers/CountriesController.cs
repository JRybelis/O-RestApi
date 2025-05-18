using AutoMapper;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(HotelListingDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await context.Countries.ToListAsync();
        var records = mapper.Map<List<GetCountryDto>>(countries);
        
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await context.Countries
            .Include(q => q.Hotels)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (country is null)
            return NotFound();
        
        var record = mapper.Map<CountryDto>(country);
        
        return Ok(record);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            return BadRequest("Invalid record id.");
        
        var country = await context.Countries.FindAsync(id);
        if (country is null)
            return NotFound();
        
        mapper.Map(updateCountryDto, country); // sets country state to modified

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (!CountryExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = mapper.Map<Country>(createCountryDto);
        context.Countries.Add(country);
        await context.SaveChangesAsync();
        
        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Country>> DeleteCountry(int id)
    {
        var country = await context.Countries.FindAsync(id);
        if (country is null)
            return NotFound();
        
        context.Countries.Remove(country);
        await context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool CountryExists(int id)
    {
        return context.Countries.Any(e => e.Id == id);
    }
}