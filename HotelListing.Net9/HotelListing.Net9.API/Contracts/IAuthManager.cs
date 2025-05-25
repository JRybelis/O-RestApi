using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(CreateApiUserDto userDto);
    Task<AuthResponseDto> Login(LoginApiUserDto userLoginDto);
}