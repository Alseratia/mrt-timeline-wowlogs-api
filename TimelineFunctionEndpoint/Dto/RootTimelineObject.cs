using TimelineDatabaseContext;

namespace Timeline;

public class RootTimelineObject
{
  public Dictionary<string, Player> TimelineStoreState { get; set; } = null!;
  public Boss Boss { get; set; } = null!;
}