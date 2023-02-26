using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.LIB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelListing.API.Core.Repository;
public class GenericRepository<TEntity> 
    : IGenericRepository<TEntity> where TEntity : class
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GenericRepository<TEntity>> _logger;

    public GenericRepository(HotelListingDbContext context, IMapper mapper,
    ILogger<GenericRepository<TEntity>> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TEntity> GetAsync(int? id)
    {
        _logger.LogInformation($"Looking {nameof(TEntity)} {id} up.");

        var result = await _context.Set<TEntity>().FindAsync(id);
        
        if (result is null)
            throw new NotFoundException(typeof(TEntity).Name, 
            id.HasValue ? id : "No key provided.");

        return result;
    }

    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        _logger.LogInformation($"Looking {nameof(TEntity)} {id} up.");
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            throw new NotFoundException(typeof(TEntity).Name, 
            id.HasValue ? id : "No key provided.");

        return _mapper.Map<TResult>(result);
    }

    public async Task<List<TResult>> GetAllAsync<TResult>()
    {
        _logger.LogInformation($"Querying all {nameof(TEntity)} records.");
        return await _context.Set<TEntity>()
        .ProjectTo<TResult>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await _context.Set<TEntity>().CountAsync();
        
        _logger.LogInformation($"Querying all {nameof(TEntity)} records, limiting results to {queryParameters.PageSize}, starting from page {queryParameters.PageNumber}.");
        // skip to the position on the list of records on the table that the user provides
        // take the required amount of records after it
        // lookup specific mapper configuration and project it on the db
        var records = await _context.Set<TEntity>()
            .Skip(queryParameters.PageNumber)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TResult>
        {
            Records = records,
            PageNumber = queryParameters.PageNumber,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
    {
        var entity = _mapper.Map<TEntity>(source);

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<TResult>(entity);
    }
    
    public async Task UpdateAsync<TSource>(int id, TSource source)
    {
        var entity = await GetAsync(id);

        // source => entity, changes entity state
        _mapper.Map(source, entity); 

        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity is not null;
    }
}