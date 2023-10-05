using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TimelineDatabaseContext;

namespace TimelineCache;

public class MemoryCacheService : BaseCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly object _cacheLock = new();
    public MemoryCacheService(IMemoryCache memoryCache, IDbContextFactory<DatabaseContext> dbFactory) : base(dbFactory)
    {
        _memoryCache = memoryCache;
    }
    protected override T? GetFromCacheOrDb<T>(string key, Func<DatabaseContext, T?> getDataFromDb) where T : class
    {
        lock (_cacheLock)
        {
            _memoryCache.TryGetValue(key, out T? data);

            if (data == null)
            {
                using var db = _dbFactory.CreateDbContext();
                data = getDataFromDb(db);
                if (data != null) _memoryCache.Set(key, data);
            }
            return data;
        }
    }
}

