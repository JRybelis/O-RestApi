using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Users;
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
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMapper mapper, IUsersRepository usersRepository, 
    ILogger<UsersController> logger)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
        _logger = logger;
    }

    // GET: api/Users/GetAll
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
    {
        var getUserDtos = await _usersRepository.GetAllAsync<GetUserDto>();

        return Ok(getUserDtos);
    }

    // GET: api/Users/?StartIndex=0&PageSize=25&PageNumber=1
    [HttpGet]
    public async Task<ActionResult<PagedResult<GetUserDto>>> GetUsers([FromQuery]
    QueryParameters queryParameters)
    {
        var pagedUsersResult = await _usersRepository.GetAllAsync<GetUserDto>(queryParameters);

        return Ok(pagedUsersResult);
    }

    // GET: api/Users/byName
    [HttpGet]
    [Route("byName")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsersByFirstAndLastName
    ([FromBody] LookupUsersByNameDto lookupUsersByNameDto)
    {
        var getUserDtos = await _usersRepository.GetUsersByFirstAndLastName(
            lookupUsersByNameDto.FirstName, lookupUsersByNameDto.LastName);

        return Ok(getUserDtos);
    }

    // GET: api/Users
    [HttpGet("byEmail")]
    public async Task<ActionResult<GetDetailedUserDto>> GetUserByEmail
    ([FromQuery] string email)
    {
        var detailedUserDto = await _usersRepository.GetUserByEmail(email);

        return Ok(detailedUserDto);
    }

    // GET: api/Users
    [HttpGet("byUsername")]
    public async Task<ActionResult<GetDetailedUserDto>> GetUserByUsername
    ([FromQuery] string username)
    {
        var detailedUserDto = await _usersRepository.GetUserByUsername(username);

        return Ok(detailedUserDto);
    }

    // PUT: api/Users/updateRole
    [HttpPut("updateRole")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
    {
        var result = await _usersRepository.UpdateRole(updateRoleDto);
        
        if(result.Errors.Any())
            throw new BadRequestException(nameof(UpdateRole), updateRoleDto.UserName);
        
        return Ok();
    }
}