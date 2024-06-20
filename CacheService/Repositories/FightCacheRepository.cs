using WarcraftLogsService.Repositories;
using WarcraftLogsService.Models;

namespace CacheService.Repositories;

public class FightCacheRepository
{
  private readonly FightRepository _repository;
  private readonly CacheService _cache;
  
  public FightCacheRepository(FightRepository repository, CacheService cache)
    => (_repository, _cache) = (repository, cache);

  public async Task<Fight?> GetFight(string code, uint fightId,
    ICollection<int> filterBossAbilities, ICollection<int> filterPlayersAbilities, int? sourceId = null)
  {
    return await _cache.GetOrCreateAsync(() => _repository.GetFight(code, fightId, filterBossAbilities, 
      filterPlayersAbilities, sourceId), $"Fight_{code}_{fightId}_{sourceId}", TimeSpan.FromDays(3));
  }
  
  public async Task<Fight?> GetLastFight(string code,
    ICollection<int> filterBossAbilities, ICollection<int> filterPlayersAbilities, int? sourceId = null)
  {
    return await _cache.GetOrCreateAsync(() => _repository.GetLastFight(code, filterBossAbilities, 
      filterPlayersAbilities, sourceId), $"Fight_{code}_{sourceId}_last", TimeSpan.FromDays(3));
  }
}