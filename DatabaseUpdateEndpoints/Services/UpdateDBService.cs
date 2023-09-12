using Timeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarcraftLogsAnalyzer.Models;
using WarcraftLogsAnalyzer;

/// <summary>
/// The service is designed to update the database based on submitted logs.
/// </summary>
public class UpdateDBService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly TimelineDbContext _db;
  private readonly ToDBDataTransformer _toDBDataTransformer;
  public UpdateDBService(WarcraftlogsAnalyzer logAnalyzer,
                        TimelineDbContext db,
                        ToDBDataTransformer toDBDataTransformer)
  {
    _logAnalyzer = logAnalyzer;
    _db = db;
    _toDBDataTransformer = toDBDataTransformer;
  }
  public async Task<ActionResult> UploadRaidBosses(string raidName)
  {
    using var transaction = _db.Database.BeginTransaction();
    var zones = await _logAnalyzer.GetAllZonesAndEncountersAsync();
    if (zones == null) return new NotFoundResult();

    var needZone = zones.FirstOrDefault(z => z.Name == raidName);
    if (needZone == null) return new NotFoundResult();

    var dbZone = _db.Zone.Where(x => x.name == raidName).FirstOrDefault();
    if (dbZone == null)
    {
      dbZone = needZone.ToZone();
      _db.Zone.Add(dbZone);
    }

    foreach (var encounter in needZone.Encounters)
    {
      if (_db.Boss.Where(x => x.inGameId == encounter.Id).FirstOrDefault() == null)
      {
        var mythicBoss = _toDBDataTransformer.ToBoss(encounter, needZone);
        mythicBoss.difficulty = Difficulty.MYTHIC;
        _db.Boss.Add(mythicBoss);
        _db.BossStage.Add(_toDBDataTransformer.CreateFirstStage(mythicBoss));

        var heroicBoss = _toDBDataTransformer.ToBoss(encounter, needZone);
        heroicBoss.difficulty = Difficulty.HEROIC;
        _db.Boss.Add(heroicBoss);
        _db.BossStage.Add(_toDBDataTransformer.CreateFirstStage(heroicBoss));
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

    boss.fightDuration = fight.GetFightDuration();
    await UpdateBossAbilities(boss, fightEvents);
    UpdateBossStages(boss, fightEvents);
    UpdateBossEvents(boss, fightEvents);

    transaction.Commit();
    return new OkObjectResult(boss);
  }

  public async Task<Boss?> GetOrCreateBossAsync(WLFight fight)
  {
    var bossId = fight.EncounterID;
    var boss = _db.Boss.Where(x => x.inGameId == bossId && x.difficulty == fight.Difficulty.ToDifficulty())
                       .FirstOrDefault();

    if (boss == null)
    {
      var zone = await _logAnalyzer.FindBossZoneAsync(bossId);
      if (zone == null) return null;

      var newZone = zone.ToZone();
      if (_db.Zone.Find(newZone.id) == null) _db.Zone.Add(newZone);

      boss = _toDBDataTransformer.ToBoss(fight, zone);
      _db.Boss.Add(boss);

      _db.BossStage.Add(_toDBDataTransformer.CreateFirstStage(boss));
    }
    _db.SaveChanges();
    return boss;
  }

  public async Task UpdateBossAbilities(Boss boss, List<WLEvent> events)
  {
    var newAbilities = await _toDBDataTransformer.ToNewAbilities(boss, events);
    _db.BossAbility.AddRange(newAbilities);
    _db.SaveChanges();
  }

  public void UpdateBossStages(Boss boss, List<WLEvent> events)
  {
    var stages = _db.BossStage.Where(x => x.bossId == boss.id)
                              .OrderByDescending(x => x.stageNumber)
                              .ToList();
    if (stages.Count == 0) return;

    int prev_startTimer = boss.fightDuration;
    foreach (var stage in stages)
    {
      stage.endTimer = prev_startTimer;
      if (stage.IsFirstStage()) continue;

      var activationEvent = events
        .Where(a => a.Type.ToEventType() == stage.eventType && a.AbilityGameId == stage.abilityId)
        .ElementAtOrDefault((Index)(stage.eventCount! - 1));

      if (activationEvent == null)
      {
        throw new Exception("Activation event not found"); // TODO
      }
      prev_startTimer = Convert.ToInt32(activationEvent.Timestamp / 1000.0);
      stage.startTimer = prev_startTimer;
    }
    _db.SaveChanges();
  }

  public void UpdateBossEvents(Boss boss, List<WLEvent> events)
  {
    var bossEvents = _toDBDataTransformer.ToBossEvents(boss, events);
    _db.BossEvent.Where(e => e.bossId == boss.id).ExecuteDelete();
    _db.BossEvent.AddRange(bossEvents);
    _db.SaveChanges();
  }
}