using DataAccess.Repositories;

namespace CacheService.Repositories;

public class AbilitiesCacheRepository
{
  private readonly AbilitiesRepository _repository;
  private readonly CacheService _cache;
  
  public AbilitiesCacheRepository(AbilitiesRepository repository, CacheService cache)
  {
    _repository = repository;
    _cache = cache;
  }

  public ICollection<int> GetAllAbilitiesIds()
  {
    return _cache.GetOrCreate(_repository.GetAllAbilitiesIds, "AbilitiesIds")!;
  }
}