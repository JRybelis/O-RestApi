using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Exceptions;
using HotelListing.API.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        _logger.LogInformation($"Querying all hotels.");
        var hotels = await _hotelsRepository.GetAllAsync();
        var getHotelDtos = _mapper.Map<List<HotelDto>>(hotels);

        return Ok(getHotelDtos);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        _logger.LogInformation($"Looking hotel {id} up.");
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
            return NotFound();

        var hotelDto = _mapper.Map<HotelDto>(hotel);

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
            throw new NotFoundException(nameof(GetHotel), id);
        
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

        await _hotelsRepository.AddAsync(hotel);

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
            throw new NotFoundException(nameof(GetHotel), id);
        
        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }
}