using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IHotelsRepository _hotelsRepository;

    public HotelsController(IMapper mapper, IHotelsRepository hotelsRepository)
    {
        _mapper = mapper;
        _hotelsRepository = hotelsRepository;
    }

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var hotels = await _hotelsRepository.GetAllAsync();
        var getHotelDtos = _mapper.Map<List<GetHotelDto>>(hotels);

        return Ok(getHotelDtos);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
            return NotFound();

        var HotelDto = _mapper.Map<HotelDto>(hotel);

        return Ok(HotelDto);
    }

    // PUT: api/Hotels/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updateHotelDto)
    {
        if (id != updateHotelDto.Id)
            return BadRequest("Invalid record Id.");
        
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
            return NotFound();
        
        // sets hotel state to modified
        _mapper.Map(updateHotelDto, hotel);

        try
        {
            await _hotelsRepository.UpdateAsync(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _hotelsRepository.Exists(id))
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

    // POST: api/Hotels
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotelDto)
    {
        var hotel = _mapper.Map<Hotel>(createHotelDto);

        await _hotelsRepository.AddAsync(hotel);

        return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = _hotelsRepository.GetAsync(id);
        if (hotel is null)
            return NotFound();
        
        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }
}