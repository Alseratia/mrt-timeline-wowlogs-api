using Timeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// The service is designed to cache queries to the database and retrieve data from the cache or database.
/// </summary>
public class CacheService
{
  private readonly IMemoryCache _memoryCache;
  private readonly IDbContextFactory<TimelineDbContext> _dbFactory;
  private readonly object _cacheLock = new object();
  public CacheService(IMemoryCache memoryCache, IDbContextFactory<TimelineDbContext> dbFactory)
  {
    _memoryCache = memoryCache;
    _dbFactory = dbFactory;
  }

  public Boss? GetBossWithIncludes(int bossId, Difficulty difficulty)
  {
    string key = $"AllBoss_{bossId}_{difficulty}";

    return GetFromCacheOrDb(key, (_db) => _db.Boss.AsNoTracking()
                                               .Include(x => x.abilities)
                                               .Include(x => x.events)
                                               .Include(x => x.stages)
                                               .FirstOrDefault(x => x.inGameId == bossId && x.difficulty == difficulty));
  }

  public List<BossAbility>? GetBossAbilities(string bossId)
  {
    string key = $"BossAbilities_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossAbility.AsNoTracking()
                                                      .Where(x => x.bossId == bossId)
                                                      .ToList());
  }

  public List<BossEvent>? GetBossEvents(string bossId)
  {
    string key = $"BossEvents_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossEvent.AsNoTracking()
                                                    .Where(x => x.bossId == bossId)
                                                    .ToList());
  }

  public List<BossStage>? GetBossStages(string bossId)
  {
    string key = $"BossStages_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossStage.AsNoTracking()
                                                    .Where(x => x.bossId == bossId)
                                                    .ToList());
  }

  public List<WoWSpec>? GetSpecs()
  {
    var key = $"Specs";
    return GetFromCacheOrDb(key, (_db) => _db.WoWSpec.ToList());
  }

  public List<Ability>? GetAbilities()
  {
    var key = $"PlayersAbilities";
    return GetFromCacheOrDb(key, (_db) => _db.Ability.ToList());

  }
  private T? GetFromCacheOrDb<T>(string key, Func<TimelineDbContext, T?> getDataFromDb)
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
      else
      {
        Console.WriteLine($"Get {key} from cache");
      }
      return data;
    }
  }
}

