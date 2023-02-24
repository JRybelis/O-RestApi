using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;

    public UsersController(IMapper mapper, IUsersRepository usersRepository)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
    {
        var users = await _usersRepository.GetAllAsync();
        var getUserDtos = _mapper.Map<List<GetUserDto>>(users);

        return Ok(getUserDtos);
    }

    // GET: api/Users/byName
    [HttpGet]
    [Route("byName")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsersByFirstAndLastName
    ([FromBody]LookupUsersByNameDto lookupUsersByNameDto)
    {
        var users = await _usersRepository.GetUsersByFirstAndLastName(
            lookupUsersByNameDto.FirstName, lookupUsersByNameDto.LastName);

        var getUserDtos = _mapper.Map<List<GetUserDto>>(users);

        return Ok(getUserDtos);
    }

    // GET: api/Users
    [HttpGet("byEmail")]
    public async Task<ActionResult<GetDetailedUserDto>> GetUserByEmail([FromQuery] string email)
    {
        var user = await _usersRepository.GetUserByEmail(email);

        if (user is null)
            return NotFound();

        var detailedUserDto = _mapper.Map<GetDetailedUserDto>(user);

        return Ok(detailedUserDto);
    }

    // GET: api/Users
    [HttpGet("byUsername")]
    public async Task<ActionResult<GetDetailedUserDto>> GetUserByUsername([FromQuery]string username)
    {
        var user = await _usersRepository.GetUserByUsername(username);

        if (user is null)
            return NotFound();

        var detailedUserDto = _mapper.Map<GetDetailedUserDto>(user);

        return Ok(detailedUserDto);
    }

    // PUT: api/Users/updateRole
    [HttpPut("updateRole")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
    {
        var user = await _usersRepository.GetUserByUsername(updateRoleDto.Username);
        var result = await _usersRepository.UpdateRole(user, updateRoleDto.SetAdminRole);
        
        if(result.Errors.Any())
        {
            var errors = result.Errors.ToList();
            if (updateRoleDto.SetAdminRole)
            {
                return BadRequest($"{errors.FirstOrDefault().Description}");
            }
            else 
            {
                return BadRequest($"{errors.FirstOrDefault().Description}");
            }
        }
        
        return Ok();
    }
}