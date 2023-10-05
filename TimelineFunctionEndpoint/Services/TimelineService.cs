using Microsoft.AspNetCore.Mvc;
using TimelineCache;
using TimelineDatabaseContext;
using WarcraftLogsAnalyzer;

namespace Timeline;

/// <summary>
/// The service is designed to analyze the log and convert it into a form that the client part of the application accepts (TimelineStoreState)
/// </summary>
public class TimelineService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly ICacheService _cacheService;
  private readonly ToDtoTransformer _toTimelineTransformer;
  private readonly ILogger<TimelineService> _logger;
  public TimelineService(WarcraftlogsAnalyzer logAnalyzer,
                         ICacheService cacheService,
                         ToDtoTransformer toTimelineTransformer,
                         ILogger<TimelineService> logger)
  {
    _logAnalyzer = logAnalyzer;
    _cacheService = cacheService;
    _toTimelineTransformer = toTimelineTransformer;
    _logger = logger;
  }
  public async Task<ActionResult> GetTimelineStoreLast(string code)
  {
    var fightId = await _logAnalyzer.GetLastFightId(code);
    if (fightId == null) return new NotFoundResult();

    return await GetTimelineStore(code, (uint)fightId);
  }

  public async Task<ActionResult> GetTimelineStore(string code, uint fightId)
  {
    var reportData = await _logAnalyzer.GetReportData(code, fightId);
    if (reportData == null) return new NotFoundResult();
    var fight = reportData.Fight;

    if (fight.Difficulty == null || !Enum.TryParse(fight.Difficulty.ToString(), out Difficulty difficulty)) return new NotFoundResult();
    var boss = _cacheService.GetBossWithIncludes(fight.EncounterID, difficulty);
    if (boss == null) return new NotFoundResult();

    boss.FightDuration = fight.GetFightDuration();
    UpdateBossStages(boss, reportData.BossEvents);

    var result = new JsonResult(new RootTimelineObject
    {
      TimelineStoreState = _toTimelineTransformer.ToTimelineStoreState(boss, reportData),
      Boss = boss
    });;

    return result;
  }

  private static void UpdateBossStages(Boss boss, List<WLEvent> wlEvents)
  {
    var stages = boss.Stages.OrderBy(x => x.StageNumber).ToList();


    foreach (var stage in stages)
    {
      var activationEvent = wlEvents.Where(a => (EventType)a.Type == stage.EventType && 
                                                a.AbilityGameId == stage.AbilityId && 
                                                stage.EventCount == a.CastNumber)
                                    .FirstOrDefault();

      if (activationEvent == null) continue;
      if ((int)activationEvent.Timestamp.TotalSeconds == stage.StartTimer) continue;

      MoveEventsTimer(boss, stage, (int)activationEvent.Timestamp.TotalSeconds);
    }

    var prevStartTimer = boss.FightDuration;
    stages.OrderByDescending(x => x.StageNumber).ToList().ForEach(x =>
    {
      x.EndTimer = prevStartTimer;
      prevStartTimer = x.StartTimer;
    });
  }
  private static void MoveEventsTimer(Boss boss, BossStage curStage, int newStartTimer)
  {
    var diffTime = newStartTimer - curStage.StartTimer;
    var greaterThenStages = boss.Stages.Where(x => x.StageNumber >= curStage.StageNumber).ToList();

    foreach (var ability in boss.Abilities)
    {
      if (ability.Event == null) continue;
      foreach (var abilityEvent in ability.Event)
      {
        if (abilityEvent.RelativeTimer != null && greaterThenStages.Any(x => x.Id == abilityEvent.StageId))
        {
          abilityEvent.AbsoluteTimer += diffTime;
        }
      }
    }
    greaterThenStages.ForEach(x =>
    {
      x.StartTimer += diffTime;
      x.EndTimer += diffTime;
    });
  }
}
