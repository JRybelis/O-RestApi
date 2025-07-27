using HotelListing.Net9.API.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.API.Core.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(CreateApiUserDto userDto);
    Task<AuthResponseDto?> Login(LoginApiUserDto userLoginDto);
    Task<string> CreateRefreshToken();
    Task<AuthResponseDto?> VerifyRefreshToken(AuthResponseDto request);
}