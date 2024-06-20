using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DataAccess.Repositories;

public class BossRepository
{
  private readonly DatabaseContext _db;

  public BossRepository(DatabaseContext dbContext) => _db = dbContext;
  
  public Boss? GetFullBoss(string id)
  {
    return _db.Bosses.AsNoTracking()
      .Include(x => x.Zone)!
      .ThenInclude(x => x.Expansion)
      .Include(x => x.BossStages)!
      .Include(x => x.BossAbilities)!
      .ThenInclude(x => x.BossEvents)
      .FirstOrDefault(x => x.Id == id);
 }

  public Boss? GetFullBoss(int inGameId, Difficulty difficulty)
  {
    return _db.Bosses.AsNoTracking()
      .Include(x => x.Zone)!
      .ThenInclude(x => x.Expansion)
      .Include(x => x.BossStages)!
      .Include(x => x.BossAbilities)!
      .ThenInclude(x => x.BossEvents)
      .FirstOrDefault(x => x.InGameId == inGameId && x.Difficulty == difficulty.Map<Enums.Difficulty>()); 
  }
}