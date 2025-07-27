using AutoMapper;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Exceptions;
using HotelListing.Net9.API.Core.Models;
using HotelListing.Net9.API.Core.Models.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IMapper mapper, IHotelsRepository hotelsRepository, ILogger<HotelsController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        logger.LogInformation("Querying all hotels.");
        var hotelDtos = await hotelsRepository.GetAllAsync<HotelDto>();
        
        return Ok(hotelDtos);
    }
    
    [HttpGet("GetAllHotelsPaged")]
    public async Task<ActionResult<PagedResult<HotelDto>>> GetHotelsPaged([FromQuery] QueryParameters queryParameters)
    {
        logger.LogInformation("Querying all hotels, limiting results to {0}, starting from page {1}.", queryParameters.PageSize, queryParameters.PageNumber);
        var pagedHotelsResult = await hotelsRepository.GetAllAsync<HotelDto>(queryParameters);
        
        return Ok(pagedHotelsResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotelDto = await hotelsRepository.GetHotelByIdAsync(id);

        if (hotelDto is null)
            throw new NotFoundException(nameof(GetHotel), id); 
        
        return Ok(hotelDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id)
            return BadRequest("Invalid record id.");
        
        var hotelExists = await HotelExists(id);
        if (!hotelExists)
            throw new NotFoundException(nameof(PutHotel), id);
        
        var hotel = await hotelsRepository.GetAsync(id);
        mapper.Map(hotelDto, hotel);
        await hotelsRepository.UpdateAsync(hotel!);
        
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
        var hotelExists = await HotelExists(id);
        if (!hotelExists)
            throw new NotFoundException(nameof(DeleteHotel), id);

        await hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await hotelsRepository.ExistsAsync(id);
    }
}