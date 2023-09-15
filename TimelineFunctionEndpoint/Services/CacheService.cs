using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using TimelineDatabaseContext;

namespace Timeline;

public class CacheService
{
  private readonly IMemoryCache _memoryCache;
  private readonly IDbContextFactory<DatabaseContext> _dbFactory;
  private readonly object _cacheLock = new();
  public CacheService(IMemoryCache memoryCache, IDbContextFactory<DatabaseContext> dbFactory)
  {
    _memoryCache = memoryCache;
    _dbFactory = dbFactory;
  }

  public Boss? GetBossWithIncludes(int bossId, Difficulty difficulty)
  {
    string key = $"AllBoss_{bossId}_{difficulty}";

    return GetFromCacheOrDb(key, (_db) => _db.Boss.AsNoTracking()
                                               .Include(x => x.Abilities)
                                               .Include(x => x.Events)
                                               .Include(x => x.Stages)
                                               .FirstOrDefault(x => x.InGameId == bossId && x.Difficulty == difficulty));
  }

  public List<BossAbility>? GetBossAbilities(string bossId)
  {
    string key = $"BossAbilities_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossAbility.AsNoTracking()
                                                      .Where(x => x.BossId == bossId)
                                                      .ToList());
  }

  public List<BossEvent>? GetBossEvents(string bossId)
  {
    string key = $"BossEvents_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossEvent.AsNoTracking()
                                                    .Where(x => x.BossId == bossId)
                                                    .ToList());
  }

  public List<BossStage>? GetBossStages(string bossId)
  {
    string key = $"BossStages_{bossId}";
    return GetFromCacheOrDb(key, (_db) => _db.BossStage.AsNoTracking()
                                                    .Where(x => x.BossId == bossId)
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
  private T? GetFromCacheOrDb<T>(string key, Func<DatabaseContext, T?> getDataFromDb)
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

