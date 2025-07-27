using HotelListing.Data;
using HotelListing.Net9.API.Core.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.API.Core.Contracts;

public interface IUsersRepository : IGenericRepository<ApiUser>
{
    Task<IEnumerable<IdentityError>> AddUserToTeams(int id, string roleName);
    Task<GetApiUserDto?> GetApiUserByEmailAsync(string email);
    Task<GetApiUserDto?> GetApiUserByIdAsync(int id);
    Task<List<BaseApiUserDto?>?> GetApiUsers();
}