using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Country;
using HotelListing.LIB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(IMapper mapper, 
    ICountriesRepository countriesRepository, 
    ILogger<CountriesController> logger)
    {
        _mapper = mapper;
        _countriesRepository = countriesRepository;
        _logger = logger;
    }

    // GET: api/Countries/GetAll
    [HttpGet("GetAll")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        _logger.LogInformation($"Querying all countries.");
        var getCountryDtos = await _countriesRepository.GetAllAsync<GetCountryDto>();

        return Ok(getCountryDtos);
    }

    // GET: api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries
    ([FromQuery] QueryParameters queryParameters)
    {
        _logger.LogInformation($"Querying all countries, limiting results to {queryParameters.PageSize}, starting from page {queryParameters.PageNumber}.");

        var pagedCountriesResult = await _countriesRepository
        .GetAllAsync<GetCountryDto>(queryParameters);

        return Ok(pagedCountriesResult);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        _logger.LogInformation($"Looking country {id} up.");
        var countryDto = await _countriesRepository.GetCountryDetailed(id);

        if (countryDto is null)
            throw new NotFoundException(nameof(GetCountry), id);

        return Ok(countryDto);
    }

    // PUT: api/Countries/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            throw new BadRequestException(nameof(PutCountry), id);

        var country = await _countriesRepository.GetAsync(id);
        if (country is null)
            throw new NotFoundException(nameof(PutCountry), id);


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
                throw new NotFoundException(nameof(PutCountry), id);
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
    public async Task<ActionResult<Country>> PostCountry(
        CreateCountryDto createCountryDto)
    {
        var country = _mapper.Map<Country>(createCountryDto);
        country = await _countriesRepository.AddAsync(country);

        var createdCountryDto = _mapper.Map<GetCountryDto>(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, 
        createdCountryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);
        if (country is null)
            throw new NotFoundException(nameof(DeleteCountry), id);

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}