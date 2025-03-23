using HotelListing.API.Core.Models.Users;
using HotelListing.LIB;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.Contracts;
public interface IUsersRepository : IGenericRepository<ApiUser>
{
    Task<List<GetUserDto>> GetUsersByFirstAndLastName(string firstName, 
    string lastName);
    Task<GetDetailedUserDto> GetUserByEmail(string email);
    Task<GetDetailedUserDto> GetUserByUsername(string username);
    Task<IdentityResult> UpdateRole(UpdateRoleDto updateRoleDto);
}