namespace Timeline;

public class TimelineEvent
{
  public int AbsoluteTimer { get; set; }
  public int RelativeTimer { get; set; }
  public string SpellId { get; set; } = null!;
  public TimerOptions Options { get; set; } = new();
}
