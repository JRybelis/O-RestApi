using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository;
public class UsersRepository : GenericRepository<ApiUser>, IUsersRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;

    public UsersRepository(HotelListingDbContext context, IMapper mapper, 
    UserManager<ApiUser> userManager) : base (context, mapper)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<List<GetUserDto>> GetUsersByFirstAndLastName(string firstName, string lastName)
    {
        return await _context.Users.Where(u => 
        u.FirstName == firstName &&
        u.LastName == lastName)
        .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<GetDetailedUserDto> GetUserByEmail(string? email)
    {
        if (email is null)
            return null;

        return await _context.Users.Where(u => u.Email == email)
        .ProjectTo<GetDetailedUserDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }
    public async Task<GetDetailedUserDto> GetUserByUsername(string? username)
    {
        if(username is null)
            return null;
        
        return await _context.Users.Where(u => u.UserName == username)
        .ProjectTo<GetDetailedUserDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }

    public async Task<IdentityResult> UpdateRole(GetDetailedUserDto userDto, 
    bool setAdminRole)
    {
        var user = _mapper.Map<ApiUser>(userDto);

        if (setAdminRole)
        {
            return await _userManager.AddToRoleAsync(user, "Administrator");
        }
        
        return await _userManager.RemoveFromRoleAsync(user, "Administrator");
    }
}