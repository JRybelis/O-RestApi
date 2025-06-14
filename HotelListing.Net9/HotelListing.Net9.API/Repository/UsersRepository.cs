using AutoMapper;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Repository;

public class UsersRepository(IMapper mapper, UserManager<ApiUser> userManager, HotelListingDbContext context)
    : GenericRepository<ApiUser>(context, mapper), IUsersRepository
{
    public async Task<IEnumerable<IdentityError>> AddUserToTeams(int id, string roleName)
    {
        var user = mapper.Map<ApiUser>(await GetApiUserByIdAsync(id));
        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Errors;
    }
    
    public async Task<List<BaseApiUserDto?>?> GetApiUsers()
    {
        var users = await userManager.Users.ToListAsync();
        
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            user.Roles = roles.ToArray();
        }
        
        return users.Select(mapper.Map<BaseApiUserDto?>).ToList();
    }
    
    public async Task<GetApiUserDto?> GetApiUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;
        
        var roles = await userManager.GetRolesAsync(user);
        user.Roles = roles.ToArray();

        return mapper.Map<GetApiUserDto>(user);
    }
    
    public async Task<GetApiUserDto?> GetApiUserByIdAsync(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());    
        
        return mapper.Map<GetApiUserDto>(user);
    }
}