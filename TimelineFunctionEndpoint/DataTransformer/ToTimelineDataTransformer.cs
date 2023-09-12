using WarcraftLogsAnalyzer.Models;
using Timeline;

/// <summary>
/// Classes for transforming data that come with warcraftlogs and database into TimelineStoreState models.
/// </summary>
public class ToTimelineDataTransformer
{
  private readonly CacheService _cacheService;
  public ToTimelineDataTransformer(CacheService cacheService)
  {
    _cacheService = cacheService;
  }
  public Player ToPlayer(WLPlayer wlPlayer, WoWSpec dbSpec)
  {
    return new Player()
    {
      id = wlPlayer.Id,
      name = wlPlayer.Name,
      events = new List<TimelineEvent>(),
      specId = dbSpec.id,
      color = dbSpec.color
    };
  }

  public TimelineEvent ToTimelineEvent(WLEvent event_, BossStage? stage)
  {
    var absoluteTimer = (int)(event_.Timestamp / 1000.0);
    return new TimelineEvent()
    {
      absoluteTimer = absoluteTimer,
      relativeTimer = (stage == null || stage.abilityId == null) ? 0 : absoluteTimer - stage.startTimer,
      spellId = event_.AbilityGameId.ToString(),
      options = ToTimerOptions(stage)
    };
  }
  public TimerOptions ToTimerOptions(BossStage? stage)
  {
    return new TimerOptions()
    {
      eventSpellId = stage?.abilityId?.ToString(),
      castCount = stage?.eventCount,
      condEvent = stage?.eventTypeForNote
    };
  }

  // Create dictionary of players
  public Dictionary<string, Player> ToTimelineStoreState(Boss boss, WLReportData report)
  {
    Dictionary<string, Player> players = new();
    var wlPlayers = report.Players;

    var specs = _cacheService.GetSpecs();
    var DBSpecsDps = (specs ?? new List<WoWSpec>()).Where(x => x.role == WoWSpecRole.MELEE || x.role == WoWSpecRole.RANGED).ToList();
    var DBSpecsHealers = (specs ?? new List<WoWSpec>()).Where(x => x.role == WoWSpecRole.HEALER).ToList();
    var DBSpecsTanks = (specs ?? new List<WoWSpec>()).Where(x => x.role == WoWSpecRole.TANK).ToList();

    AddPlayersByClass(players, wlPlayers.Dps, DBSpecsDps);
    AddPlayersByClass(players, wlPlayers.Tanks, DBSpecsTanks);
    AddPlayersBySpec(players, wlPlayers.Healers, DBSpecsHealers);

    AddPlayersCasts(players, report.PlayersCasts, boss);
    players.RemoveAll(x => x.events.Count == 0);

    return players;
  }

  private void AddPlayersByClass(Dictionary<string, Player> players, List<WLPlayer> wlPlayers, List<WoWSpec> dbSpecs)
  {
    foreach (var player in wlPlayers)
    {
      var findSpec = dbSpecs.FirstOrDefault(x => x.inGameClass == player.Type);
      if (findSpec == null) continue;

      AddPlayerToDictionary(players, player, findSpec);
    }
  }

  private void AddPlayersBySpec(Dictionary<string, Player> players, List<WLPlayer> wlPlayers, List<WoWSpec> dbSpecs)
  {
    foreach (var player in wlPlayers)
    {
      var findSpec = dbSpecs.FirstOrDefault(x => x.inGameClass == player.Type && x.name == player.Specs[0].Spec);
      if (findSpec == null) continue;

      AddPlayerToDictionary(players, player, findSpec);
    }
  }

  private void AddPlayerToDictionary(Dictionary<string, Player> players, WLPlayer player, WoWSpec dbSpec)
  {
    players.Add(player.Id.ToString(), ToPlayer(player, dbSpec));
  }

  // Add events to each players in dictionary
  private void AddPlayersCasts(Dictionary<string, Player> players, List<WLEvent> playersCasts, Boss boss)
  {
    // supported abilities in db
    var supportedSpecsAbilities = (_cacheService.GetAbilities() ?? new List<Ability>()).GroupBy(x => x.WoWSpecId);
    var bossStages = boss.stages.OrderBy(x => x.startTimer).ToList();
    // group events by player
    var eventsGroups = playersCasts.GroupBy(x => x.SourceID);

    foreach (var playerCasts in eventsGroups)
    {
      players.TryGetValue(playerCasts.Key.ToString(), out var player);
      if (player == null) continue;

      var supportedPlayerAbilities = supportedSpecsAbilities.FirstOrDefault(x => x.Key == player.specId);
      if (supportedPlayerAbilities == null) continue;

      foreach (var playerCast in playerCasts)
      {
        if (supportedPlayerAbilities.Any(x => x.id == playerCast.AbilityGameId.ToString()))
        {
          var currentStage = bossStages.FirstOrDefault(x => x.endTimer > playerCast.Timestamp / 1000.0);
          player.events.Add(ToTimelineEvent(playerCast, currentStage));
        }
      }
    }
  }
}