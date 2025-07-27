using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Data;
using HotelListing.Net9.API.Core.Contracts;
using HotelListing.Net9.API.Core.Exceptions;
using HotelListing.Net9.API.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.API.Core.Repository;

public class GenericRepository<T>(HotelListingDbContext context, IMapper mapper) : IGenericRepository<T>
    where T : class
{
    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        var result = await context.Set<T>().FindAsync(id);
        if (result is null)
            throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No key provided.");
        
        return mapper.Map<TResult>(result);
    }

    public async Task<List<TResult>> GetAllAsync<TResult>()
    {
        return await context.Set<T>()
            .ProjectTo<TResult>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await context.Set<T>().LongCountAsync();
        var items = await context.Set<T>()
            .Skip(queryParameters.StartIndex)
            .Take(queryParameters.PageSize)
            .ProjectTo<TResult>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TResult>
        {
            Items = items,
            PageNumber = queryParameters.PageNumber,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize
        };
    }

    public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
    {
        var entity = mapper.Map<T>(source);
        
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return mapper.Map<TResult>(entity);
    }

    public async Task UpdateAsync<TSource, TResult>(int id, TSource source)
    {
        var entity = await GetAsync<TResult>(id);
        
        mapper.Map(source, entity);
        context.Update(entity!);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TResult>(int id) where TResult : class
    {
        var entity = await GetAsync<TResult>(id);
        context.Set<TResult>().Remove(entity!);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync<TResult>(int id)
    {
        var entity = await GetAsync<TResult>(id);
        
        return entity is not null;
    }
}