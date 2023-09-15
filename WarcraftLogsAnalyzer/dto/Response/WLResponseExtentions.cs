namespace WarcraftLogs;

public static class WLResponseExtentions
{
  public static WLFight? GetFight(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.Fights?[0];
  
  public static List<WLZone>? GetZones(this WLResponse? response)
    => response?.Data?.WorldData?.Zones;

  public static WLAbility? GetAbility(this WLResponse? response)
    => response?.Data?.GameData?.Ability;

  public static WLPlayers? GetPlayers(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.PlayerDetails?.Data?.PlayerDetails;

  public static List<WLEvent>? GetBossEvents(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.BossEvents;

  public static List<WLEvent>? GetBossCasts(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.BossCasts?.Data;

  public static List<WLEvent>? GetBossBuffs(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.BossBuffs?.Data;

  public static List<WLEvent>? GetPlayersCasts(this WLResponse? response)
    => response?.Data?.ReportData?.Report?.PlayersCasts?.Data;

  public static WLReportData? GetReportData(this WLResponse? response)
  {
    if (response.GetFight() == null ||  response.GetBossEvents() == null || 
        response.GetPlayers() == null || response.GetPlayersCasts() == null)
    { 
      return null; 
    }

    return new WLReportData()
    {
      Fight = response.GetFight()!,
      BossEvents = response.GetBossEvents()!,
      Players = response.GetPlayers()!,
      PlayersCasts = response.GetPlayersCasts()!
    };
  }

  /// <summary>
  /// Brings the events to a complete form. Glues all boss events into one array. 
  /// Calculates the timer of events relative to the start of the fight, not the report. 
  /// </summary>
  /// <param name="calcDurationAndNumbers">Fills in additional fields in the event with the cast number and its duration?</param>
  /// <returns></returns>
  public static WLResponse? FinilizeEvents(this WLResponse? response, bool calcDurationAndNumbers)
  {
    response.ConcatBossEvents();
    response.NormalizeEventTimers();

    if (calcDurationAndNumbers) response.CalcBossEventsDuration();
    return response;
  }

  private static List<WLEvent>? ConcatBossEvents(this WLResponse? response)
  {
    var bossCasts = response?.Data?.ReportData?.Report?.BossCasts?.Data;
    var bossBuffs = response?.Data?.ReportData?.Report?.BossBuffs?.Data;
    if (bossCasts == null || bossBuffs == null) return null;

    var bossEvents = bossCasts.Concat(bossBuffs).ToList();
    response!.Data!.ReportData!.Report!.BossEvents = bossEvents;
    return bossEvents;
  }

  private static WLResponse? NormalizeEventTimers(this WLResponse? response)
  {
    var fight = response?.Data?.ReportData?.Report?.Fights?[0];
    if (fight == null) return response;

    var bossCasts = response?.Data?.ReportData?.Report?.BossCasts?.Data;
    var bossBuffs = response?.Data?.ReportData?.Report?.BossBuffs?.Data;
    var playersCasts = response?.Data?.ReportData?.Report?.PlayersCasts?.Data;

    playersCasts?.ForEach(x => x.Timestamp -= fight.StartTime);
    bossCasts?.ForEach(x => x.Timestamp -= fight.StartTime);
    bossBuffs?.ForEach(x => x.Timestamp -= fight.StartTime);
    return response;
  }

  private static WLResponse? CalcBossEventsDuration(this WLResponse? response)
  {
    response?.Data?.ReportData?.Report?.BossEvents?.CalcDurationAndNumber();
    return response;
  }
}
