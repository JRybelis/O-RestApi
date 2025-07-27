using HotelListing.Net9.API.Core.Models;

namespace HotelListing.Net9.API.Core.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<TResult> GetAsync<TResult>(int? id);
    Task<List<TResult>> GetAllAsync<TResult>();
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<TResult> AddAsync<TSource, TResult>(TSource source);
    Task UpdateAsync<TSource, TResult>(int id, TSource source);
    Task DeleteAsync<TResult>(int id) where TResult : class;
    Task<bool> ExistsAsync<TResult>(int id);
}