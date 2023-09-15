using WarcraftLogs;
using TimelineDatabaseContext;


/// <summary>
/// Classes for transforming data that come with warcraftlogs into dbo.
/// </summary>
public class ToDboTransformer
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  public ToDboTransformer(WarcraftlogsAnalyzer logAnalyzer)
  {
    _logAnalyzer = logAnalyzer;
  }

  public Boss ToBoss(WLFight fight, WLZone zone)
  {
    return new Boss()
    {
      Id = Guid.NewGuid().ToString(),
      InGameId = fight.EncounterID,
      Name = fight.Name,
      FightDuration = fight.GetFightDuration(),
      ZoneId = zone.Id,
      OrderInRaid = zone.Encounters.TakeWhile(x => x.Name != fight.Name).Count(),
      Difficulty = fight.Difficulty.ToDifficulty(),
      FightType = FightType.STAGE_DEPENDANT_TIMERS
    };
  }

  public Boss ToBoss(WLBoss wlBoss, WLZone zone)
  {
    return new Boss()
    {
      Id = Guid.NewGuid().ToString(),
      InGameId = wlBoss.Id,
      Name = wlBoss.Name,
      FightDuration = 0,
      ZoneId = zone.Id,
      OrderInRaid = zone.Encounters.TakeWhile(x => x.Name != wlBoss.Name).Count(),
      FightType = FightType.STAGE_DEPENDANT_TIMERS
    };
  }

  public BossEvent ToBossEvent(WLEvent event_, BossStage stage, BossAbility ability)
  {
    return new BossEvent()
    {
      Id = Guid.NewGuid().ToString(),
      BossId = stage.BossId,
      StageId = stage.Id,
      EventType = (EventType)event_.Type,
      EventTypeForNote = ((EventType)event_.Type).GetEventTypeForNote(),
      AbilityId = ability.Id,
      AbilityCount = event_.CastNumber,
      AbsoluteTimer = event_.Timestamp.Seconds
    };
  }

  public BossAbility ToAbilityAsync(Boss boss, WLEvent event_, WLAbility ability)
  {
    return new BossAbility()
    {
      Id = Guid.NewGuid().ToString(),
      InGameId = ability.Id,
      Name = ability.Name,
      Icon = $"https://wow.zamimg.com/images/wow/icons/large/{ability.Icon}",
      Duration = event_.Duration,
      IsTracked = false,
      Boss = boss
    };
  }

  public async Task<List<BossAbility>> ToNewAbilities(Boss boss, List<WLEvent> events)
  {
    List<BossAbility> result = new();

    var uniqueEvents = events.DistinctBy(x => x.AbilityGameId);
    var newEventAbilities = uniqueEvents.Where(wlAbility => !boss.Abilities.AsEnumerable()
                                                            .Any(dbAbility => dbAbility.InGameId == wlAbility.AbilityGameId))
                                                            .ToList();

    foreach (var newEvent in newEventAbilities)
    {
      var wlAbility = await _logAnalyzer.GetAbilityAsync(newEvent.AbilityGameId);
      if (wlAbility == null) throw new Exception($"Can't get {newEvent.AbilityGameId} ability from warcraftlogs");

      result.Add(ToAbilityAsync(boss, newEvent, wlAbility));
    }
    return result;
  }

  public List<BossEvent> ToBossEvents(Boss boss, List<WLEvent> events)
  {
    // get this boss stages, can't be 0
    var result = new List<BossEvent>();
    var bossStages = boss.Stages.ToList();
    if (bossStages.Count == 0) throw new Exception($"Can't find stages for boss {boss.Id}");

    // Create BossEvent and bind to stage
    foreach (var event_ in events)
    {
      var eventTimerSec = event_.Timestamp.Seconds;
      var curStage = bossStages.LastOrDefault(stage => stage.StartTimer <= eventTimerSec)!;
      var curAbility = boss.Abilities.LastOrDefault(ability => ability.InGameId == event_.AbilityGameId &&
                                                               ability.BossId == boss.Id)!;

      var newEvent = ToBossEvent(event_, curStage, curAbility);
      newEvent.RelativeTimer = boss.FightType == FightType.STAGE_DEPENDANT_TIMERS ? newEvent.AbsoluteTimer - curStage.StartTimer : null;

      result.Add(newEvent);
    }

    return result;
  }

  public BossStage CreateFirstStage(Boss boss)
  {
    var newStage = new BossStage()
    {
      Id = Guid.NewGuid().ToString(),
      BossId = boss.Id,
      StageNumber = 0,
      StageName = "Start fight",
      StartTimer = 0,
      EndTimer = boss.FightDuration
    };

    return newStage;
  }
}
