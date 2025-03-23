using HotelListing.API.Core.Models;

namespace HotelListing.API.Core.Contracts;
public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetAsync(int? id);
    Task<TResult>GetAsync<TResult>(int? id);
    Task<List<TResult>> GetAllAsync<TResult>();
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<TResult> AddAsync<TSource, TResult>(TSource source);
    Task UpdateAsync<TSource>(int id, TSource source);
    Task DeleteAsync(int id);
    Task<bool> Exists(int id);
}