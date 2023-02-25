using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts;
public interface IUsersRepository : IGenericRepository<ApiUser>
{
    Task<List<GetUserDto>> GetUsersByFirstAndLastName(string firstName, 
    string lastName);
    Task<GetDetailedUserDto> GetUserByEmail(string email);
    Task<GetDetailedUserDto> GetUserByUsername(string username);
    Task<IdentityResult> UpdateRole(GetDetailedUserDto detailedUserDto, 
    bool setAdminRole);
}