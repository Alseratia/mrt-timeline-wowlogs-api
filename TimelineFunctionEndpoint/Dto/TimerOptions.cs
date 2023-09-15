using TimelineDatabaseContext;

namespace Timeline;

public class TimerOptions
{
  public string? EventSpellId { get; set; } 
  public int? CastCount { get; set; }
  public EventTypeForNote? Event { get; set; }
}
