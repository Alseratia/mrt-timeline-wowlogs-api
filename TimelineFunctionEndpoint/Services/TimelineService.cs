using Microsoft.AspNetCore.Mvc;

using WarcraftLogs;
using TimelineDatabaseContext;

namespace Timeline;

/// <summary>
/// The service is designed to analyze the log and convert it into a form that the client part of the application accepts (TimelineStoreState)
/// </summary>
public class TimelineService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly CacheService _cacheService;
  private readonly ToDtoTransformer _toTimelineTransformer;
  public TimelineService(WarcraftlogsAnalyzer logAnalyzer,
                         CacheService cacheService,
                         ToDtoTransformer toTimelineTransformer)
  {
    _logAnalyzer = logAnalyzer;
    _cacheService = cacheService;
    _toTimelineTransformer = toTimelineTransformer;
  }

  public async Task<ActionResult> GetTimelineStore(string code, int fightId)
  {
    var reportData = await _logAnalyzer.GetReportData(code, fightId, true);
    if (reportData == null) return new NotFoundResult();
    var fight = reportData.Fight;

    var boss = _cacheService.GetBossWithIncludes(fight.EncounterID, (Difficulty)fight.Difficulty);   // TODO handle a conversion error of complexities that are not supported
    if (boss == null) throw new Exception("This boss not exist");

    boss.FightDuration = fight.GetFightDuration();
    UpdateBossStages(boss, reportData.BossEvents);

    var result = new JsonResult(new RootTimelineObject
    {
      TimelineStoreState = _toTimelineTransformer.ToTimelineStoreState(boss, reportData),
      Boss = boss
    });

    return result;
  }

  private static void UpdateBossStages(Boss boss, List<WLEvent> wlEvents)
  {
    var stages = boss.Stages.OrderByDescending(x => x.StageNumber).ToList();
    var events = boss.Events.OrderBy(x => x.AbsoluteTimer).ToList();

    int prev_startTimer = boss.FightDuration;
    foreach (var stage in stages)
    {
      stage.EndTimer = prev_startTimer;
      if (stage.IsFirstStage()) continue;

      var activationEvent = wlEvents.Where(a => (EventType)a.Type == stage.EventType && 
                                                a.AbilityGameId == stage.AbilityId && 
                                                stage.EventCount == a.CastNumber)
                                    .FirstOrDefault();

      if (activationEvent == null)
      {
        stage.StartTimer = prev_startTimer;
        continue;
      }

      prev_startTimer = (int)activationEvent.Timestamp.TotalSeconds;
      MoveEventsTimer(boss, stage, prev_startTimer);
      stage.StartTimer = prev_startTimer;
    }

  }
  private static void MoveEventsTimer(Boss boss, BossStage curStage, int newStartTimer)
  {
    var diffTime = newStartTimer - curStage.StartTimer;
    foreach (var stage in boss.Stages)
    {
      if (stage == curStage || stage.StartTimer < curStage.StartTimer) continue;
      foreach (var event_ in stage.BossEvents)
      {
        if (event_.RelativeTimer != null) event_.AbsoluteTimer += diffTime;
      }
    }
  }
}
