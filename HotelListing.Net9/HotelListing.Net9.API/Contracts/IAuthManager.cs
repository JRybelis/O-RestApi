using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(CreateApiUserDto userDto);
    Task<IEnumerable<IdentityError>> AddUserToTeam(string id, string roleName);
    Task<GetApiUserDto> GetApiUserByEmailAsync(string email);
    Task<AuthResponseDto> Login(LoginApiUserDto userLoginDto);
}