using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.API.Core.Models.Users;
using HotelListing.LIB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelListing.API.Core.Repository;
public class UsersRepository : GenericRepository<ApiUser>, IUsersRepository
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly ILogger<UsersRepository> _logger;

    public UsersRepository(HotelListingDbContext context, IMapper mapper, 
    UserManager<ApiUser> userManager, ILogger<UsersRepository> logger) 
    : base (context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<GetUserDto>> GetUsersByFirstAndLastName(
        string firstName, string lastName)
    {
        _logger.LogInformation($"Looking users {firstName} {lastName} up.");
        
        return await _context.Users.Where(u => 
        u.FirstName == firstName &&
        u.LastName == lastName)
        .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<GetDetailedUserDto> GetUserByEmail(string? email)
    {
        _logger.LogInformation($"Looking user {email} up.");

        var detailedUserDto = await _context.Users.Where(u => u.Email == email)
        .ProjectTo<GetDetailedUserDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

        if (detailedUserDto is null)
            throw new NotFoundException(typeof(ApiUser).Name, 
            email?? "No key provided.");
        
        return detailedUserDto;
    }
    public async Task<GetDetailedUserDto> GetUserByUsername(string? userName)
    {
        _logger.LogInformation($"Looking user {userName} up.");

        var detailedUserDto = await _context.Users.Where(u => u.UserName == userName)
        .ProjectTo<GetDetailedUserDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

        if (detailedUserDto is null)
            throw new NotFoundException(typeof(ApiUser).Name,
            userName?? "No key provided.");

        return detailedUserDto;
    }

    public async Task<IdentityResult> UpdateRole(UpdateRoleDto updateRoleDto)
    {
        var userDto = await GetUserByUsername(updateRoleDto.UserName);
        var user = _mapper.Map<ApiUser>(userDto);

        if (updateRoleDto.SetAdminRole)
        {
            _logger.LogInformation($"Setting administrator role for user {userDto.NormalizedUserName}.");
            return await _userManager.AddToRoleAsync(user, "Administrator");
        }
        _logger.LogInformation($"Removing administrator role from user {userDto.NormalizedUserName}.");
        return await _userManager.RemoveFromRoleAsync(user, "Administrator");
    }
}