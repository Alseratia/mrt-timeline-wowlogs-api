using TimelineDatabaseContext;

public class PartialBossStage
{
  public string Name { get; set; } = null!;
  public int AbilityId { get; set; }
  public EventType EventType { get; set; }
  public int Count { get; set; }
}
