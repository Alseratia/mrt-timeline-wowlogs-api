using TimelineDatabaseContext;
using WarcraftLogsAnalyzer;

public static class ToDboTransformerExtentions
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
    return new Zone() { Id = zone.Id, Name = zone.Name };
  }
}

