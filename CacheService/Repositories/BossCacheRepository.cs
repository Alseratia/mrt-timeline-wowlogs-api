using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories;

namespace CacheService.Repositories;

public class BossCacheRepository
{
  private readonly BossRepository _repository;
  private readonly CacheService _cache;
  public BossCacheRepository(BossRepository repository, CacheService cache)
  {
    _repository = repository;
    _cache = cache;
  }

  public Boss? GetFullBoss(string id)
  {
    return _cache.GetOrCreate(() => GetFullBoss(id), $"FullBoss_{id}");
  }
  
  public Boss? GetFullBoss(int inGameId, Difficulty difficulty)
  {
    return _cache.GetOrCreate(() => _repository.GetFullBoss(inGameId, difficulty), 
      $"FullBoss_{inGameId}_{difficulty}");
  }
}