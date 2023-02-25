using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.LIB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HotelsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly ILogger<HotelsController> _logger;

    public HotelsController(IMapper mapper, IHotelsRepository hotelsRepository,
    ILogger<HotelsController> logger)
    {
        _mapper = mapper;
        _hotelsRepository = hotelsRepository;
        _logger = logger;
    }

    // GET: api/Hotels/GetAll
    [HttpGet("GetAll")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        _logger.LogInformation($"Querying all hotels.");
        var hotelDtos = await _hotelsRepository.GetAllAsync<HotelDto>();

        return Ok(hotelDtos);
    }

    // GET: api/Hotels/?StartIndex=0&PageSize=25&PageNumber=1
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<PagedResult<HotelDto>>> GetPagedHotels
    ([FromQuery] QueryParameters queryParameters)
    {
        _logger.LogInformation($"Querying all hotels, limiting results to {queryParameters.PageSize}, starting from page {queryParameters.PageNumber}.");
        var pagedHotelsResult = await _hotelsRepository.GetAllAsync<HotelDto>(queryParameters);

        return Ok(pagedHotelsResult);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        _logger.LogInformation($"Looking hotel {id} up.");
        var hotelDto = await _hotelsRepository.GetHotel(id);

        if (hotelDto is null)
            throw new NotFoundException(nameof(GetHotel), id);

        return Ok(hotelDto);
    }

    // PUT: api/Hotels/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id)
            throw new BadRequestException(nameof(PutHotel), id);
        
        var hotel = await _hotelsRepository.GetAsync(id);
        if (hotel is null)
            throw new NotFoundException(nameof(PutHotel), id);
        
        // sets hotel state to modified
        _mapper.Map(hotelDto, hotel);

        try
        {
            await _hotelsRepository.UpdateAsync(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _hotelsRepository.Exists(id))
            {
                throw new NotFoundException(nameof(PutHotel), id);
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Hotels
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var hotel = _mapper.Map<Hotel>(hotelDto);
        hotel = await _hotelsRepository.AddAsync(hotel);

        var createdHotelDto = _mapper.Map<HotelDto>(hotel);

        return CreatedAtAction("GetHotel", new { id = hotel.Id }, createdHotelDto);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);
        if (hotel is null)
            throw new NotFoundException(nameof(DeleteHotel), id);
        
        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }
}