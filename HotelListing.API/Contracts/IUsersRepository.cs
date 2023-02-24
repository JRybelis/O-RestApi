using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts;
public interface IUsersRepository : IGenericRepository<ApiUser>
{
    Task<List<ApiUser>> GetUsersByFirstAndLastName(string firstName, string lastName);
    Task<ApiUser> GetUserByEmail(string email);
    Task<ApiUser> GetUserByUsername(string username);
    Task<IdentityResult> UpdateRole(ApiUser user, bool setAdminRole);
}