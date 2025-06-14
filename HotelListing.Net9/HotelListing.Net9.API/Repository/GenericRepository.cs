using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using HotelListing.Net9.Exceptions;
using HotelListing.Net9.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Repository;

public class GenericRepository<T>(HotelListingDbContext context, IMapper mapper) : IGenericRepository<T>
    where T : class
{
    public async Task<T?> GetAsync(int? id)
    {
        if (id is null)
            throw new NotFoundException(nameof(GetAsync), id);
        
        return await context.Set<T>().FindAsync(id);
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

    public async Task<T> AddAsync(T entity)
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        context.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (await ExistsAsync(id))
        {
            var entity = await GetAsync(id);
            context.Set<T>().Remove(entity!);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetAsync(id);
        
        return entity is not null;
    }
}