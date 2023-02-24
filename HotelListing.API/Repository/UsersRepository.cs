using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository;
public class UsersRepository : GenericRepository<ApiUser>, IUsersRepository
{
    private readonly HotelListingDbContext _context;
    private readonly UserManager<ApiUser> _userManager;

    public UsersRepository(HotelListingDbContext context, 
    UserManager<ApiUser> userManager) : base (context)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<ApiUser>> GetUsersByFirstAndLastName(string firstName, string lastName)
    {
        return await _context.Users.Where(u => 
        u.FirstName == firstName &&
        u.LastName == lastName).ToListAsync();
    }

    public async Task<ApiUser> GetUserByEmail(string? email)
    {
        if (email is null)
            return null;

        return await _context.Users.Where(u => u.Email == email)
        .FirstOrDefaultAsync();
    }
    public async Task<ApiUser> GetUserByUsername(string? username)
    {
        if(username is null)
            return null;
        
        return await _context.Users.Where(u => u.UserName == username)
        .FirstOrDefaultAsync();
    }

    public async Task<IdentityResult> UpdateRole(ApiUser user, bool setAdminRole)
    {
        if (setAdminRole)
        {
            return await _userManager.AddToRoleAsync(user, "Administrator");
        }
        
        return await _userManager.RemoveFromRoleAsync(user, "Administrator");
    }
}