using Newtonsoft.Json;

namespace WarcraftLogsAnalyzer.Models;

// ------------------------- Boss tables ------------------------- //
public class WLBossData
{
  public WLFight Fight { get; set; } = new();
  public List<WLEvent> BossEvents { get; set; } = new();
}

public class WLReportData
{
  public WLFight Fight { get; set; } = new();
  public WLPlayers Players { get; set; } = new();
  public List<WLEvent> BossEvents { get; set; } = new();
  public List<WLEvent> PlayersCasts { get; set; } = new();
}

public class WLZone
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public ICollection<WLBoss> Encounters { get; set; } = new List<WLBoss>();
}

public class WLBoss
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
}

public enum WLDifficulty
{
  MYTHIC = 5,
  HEROIC = 4,
  NORMAL = 3,
  LFR = 2,
  Unknown = 1,
  NaN = 0
}

public class WLFight
{
  public int EncounterID { get; set; }
  public string Name { get; set; } = "";
  public long StartTime { get; set; }
  public long EndTime { get; set; }
  public WLDifficulty Difficulty { get; set; }
  public int GetFightDuration()
    => Convert.ToInt32((EndTime - StartTime) / 1000.0);
}

public class WLAbility
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public string Icon { get; set; } = "";
}

public enum WLEventType
{
  begincast, cast, applybuff, removebuff,
  applydebuff, removedebuff, refreshdebuff,
  applybuffstack, removebuffstack, refreshbuff,
  applydebuffstack, removedebuffstack
};

public class WLEvent
{
  public long Timestamp { get; set; }
  public WLEventType Type { get; set; }
  public int AbilityGameId { get; set; }
  public int SourceID { get; set; }
  [JsonIgnore]
  public int Duration { get; set; }
  [JsonIgnore]
  public int CastNumber { get; set; }
}

// ------------------------- Player tables ------------------------- //

public class WLPlayers
{
  public List<WLPlayer> Dps { get; set; } = new List<WLPlayer>();
  public List<WLPlayer> Healers { get; set; } = new List<WLPlayer>();
  public List<WLPlayer> Tanks { get; set; } = new List<WLPlayer>();
}

public class WLPlayer
{
  public string Name { get; set; } = "";
  public int Id { get; set; }
  public string Type { get; set; } = "";

  public List<WLSpec> Specs { get; set; } = new List<WLSpec>();
}


public class WLSpec
{
  public string Spec { get; set; } = "";
  public int Count { get; set; }
}