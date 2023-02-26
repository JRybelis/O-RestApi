using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Exceptions;
using HotelListing.LIB;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public GenericRepository(HotelListingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TEntity> GetAsync(int? id)
    {
        if (id is null)
            throw new NotFoundException(typeof(TEntity).Name, "No key provided.");

        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            throw new NotFoundException(typeof(TEntity).Name, 
            id.HasValue ? id : "No key provided.");

        return _mapper.Map<TResult>(result);
    }

    public async Task<List<TResult>> GetAllAsync<TResult>()
    {
        return await _context.Set<TEntity>()
        .ProjectTo<TResult>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await _context.Set<TEntity>().CountAsync();
        
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

        _mapper.Map(source, entity); // source => entity
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