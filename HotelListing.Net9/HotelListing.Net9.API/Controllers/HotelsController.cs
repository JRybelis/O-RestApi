using AutoMapper;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Models;
using HotelListing.Net9.API.Core.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        logger.LogInformation("Looking hotel {0} up", id);
        var hotelDto = await hotelsRepository.GetHotelByIdAsync(id);

        return Ok(hotelDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id)
            return BadRequest("Invalid record id.");

        try
        {
            await hotelsRepository.UpdateAsync<HotelDto, Hotel>(id, hotelDto);
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!await HotelExists(id))
                return NotFound();
            
            throw;
        }
        
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotelDto)
    {
        var hotel = await hotelsRepository.AddAsync<CreateHotelDto, HotelDto>(createHotelDto);
        
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
        await hotelsRepository.DeleteAsync<Hotel>(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await hotelsRepository.ExistsAsync<HotelDto>(id);
    }
}