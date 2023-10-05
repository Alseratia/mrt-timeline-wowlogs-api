using Timeline;
using TimelineDatabaseContext;
using WarcraftLogsAnalyzer;

namespace TimelineCache;

public class ToDtoTransformer
{
  private readonly ICacheService _cacheService;
  public ToDtoTransformer(ICacheService cacheService)
  {
    _cacheService = cacheService;
  }
  public Player ToPlayer(WLPlayer wlPlayer, WoWSpec dbSpec)
  {
    return new Player()
    {
      Id = wlPlayer.Id,
      Name = wlPlayer.Name,
      Events = new List<TimelineEvent>(),
      SpecId = dbSpec.Id,
      Color = dbSpec.Color
    };
  }

  public TimelineEvent ToTimelineEvent(WLEvent event_, BossStage? stage)
  {
    var absoluteTimer = (int)event_.Timestamp.TotalSeconds;
    return new TimelineEvent()
    {
      AbsoluteTimer = absoluteTimer,
      RelativeTimer = (stage == null || stage.AbilityId == null) ? absoluteTimer : absoluteTimer - stage.StartTimer,
      SpellId = event_.AbilityGameId.ToString(),
      Options = ToTimerOptions(stage)
    };
  }

  public TimerOptions ToTimerOptions(BossStage? stage)
  {
    return new TimerOptions()
    {
      EventSpellId = stage?.AbilityId?.ToString(),
      CastCount = stage?.EventCount,
      Event = stage?.EventTypeForNote
    };
  }

  // Create dictionary of players
  public Dictionary<string, Player> ToTimelineStoreState(Boss boss, WLReportData report)
  {
    Dictionary<string, Player> players = new();
    var wlPlayers = report.Players;

    var specs = _cacheService.GetSpecs();
    var DBSpecsDps = (specs ?? new List<WoWSpec>()).Where(x => x.Role == WoWSpecRole.MELEE || x.Role == WoWSpecRole.RANGED).ToList();
    var DBSpecsHealers = (specs ?? new List<WoWSpec>()).Where(x => x.Role == WoWSpecRole.HEALER).ToList();
    var DBSpecsTanks = (specs ?? new List<WoWSpec>()).Where(x => x.Role == WoWSpecRole.TANK).ToList();

    AddPlayersByClass(players, wlPlayers.Dps, DBSpecsDps);
    AddPlayersByClass(players, wlPlayers.Tanks, DBSpecsTanks);
    AddPlayersBySpec(players, wlPlayers.Healers, DBSpecsHealers);

    AddPlayersCasts(players, report.PlayersCasts, boss);
    players.RemoveAll(x => x.Events.Count == 0);

    return players;
  }

  private void AddPlayersByClass(Dictionary<string, Player> players, List<WLPlayer> wlPlayers, List<WoWSpec> dbSpecs)
  {
    foreach (var player in wlPlayers)
    {
      var findSpec = dbSpecs.FirstOrDefault(x => x.InGameClass == player.Type);
      if (findSpec == null) continue;

      AddPlayerToDictionary(players, player, findSpec);
    }
  }

  private void AddPlayersBySpec(Dictionary<string, Player> players, List<WLPlayer> wlPlayers, List<WoWSpec> dbSpecs)
  {
    foreach (var player in wlPlayers)
    {
      var findSpec = dbSpecs.FirstOrDefault(x => x.InGameClass == player.Type && x.Name == player.Specs[0].Spec);
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
    var supportedSpecsAbilities = (_cacheService.GetAbilities() ?? new List<Ability>()).GroupBy(x => x.WowSpecId);
    var bossStages = boss.Stages.OrderBy(x => x.StartTimer).ToList();
    
    // group events by player
    var eventsGroups = playersCasts.Where(x => x.Type == WLEventType.cast).GroupBy(x => x.SourceID);

    foreach (var playerCasts in eventsGroups)
    {
      players.TryGetValue(playerCasts.Key.ToString(), out var player);
      if (player == null) continue;

      var supportedPlayerAbilities = supportedSpecsAbilities.FirstOrDefault(x => x.Key == player.SpecId);
      if (supportedPlayerAbilities == null) continue;

      foreach (var playerCast in playerCasts)
      {
        if (supportedPlayerAbilities.Any(x => x.Id == playerCast.AbilityGameId.ToString()))
        {
          var currentStage = bossStages.LastOrDefault(x => x.StartTimer <= playerCast.Timestamp.TotalSeconds);
          player.Events.Add(ToTimelineEvent(playerCast, currentStage));
        }
      }
    }
  }
}