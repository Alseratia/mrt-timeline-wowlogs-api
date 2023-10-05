using Microsoft.EntityFrameworkCore;
using TimelineDatabaseContext;

namespace TimelineCache;

public abstract class BaseCacheService : ICacheService
{
  protected readonly IDbContextFactory<DatabaseContext> _dbFactory;

  public BaseCacheService(IDbContextFactory<DatabaseContext> dbFactory)
  {
    _dbFactory = dbFactory;
  }

  public Boss? GetBossWithIncludes(int bossId, Difficulty difficulty)
  {
    string key = $"AllBoss_{bossId}_{difficulty}";
 
    return GetFromCacheOrDb(key, (_db) => _db.Boss.AsNoTracking()
                                               .Include(x => x.Abilities)
                                               .ThenInclude(x => x.Event)
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

  public List<BossStage>? GetBossesStages()
  {
    var key = $"BossesStages";
    return GetFromCacheOrDb(key, (_db) => _db.BossStage.ToList());
  }

  protected abstract T? GetFromCacheOrDb<T>(string key, Func<DatabaseContext, T?> getDataFromDb) where T : class;
}
