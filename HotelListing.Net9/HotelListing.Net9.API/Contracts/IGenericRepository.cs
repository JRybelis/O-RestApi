using HotelListing.Net9.Models;

namespace HotelListing.Net9.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(int? id);
    Task<List<TResult>> GetAllAsync<TResult>();
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}