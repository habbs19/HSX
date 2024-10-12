namespace HSX.Core.Interfaces;
public interface ICacheService
{
    T GetOrCreate<T>(string cacheKey, Func<T> createItem, TimeSpan cacheDuration);
    void Remove(string cacheKey);
}