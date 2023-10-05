using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WarcraftLogsAnalyzer;
using TimelineDatabaseContext;
using Newtonsoft.Json;
using NRedisStack.Graph.DataTypes;

/// <summary>
/// The service is designed to update the database based on submitted logs.
/// </summary>
public class UpdateDBService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly DatabaseContext _db;
  private readonly ToDboTransformer _ToDboTransformer;
  private readonly ILogger<UpdateDBService> _logger;
  public UpdateDBService(WarcraftlogsAnalyzer logAnalyzer,
                        DatabaseContext db,
                        ToDboTransformer ToDboTransformer,
                        ILogger<UpdateDBService> logger)
  {
    _logAnalyzer = logAnalyzer;
    _db = db;
    _ToDboTransformer = ToDboTransformer;
    _logger = logger;
  }

  public async Task<ActionResult> GetBossFromLog(string code, string fightId)
  {
    var intFightId = await GetFightId(code, fightId);
    if (intFightId == null) return new NotFoundResult();

    var fight = await _logAnalyzer.GetFightAsync(code, (int)intFightId);
    if (fight == null) return new NotFoundResult();

    var zone = await _logAnalyzer.FindBossZoneAsync(fight.EncounterID);
    if (zone == null) return new NotFoundResult();

    return new JsonResult(ToDboTransformer.ToBoss(fight, zone));
  }

  public async Task<ActionResult> GetCompletedBoss(string code, string fightId, Boss boss, 
                                                   IEnumerable<PartialBossStage> partialBossStages)
  {
    var intFightId = await GetFightId(code, fightId);
    if (intFightId == null) return new NotFoundResult();

    AddBossStages(boss, partialBossStages);

    var bossEvents = await _logAnalyzer.GetBossEventsAsync(code, (int)intFightId, true);
    if (bossEvents == null) return new NotFoundResult();

    await AddBossAbilities(boss, bossEvents);
    UpdateBossStages(boss, bossEvents); // Объединить с complite
    AddBossEvents(boss, bossEvents);

    return new JsonResult(boss);
  }

  public async Task<ActionResult> SaveBoss(Boss boss)
  {
    _logger.LogInformation(JsonConvert.SerializeObject(boss));
    if (await _db.Boss.AnyAsync(x => x.InGameId == boss.InGameId && x.Difficulty == boss.Difficulty))
    {
      _logger.LogInformation("Boss exist");
      throw new BadHttpRequestException("Boss already exist");
    }

    _logger.LogInformation("Boss not exist");
    if (boss.Zone != null && _db.Zone.Any(z => z.Id == boss.Zone.Id)) boss.Zone = null;

    
    await _db.Boss.AddAsync(boss);
    await _db.SaveChangesAsync();

    _logger.LogInformation("Boss success save");
    // обновление редиса

    return new OkResult();
  }

  //public async Task<ActionResult> UpdateBoss(Boss boss)
  //{

  //}
  private async Task<int?> GetFightId(string code, string fightId)
  {
    if (fightId == "last") return await _logAnalyzer.GetLastFightId(code);
    if (int.TryParse(fightId, out var parsedFightId)) return parsedFightId;
    return null;
  }

  private static void AddBossStages(Boss boss, IEnumerable<PartialBossStage> partialBossStages)
  {
    boss.Stages = ToDboTransformer.ToBossStages(boss, partialBossStages);
  }

  public async Task AddBossAbilities(Boss boss, List<WLEvent> events)
  {
    boss.Abilities = await _ToDboTransformer.ToNewAbilities(boss, events);
  }

  public void UpdateBossStages(Boss boss, List<WLEvent> events)
  {
    var stages = boss.Stages?.OrderByDescending(x => x.StageNumber).ToList();
    if (stages == null || stages.Count == 0) return;

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

      prev_startTimer = (int)activationEvent.Timestamp.TotalSeconds;
      stage.StartTimer = prev_startTimer;
    }
  }

  public void AddBossEvents(Boss boss, List<WLEvent> events) // TODO привязка от типа боя
  {
    var bossEventsGroups = ToDboTransformer.ToBossEvents(boss, events).GroupBy(x => x.AbilityId);
    var bossAbilities = boss.Abilities?.ToList();
    if (bossAbilities == null || bossAbilities.Count == 0) return;

    foreach (var bossEventGroup in bossEventsGroups)
    {
      var ability = bossAbilities.Find(x => x.Id == bossEventGroup.Key);
      if (ability == null) continue;
      ability.Event = new List<BossEvent>(bossEventGroup);
    }
  }
}

