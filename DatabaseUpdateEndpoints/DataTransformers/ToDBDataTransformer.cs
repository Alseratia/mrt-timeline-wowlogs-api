using Timeline;
using WarcraftLogsAnalyzer;
using WarcraftLogsAnalyzer.Models;

/// <summary>
/// Classes for transforming data that come with warcraftlogs into database models.
/// </summary>
public class ToDBDataTransformer
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  public ToDBDataTransformer(WarcraftlogsAnalyzer logAnalyzer)
  {
    _logAnalyzer = logAnalyzer;
  }

  public Boss ToBoss(WLFight fight, WLZone zone)
  {
    return new Boss()
    {
      inGameId = fight.EncounterID,
      name = fight.Name,
      fightDuration = fight.GetFightDuration(),
      zoneId = zone.Id,
      orderInRaid = zone.Encounters.TakeWhile(x => x.Name != fight.Name).Count(),
      difficulty = fight.Difficulty.ToDifficulty(),
    };
  }
  
  public Boss ToBoss(WLBoss wlBoss, WLZone zone)
  {
    return new Boss()
    {
      inGameId = wlBoss.Id,
      name = wlBoss.Name,
      fightDuration = 0,
      zoneId = zone.Id,
      orderInRaid = zone.Encounters.TakeWhile(x => x.Name != wlBoss.Name).Count()
    };
  }

  public BossEvent ToBossEvent(WLEvent event_, BossStage stage, BossAbility ability)
  {
    return new BossEvent()
    {
      bossId = stage.bossId,
      stageId = stage.id!,
      eventType = (EventType)event_.Type,
      eventTypeForNote = ((EventType)event_.Type).GetEventTypeForNote(),
      abilityId = ability.id,
      abilityCount = event_.CastNumber,
      absoluteTimer = (int)(event_.Timestamp / 1000.0)
    };
  }

  public BossAbility ToAbilityAsync(Boss boss, WLEvent event_, WLAbility ability)
  {
    return new BossAbility()
    {
      id = Guid.NewGuid().ToString(),
      inGameId = ability.Id,
      name = ability.Name,
      icon = $"https://wow.zamimg.com/images/wow/icons/large/{ability.Icon}",
      duration = event_.Duration,
      isTracked = false,
      boss = boss
    };
  }

  public async Task<List<BossAbility>> ToNewAbilities(Boss boss, List<WLEvent> events)
  {
    List<BossAbility> result = new();

    var uniqueEvents = events.DistinctBy(x => x.AbilityGameId);
    var newEventAbilities = uniqueEvents.Where(wlAbility => !boss.abilities.AsEnumerable()
                                                            .Any(dbAbility => dbAbility.inGameId == wlAbility.AbilityGameId))
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
    var bossStages = boss.stages.ToList();
    if (bossStages.Count == 0) throw new Exception($"Can't find stages for boss {boss.id}");

    // Create BossEvent and bind to stage
    foreach (var event_ in events)
    {
      var eventTimerSec = (int)(event_.Timestamp / 1000.0);
      var curStage = bossStages.LastOrDefault(stage => stage.startTimer <= eventTimerSec)!;
      var curAbility = boss.abilities.LastOrDefault(ability => ability.inGameId == event_.AbilityGameId &&
                                                               ability.bossId == boss.id)!;

      var newEvent = ToBossEvent(event_, curStage, curAbility);
      newEvent.relativeTimer = boss.fightType == FightType.STAGE_DEPENDANT_TIMERS ? newEvent.absoluteTimer - curStage.startTimer : null;

      result.Add(newEvent);
    }

    return result;
  }

  public BossStage CreateFirstStage(Boss boss)
  {
    var newStage = new BossStage()
    {
      id = Guid.NewGuid().ToString(),
      bossId = boss.id,
      stageNumber = 0,
      stageName = "Start fight",
      startTimer = 0,
      endTimer = boss.fightDuration
    };

    return newStage;
  }
}

public static class ToDBDataTransformerExtentions
{
  public static Difficulty ToDifficulty(this WLDifficulty difficulty)
  {
    return difficulty switch
    {
      WLDifficulty.MYTHIC => Difficulty.MYTHIC,
      WLDifficulty.HEROIC => Difficulty.HEROIC,
    _ => throw new Exception("Unknown difficulty")
    };
  }
  public static EventType ToEventType(this WLEventType type) => (EventType)type;
  
  public static Zone ToZone(this WLZone zone)
  {
      return new Zone() { id = zone.Id, name = zone.Name };
  }
}