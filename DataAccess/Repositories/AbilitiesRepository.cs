using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class AbilitiesRepository
{
  private readonly DatabaseContext _db;

  public AbilitiesRepository(DatabaseContext dbContext) => _db = dbContext;
  
  public ICollection<int> GetAllAbilitiesIds()
  {
    var abilities = _db.Abilities.AsNoTracking().Select(a => new { InGameId = a.Id, ExtraIds = a.ExtraIds }).ToList();

    var abilityIds = abilities.Where(x => x.InGameId != "")
      .Select(a => int.Parse(a.InGameId))
      .Union(abilities.SelectMany(a => a.ExtraIds).Select(int.Parse))
      .Distinct();

    return abilityIds.ToList();
  }

}