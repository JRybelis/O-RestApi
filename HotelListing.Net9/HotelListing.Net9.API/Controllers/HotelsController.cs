using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IMapper mapper, IHotelsRepository hotelsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        var hotels = await hotelsRepository.GetAllAsync();
        var records = mapper.Map<List<HotelDto>>(hotels);
        
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsRepository.GetAsync(id);

        if (hotel is null)
            return NotFound();
        
        var record = mapper.Map<HotelDto>(hotel);
        
        return Ok(record);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id)
            return BadRequest("Invalid record id.");
        
        var hotel = await hotelsRepository.GetAsync(id);
        var hotelExists = await HotelExists(id);

        if (!hotelExists)
            return NotFound();
        
        mapper.Map(hotelDto, hotel);

        try
        {
            await hotelsRepository.UpdateAsync(hotel!);
        }
        catch (Exception e)
        {
            if (!hotelExists)
                return NotFound();
            
            throw;
        }
        
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotelDto)
    {
        var hotel = mapper.Map<Hotel>(createHotelDto);
        await hotelsRepository.AddAsync(hotel);
        
        return CreatedAtAction("GetHotel", new { id = hotel.Id }, mapper.Map<HotelDto>(hotel));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<HotelDto>> DeleteHotel(int id)
    {
        var hotel = await hotelsRepository.GetAsync(id);
        
        if (hotel is null)
            return NotFound();

        await hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await hotelsRepository.ExistsAsync(id);
    }
}