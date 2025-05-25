using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Net9.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class AccountsController(IMapper mapper, IAuthManager authManager, IUsersRepository usersRepository) : ControllerBase
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
    
    // api/Accounts/GetUserByUsername
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("GetUserByUsername")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await usersRepository.GetApiUserByEmailAsync(username);
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }
    
    // api/Accounts/GetUsers
    [HttpGet]
    [Route("GetUsers")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        var users = await usersRepository.GetApiUsers();
        if (!users.Any())
            return NotFound();
        
        return Ok(users);
    }
    
    // api/Accounts/assignRoles
    [HttpPost]
    [Route("assignRoles")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRoles([FromBody] AddUserRolesDto addUserRolesDto)
    {
        var errors = new List<IdentityError>();
        
        foreach (var role in addUserRolesDto.Roles)
        {
            var roleErrors = await usersRepository.AddUserToTeams(addUserRolesDto.Id, role);
            errors.AddRange(roleErrors);
        }
        
        if (!errors.Any()) return Ok();

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PutUser(int id, UpdateApiUserDto updateApiUserDto)
    {
        if (id != updateApiUserDto.Id)
            return BadRequest("User id mismatch.");

        var user = await usersRepository.GetApiUserByIdAsync(id);
        var userExists = await UserExists(id);

        if (!userExists)
            return NotFound();
        
        mapper.Map(updateApiUserDto, user);

        try
        {
            await usersRepository.UpdateAsync(user);
        }
        catch (Exception e)
        {
            if (!userExists)
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiUser>> DeleteUser(int id)
    {
        var user = await usersRepository.GetApiUserByIdAsync(id);

        if (user is null)
            return NotFound();
        
        await usersRepository.DeleteAsync(id);

        return NoContent();
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
    
    // api/Accounts/refreshToken
    [HttpPost]
    [Route("refreshToken")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDto request)
    {
        var authResponse = await authManager.VerifyRefreshToken(request);

        if (authResponse == null) return Unauthorized();

        return Ok(authResponse);
    }
    
    private async Task<bool> UserExists(int id)
    {
        return await usersRepository.ExistsAsync(id);
    }
}