using System.Text.Json;
using StackExchange.Redis;

namespace CacheService.CacheProvider;

public class RedisProvider : ICacheProvider
{
  private readonly ConnectionMultiplexer _connect;
  public RedisProvider(ConnectionMultiplexer connect) =>  _connect = connect;

  public T? GetItem<T>(string key)
  {
    var redis = _connect.GetDatabase();
    var cacheData = redis.StringGet(key);
    return !cacheData.IsNullOrEmpty
      ? JsonSerializer.Deserialize<T>(cacheData!)
      : default;
  }

  public bool SetItem<T>(string key, T item, TimeSpan? expiry)
  {
    var redis = _connect.GetDatabase();
    return redis.StringSet(key, JsonSerializer.Serialize(item), expiry);
  }

  public bool RemoveItem(string key)
  {
    var redis = _connect.GetDatabase();
    return redis.KeyDelete(key);
  }
}