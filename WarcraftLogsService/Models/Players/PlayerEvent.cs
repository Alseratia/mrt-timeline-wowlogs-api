namespace WarcraftLogsService.Models;

public class PlayerEvent
{
  public EventType Type { get; set; }
  public int AbsoluteTimer { get; set; }
  public int Number { get; set; }
}