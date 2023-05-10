namespace online_store.Services.CachingServices;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string cacheKey ) where T : class;

    Task SetAsync<T> (string cacheKey, T value) where T : class;    

    Task RemoveAsync(string cacheKey);
    Task RemoveByAsync(string prefixKey);
}
