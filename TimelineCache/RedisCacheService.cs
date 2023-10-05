using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Newtonsoft.Json;
using TimelineDatabaseContext;

namespace TimelineCache;

public class RedisCacheService : BaseCacheService
{
  private readonly ConnectionMultiplexer _connect;
  public RedisCacheService(ConnectionMultiplexer connect, IDbContextFactory<DatabaseContext> dbFactory) : base(dbFactory)
  {
    _connect = connect;
  }
  protected override T? GetFromCacheOrDb<T>(string key, Func<DatabaseContext, T?> getDataFromDb) where T : class
  {
    var db = _connect.GetDatabase();
    var cacheData = db.StringGet(key);
    if (cacheData.IsNullOrEmpty)
    {
      using (var _db = _dbFactory.CreateDbContext())
      {
        var dbData = getDataFromDb(_db);
        if (dbData != null)
        {
          db.StringSet(key, JsonConvert.SerializeObject(dbData));
        }
        return dbData;
      }
    }
    return JsonConvert.DeserializeObject<T>(cacheData!);
  }
}
