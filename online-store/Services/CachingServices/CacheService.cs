using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace online_store.Services.CachingServices;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private static readonly ConcurrentDictionary<string, bool> cacheKeys = new();  
    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task<T?> GetAsync<T>(string cacheKey) where T : class
    {
        
        string? cachedValue = await _distributedCache.GetStringAsync(cacheKey);
        if (cachedValue is null)
        {
            return null;
        }
        T? value = JsonConvert.DeserializeObject<T>(cachedValue);
        return value;
      
    }

    public async Task RemoveAsync(string cacheKey)
    {
        await _distributedCache.RemoveAsync(cacheKey);
        cacheKeys.TryRemove(cacheKey, out bool _);
    }

    public async Task RemoveByAsync(string prefixKey)
    {
        foreach(string key in cacheKeys.Keys)
        {
            if (key.StartsWith(prefixKey))
            {
                await RemoveAsync(key);
            }
        }
    }

    public async Task SetAsync<T>(string cacheKey, T value) where T : class
    {
        string cachedValue = JsonConvert.SerializeObject(value);
        await _distributedCache.SetStringAsync(cacheKey , cachedValue);
        cacheKeys.TryAdd(cacheKey, true);
    }
}
