using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class StagesRepository
{
  private readonly DatabaseContext _db;

  public StagesRepository(DatabaseContext dbContext) => _db = dbContext;
  
  public ICollection<int> GetStagesActivationIds()
  {
    return _db.BossStages.AsNoTracking().Where(x => x.AbilityId != null)
      .Select(x => x.AbilityId!.Value)
      .Distinct().ToList();
  }
}