namespace CacheService.CacheProvider;

public interface ICacheProvider
{
  public T? GetItem<T>(string key);
  public bool SetItem<T>(string key, T item, TimeSpan? expiry);
  public bool RemoveItem(string key);
}