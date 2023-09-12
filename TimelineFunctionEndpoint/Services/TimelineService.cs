using Timeline;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using WarcraftLogsAnalyzer;
using WarcraftLogsAnalyzer.Models;

/// <summary>
/// The service is designed to analyze the log and convert it into a form that the client part of the application accepts (TimelineStoreState)
/// </summary>
public class TimelineService
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  private readonly CacheService _cacheService;
  private readonly ToTimelineDataTransformer _toTimelineTransformer;
  public TimelineService(WarcraftlogsAnalyzer logAnalyzer,
                         CacheService cacheService,
                         ToTimelineDataTransformer toTimelineTransformer)
  {
    _logAnalyzer = logAnalyzer;
    _cacheService = cacheService;
    _toTimelineTransformer = toTimelineTransformer;
  }

  public async Task<ActionResult> GetTimelineStore(string code, int fightId)
  {
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    var reportData = await _logAnalyzer.GetReportData(code, fightId, true);
    if (reportData == null) return new NotFoundResult();
    var fight = reportData.Fight;

    var boss = _cacheService.GetBossWithIncludes(fight.EncounterID, (Difficulty)fight.Difficulty); // TODO handle a conversion error of complexities that are not supported
    if (boss == null) throw new Exception("This boss not exist");
    boss.fightDuration = fight.GetFightDuration();

    UpdateBossStages(boss, reportData.BossEvents);

    var result = new JsonResult(new RootTimelineObject
    {
      timelineStoreState = _toTimelineTransformer.ToTimelineStoreState(boss, reportData),
      boss = boss
    });
    stopwatch.Stop();
    Console.WriteLine("All time: " + stopwatch.ElapsedMilliseconds + " ms");
    return result;
  }

  private void UpdateBossStages(Boss boss, List<WLEvent> wlEvents)
  {
    var stages = boss.stages.OrderByDescending(x => x.stageNumber).ToList();
    var events = boss.events.OrderBy(x => x.absoluteTimer).ToList();

    int prev_startTimer = boss.fightDuration;
    foreach (var stage in stages)
    {
      stage.endTimer = prev_startTimer;
      if (stage.IsFirstStage()) continue;

      var activationEvent = wlEvents
        .Where(a => (EventType)a.Type == stage.eventType && a.AbilityGameId == stage.abilityId && stage.eventCount == a.CastNumber).FirstOrDefault();

      if (activationEvent == null)
      {
        stage.startTimer = prev_startTimer;
        continue;
      }

      prev_startTimer = Convert.ToInt32(activationEvent.Timestamp / 1000.0);
      MoveEventsTimer(boss, stage, prev_startTimer);
      stage.startTimer = prev_startTimer;
    }

  }
  private void MoveEventsTimer(Boss boss, BossStage curStage, int newStartTimer)
  {
    var diffTime = newStartTimer - curStage.startTimer;
    foreach (var stage in boss.stages)
    {
      if (stage == curStage || stage.startTimer < curStage.startTimer) continue;
      foreach (var event_ in stage.bossEvents)
      {
        if (event_.relativeTimer != null) event_.absoluteTimer += diffTime;
      }
    }
  }
}
