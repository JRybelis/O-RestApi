using HotelListing.Net9.Contracts;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class AccountsController(IAuthManager authManager) : ControllerBase
{
    // api/Accounts/register
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] CreateApiUserDto createApiUserDto)
    {
        var errors = await authManager.Register(createApiUserDto);

        if (!errors.Any()) return Ok();
        
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
            
        return BadRequest(ModelState);
    }
    
    // TODO: create action for getting user by email
    // api/Accounts/assignRoles
    [HttpPost]
    [Route("assignRoles")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRoles([FromBody] GetApiUserDto getApiUserDto)
    {
        var errors = new List<IdentityError>();
        
        foreach (var role in getApiUserDto.Roles)
        {
            var roleErrors = await authManager.AddUserToTeam(getApiUserDto.UserId, role);
            errors.AddRange(roleErrors);
        }
        
        if (!errors.Any()) return Ok();

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }
    
    // api/Accounts/login
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginApiUserDto loginApiUserDto)
    {
        var authResponse = await authManager.Login(loginApiUserDto);

        if (authResponse == null) return Unauthorized();

        return Ok(authResponse);
    }
}