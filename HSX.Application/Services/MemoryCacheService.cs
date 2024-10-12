using HSX.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HSX.Application.Services;


public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Gets the item from cache if available, otherwise creates it, stores it in cache, and returns it.
    /// </summary>
    public T GetOrCreate<T>(string cacheKey, Func<T> createItem, TimeSpan cacheDuration)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        if (!_memoryCache.TryGetValue(cacheKey, out T cacheEntry))
        {
            // If not in cache, create the item
            cacheEntry = createItem();

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration, // Cache duration
                SlidingExpiration = TimeSpan.FromMinutes(5) // Optional sliding expiration
            };

            // Save the item in cache
            _memoryCache.Set(cacheKey, cacheEntry, cacheEntryOptions);
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        return cacheEntry!;
    }

    /// <summary>
    /// Removes an item from the cache by its key.
    /// </summary>
    public void Remove(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }
}
