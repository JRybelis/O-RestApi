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

public class AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger) : IAuthManager
{
    private ApiUser? _user;
    private const string LoginProvider = "HotelListingApi";
    private const string RefreshToken = "RefreshToken";

    public async Task<IEnumerable<IdentityError>> Register(CreateApiUserDto userDto)
    {
        _user = mapper.Map<ApiUser>(userDto);
        _user.UserName = userDto.Email;
        
        var result = await userManager.CreateAsync(_user, userDto.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(_user, "FRONTDESK");
        }

        return result.Errors;
    }

    public async Task<AuthResponseDto?> Login(LoginApiUserDto userLoginDto)
    {
        logger.LogInformation("Looking for user with email {1}.", userLoginDto.Email);
        _user = await userManager.FindByEmailAsync(userLoginDto.Email);
        if (_user == null) return null;
        
        var isValidUser =  await userManager.CheckPasswordAsync(_user, userLoginDto.Password);
        if (!isValidUser) {return null;}
            
        var token = await GenerateToken();
        logger.LogInformation("Token generated for user {1} | Token: {2}.", userLoginDto.Email, token);
        
        return new AuthResponseDto()
        {
            Token = token,
            UserId = _user.Id,
            RefreshToken = await CreateRefreshToken() 
        };
    }

    private async Task<string> GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roles = await userManager.GetRolesAsync(_user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await userManager.GetClaimsAsync(_user);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            new Claim("uid", _user.Id)
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
    
    public async Task<string> CreateRefreshToken()
    {
        await userManager.RemoveAuthenticationTokenAsync(_user, LoginProvider, RefreshToken);
        
        var newRefreshToken = await userManager.GenerateUserTokenAsync(_user, LoginProvider, RefreshToken);
        
        await userManager.SetAuthenticationTokenAsync(_user, LoginProvider, RefreshToken, newRefreshToken);
        
        return newRefreshToken; 
    }

    public async Task<AuthResponseDto?> VerifyRefreshToken(AuthResponseDto request)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
        var userName = tokenContent.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        _user = await userManager.FindByEmailAsync(userName);
        if (_user == null || _user.Id != request.UserId)
        {
            return null;
        }
        
        var isValidRefreshToken =
            await userManager.VerifyUserTokenAsync(_user, LoginProvider, RefreshToken, request.RefreshToken);

        if (isValidRefreshToken)
        {
            var token = await GenerateToken();
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        await userManager.UpdateSecurityStampAsync(_user);
        return null;
    }
        
}