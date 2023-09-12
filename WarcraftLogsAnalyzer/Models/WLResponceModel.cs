namespace WarcraftLogsAnalyzer.Models;

public class WLResponse
{
  public Data? Data { get; set; }
}

public class Data
{
  public WorldData? WorldData { get; set; }
  public GameData? GameData { get; set; }
  public ReportData? ReportData { get; set; }
}

public class WorldData
{
  public List<WLZone>? Zones { get; set; }
}

public class GameData
{
  public WLAbility? Ability { get; set; }
}

public class ReportData
{
  public Report? Report { get; set; }
}

public class Report
{
  public PlayerDetails? PlayerDetails { get; set; }
  public List<WLFight>? Fights { get; set; }
  public PlayersCasts? PlayersCasts { get; set; }
  public BossCasts? BossCasts { get; set; }
  public BossBuffs? BossBuffs { get; set; }
  public List<WLEvent>? BossEvents { get; set; }
}

public class PlayerDetails
{
  public PlayerDetailsData? Data { get; set; }
  public class PlayerDetailsData
  {
    public WLPlayers? PlayerDetails { get; set; }
  }
}

public class PlayersCasts
{
  public List<WLEvent>? Data { get; set; }
}
public class BossCasts
{
  public List<WLEvent>? Data { get; set; }
}
public class BossBuffs
{
  public List<WLEvent>? Data { get; set; }
}