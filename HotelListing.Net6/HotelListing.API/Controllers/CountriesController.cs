using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Country;
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
        var getCountryDtos = await _countriesRepository.GetAllAsync<GetCountryDto>();

        return Ok(getCountryDtos);
    }

    // GET: api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries
    ([FromQuery] QueryParameters queryParameters)
    {
        var pagedCountriesResult = await _countriesRepository
        .GetAllAsync<GetCountryDto>(queryParameters);

        return Ok(pagedCountriesResult);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var countryDto = await _countriesRepository.GetCountryDetailed(id);

        return Ok(countryDto);
    }

    // PUT: api/Countries/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
            throw new BadRequestException(nameof(PutCountry), id);

        try
        {
            await _countriesRepository.UpdateAsync(id, updateCountryDto);
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
    public async Task<ActionResult<CountryDto>> PostCountry(
        CreateCountryDto createCountryDto)
    {
        var createdCountryDto = await _countriesRepository
        .AddAsync<CreateCountryDto, CountryDto>(createCountryDto);

        return CreatedAtAction(nameof(GetCountry), 
        new { id = createdCountryDto.Id }, createdCountryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}