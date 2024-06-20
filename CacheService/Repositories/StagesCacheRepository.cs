using DataAccess.Repositories;

namespace CacheService.Repositories;

public class StagesCacheRepository
{
  private readonly StagesRepository _repository;
  private readonly CacheService _cache;
  public StagesCacheRepository(StagesRepository repository, CacheService cache)
  {
    _repository = repository;
    _cache = cache;
  }

  public ICollection<int> GetStagesActivationIds()
  {
    return _cache.GetOrCreate(_repository.GetStagesActivationIds, "StagesActivationIds")!;
  }
}