using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICountriesRepository _countriesRepository;

    public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
    {
        _mapper = mapper;
        _countriesRepository = countriesRepository;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var getCountryDtos = _mapper.Map<List<GetCountryDto>>(countries);

        return Ok(getCountryDtos);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country is null)
            return NotFound();

        var countryDto = _mapper.Map<CountryDto>(country);

        return Ok(countryDto);
    }

    // PUT: api/Countries/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            return BadRequest("Invalid record Id.");

        var country = await _countriesRepository.GetAsync(id);

        if (country is null)
            return NotFound();

        // sets country state to modified
        _mapper.Map(updateCountryDto, country);

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _countriesRepository.Exists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Countries
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = _mapper.Map<Country>(createCountryDto);

        await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);
        if (country is null)
            return NotFound();

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}