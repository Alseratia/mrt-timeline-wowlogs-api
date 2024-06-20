using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DataAccess.Repositories;

public class SpecsRepository
{
  private readonly DatabaseContext _db;

  public SpecsRepository(DatabaseContext dbContext) => _db = dbContext;
  
  public ICollection<WoWSpec> GetSpecsWithAbilities(string expansionId)
  {
    return _db.WoWSpecs.AsNoTracking().Include(x => x.Abilities)
      .Where(x => x.ExpansionId == expansionId)
      .Where(x => x.Expansion.Id == expansionId).ToList();
  }
}