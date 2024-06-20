using DataAccess.Entities;
using DataAccess.Repositories;

namespace CacheService.Repositories;

public class SpecsCacheRepository
{
  private readonly SpecsRepository _repository;
  private readonly CacheService _cache;
  public SpecsCacheRepository(SpecsRepository repository, CacheService cache)
  {
    _repository = repository;
    _cache = cache;
  }
  
  public ICollection<WoWSpec> GetSpecsWithAbilities(string expansionId)
  {
    return _cache.GetOrCreate(() => _repository.GetSpecsWithAbilities(expansionId), $"SpecsWithAbilities_{expansionId}")!;
  }
}