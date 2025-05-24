using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.Net9.Repository;

public class AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration) : IAuthManager
{
    public async Task<IEnumerable<IdentityError>> Register(CreateApiUserDto userDto)
    {
        var user = mapper.Map<ApiUser>(userDto);
        user.UserName = userDto.Email;
        
        var result = await userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "FRONTDESK");
        }

        return result.Errors;
    }

    public async Task<IEnumerable<IdentityError>> AddUserToTeam(string id, string roleName)
    {
        var user = await GetApiUserByIdAsync(id);

        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Errors;
    }
    public async Task<AuthResponseDto> Login(LoginApiUserDto userLoginDto)
    {
        var user = await userManager.FindByEmailAsync(userLoginDto.Email);
        var isValidUser =  await userManager.CheckPasswordAsync(user, userLoginDto.Password);
        
        if (isValidUser == false || user == null)
            return null;

        var token = await GenerateToken(user);

        return new AuthResponseDto()
        {
            Token = token,
            UserId = user.Id
        };
    }

    public async Task<GetApiUserDto> GetApiUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        return user == null ? null : mapper.Map<GetApiUserDto>(user);
    }
    
    public async Task<GetApiUserDto> GetUserAsync(string userEmail) =>
        mapper.Map<GetApiUserDto>(await userManager.FindByEmailAsync(userEmail));
    
    private async Task<ApiUser> GetApiUserByIdAsync(string id) =>
        await userManager.FindByIdAsync(id);

    private async Task<string> GenerateToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await userManager.GetClaimsAsync(user);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }.Union(userClaims).Union(roleClaims);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience:configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["JwtSettings:DurationInMinutes"])),
            signingCredentials: credentials
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
        
}