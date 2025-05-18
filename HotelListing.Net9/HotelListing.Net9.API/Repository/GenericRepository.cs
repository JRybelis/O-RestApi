using HotelListing.Net9.Contracts;
using HotelListing.Net9.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Net9.Repository;

public class GenericRepository<T>(HotelListingDbContext context) : IGenericRepository<T>
    where T : class
{
    public async Task<T> GetAsync(int? id)
    {
        if (id is null)
            return null;
        
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
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
        var entity = await GetAsync(id);
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetAsync(id);
        
        return entity is not null;
    }
}