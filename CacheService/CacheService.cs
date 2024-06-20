using CacheService.CacheProvider;

namespace CacheService;

public class CacheService
{
  private readonly ICacheProvider _provider;

  public CacheService(ICacheProvider provider) => _provider = provider;

  public T? Get<T>(string key) => _provider.GetItem<T>(key);
  public bool Set<T>(string key, T value, TimeSpan? expiry = null) => _provider.SetItem(key, value, expiry);
  
  public T? GetOrCreate<T>(Func<T> getDataFunc, string key, TimeSpan? expiry = null)
  {
    var cacheData = _provider.GetItem<T>(key);
    if (cacheData != null)
    {
      return cacheData;
    }
    var dbData = getDataFunc();
    if (dbData != null) _provider.SetItem(key, dbData, expiry);
    return dbData;
  }

  public async Task<T?> GetOrCreateAsync<T>(Func<Task<T>> getDataFunc, string key, TimeSpan? expiry = null)
  {
    var cacheData = _provider.GetItem<T>(key);
    if (cacheData != null)
    {
      return cacheData;
    }
    var dbData = await getDataFunc();
    if (dbData != null) _provider.SetItem(key, dbData, expiry);
    return dbData;
  }
  
  public bool Remove(string key) => _provider.RemoveItem(key);
}
