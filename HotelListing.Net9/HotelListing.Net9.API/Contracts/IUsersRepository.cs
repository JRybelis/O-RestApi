using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Net9.Contracts;

public interface IUsersRepository : IGenericRepository<ApiUser>
{
    Task<IEnumerable<IdentityError>> AddUserToTeams(int id, string roleName);
    Task<GetApiUserDto?> GetApiUserByEmailAsync(string email);
    Task<ApiUser?> GetApiUserByIdAsync(int id);
    Task<List<BaseApiUserDto?>?> GetApiUsers();
}