using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WarcraftLogs;
using TimelineDatabaseContext;

/// <summary>
/// The service is designed to update the database based on submitted logs.
/// </summary>
public class UpdateDBService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly DatabaseContext _db;
  private readonly ToDboTransformer _ToDboTransformer;
  public UpdateDBService(WarcraftlogsAnalyzer logAnalyzer,
                        DatabaseContext db,
                        ToDboTransformer ToDboTransformer)
  {
    _logAnalyzer = logAnalyzer;
    _db = db;
    _ToDboTransformer = ToDboTransformer;
  }
  public async Task<ActionResult> UploadRaidBosses(string raidName)
  {
    using var transaction = _db.Database.BeginTransaction();
    var zones = await _logAnalyzer.GetAllZonesAndEncountersAsync();
    if (zones == null) return new NotFoundResult();

    var needZone = zones.FirstOrDefault(z => z.Name == raidName);
    if (needZone == null) return new NotFoundResult();

    var dbZone = _db.Zone.Where(x => x.Name == raidName).FirstOrDefault();
    if (dbZone == null)
    {
      dbZone = needZone.ToZone();
      _db.Zone.Add(dbZone);
    }

    foreach (var encounter in needZone.Encounters)
    {
      if (_db.Boss.Where(x => x.InGameId == encounter.Id).FirstOrDefault() == null)
      {
        var mythicBoss = _ToDboTransformer.ToBoss(encounter, needZone);
        mythicBoss.Difficulty = Difficulty.MYTHIC;
        _db.Boss.Add(mythicBoss);
        _db.BossStage.Add(_ToDboTransformer.CreateFirstStage(mythicBoss));

        var heroicBoss = _ToDboTransformer.ToBoss(encounter, needZone);
        heroicBoss.Difficulty = Difficulty.HEROIC;
        _db.Boss.Add(heroicBoss);
        _db.BossStage.Add(_ToDboTransformer.CreateFirstStage(heroicBoss));
      }
    }
    _db.SaveChanges();
    transaction.Commit();
    return new OkResult();
  }

  public async Task<ActionResult> UploadLog(string code, int fightId)
  {
    using var transaction = _db.Database.BeginTransaction();

    var fight = await _logAnalyzer.GetFightAsync(code, fightId);
    if (fight == null) return new NotFoundResult();

    var fightEvents = await _logAnalyzer.GetBossEventsAsync(code, fightId, true);
    if (fightEvents == null) return new NotFoundResult();

    var boss = await GetOrCreateBossAsync(fight);
    if (boss == null) return new NotFoundResult();

    boss.FightDuration = fight.GetFightDuration();
    await UpdateBossAbilities(boss, fightEvents);
    UpdateBossStages(boss, fightEvents);
    UpdateBossEvents(boss, fightEvents);

    transaction.Commit();
    return new OkObjectResult(boss);
  }

  public async Task<Boss?> GetOrCreateBossAsync(WLFight fight)
  {
    var bossId = fight.EncounterID;
    var boss = _db.Boss.Where(x => x.InGameId == bossId && x.Difficulty == fight.Difficulty.ToDifficulty())
                       .FirstOrDefault();

    if (boss == null)
    {
      var zone = await _logAnalyzer.FindBossZoneAsync(bossId);
      if (zone == null) return null;

      var newZone = zone.ToZone();
      if (_db.Zone.Find(newZone.Id) == null) _db.Zone.Add(newZone);

      boss = _ToDboTransformer.ToBoss(fight, zone);
      _db.Boss.Add(boss);

      _db.BossStage.Add(_ToDboTransformer.CreateFirstStage(boss));
    }
    _db.SaveChanges();
    return boss;
  }

  public async Task UpdateBossAbilities(Boss boss, List<WLEvent> events)
  {
    var newAbilities = await _ToDboTransformer.ToNewAbilities(boss, events);
    _db.BossAbility.AddRange(newAbilities);
    _db.SaveChanges();
  }

  public void UpdateBossStages(Boss boss, List<WLEvent> events)
  {
    var stages = _db.BossStage.Where(x => x.BossId == boss.Id)
                              .OrderByDescending(x => x.StageNumber)
                              .ToList();
    if (stages.Count == 0) return;

    int prev_startTimer = boss.FightDuration;
    foreach (var stage in stages)
    {
      stage.EndTimer = prev_startTimer;
      if (stage.IsFirstStage()) continue;

      var activationEvent = events
        .Where(a => a.Type.ToEventType() == stage.EventType && a.AbilityGameId == stage.AbilityId)
        .ElementAtOrDefault((Index)(stage.EventCount! - 1));

      if (activationEvent == null)
      {
        throw new Exception("Activation event not found"); // TODO
      }
      prev_startTimer = Convert.ToInt32(activationEvent.Timestamp / 1000.0);
      stage.StartTimer = prev_startTimer;
    }
    _db.SaveChanges();
  }

  public void UpdateBossEvents(Boss boss, List<WLEvent> events)
  {
    var bossEvents = _ToDboTransformer.ToBossEvents(boss, events);
    _db.BossEvent.Where(e => e.BossId == boss.Id).ExecuteDelete();
    _db.BossEvent.AddRange(bossEvents);
    _db.SaveChanges();
  }
}